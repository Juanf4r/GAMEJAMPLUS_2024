using System;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;
using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;

namespace ProjectDawn.SplitScreen
{
    [BurstCompile]
    public struct SplitScreen3BalanceJob : IJob
    {
        public SplitScreen3 SplitScreen;
        public Balancing Balancing;

        public void Execute()
        {
            SplitScreen.Balance(Balancing);
        }
    }

    /// <summary>
    /// Split screen implementation for 3 players.
    /// </summary>
    public struct SplitScreen3 : ISplitScreen, IDisposable
    {
        public VoronoiBuilder VoronoiBuilder;
        public VoronoiDiagram VoronoiDiagram;
        public NativeArray<float2> Sites;

        float3 m_Position0;
        float3 m_Position1;
        float3 m_Position2;
        float m_Scale;

        public bool IsCreated => VoronoiDiagram.IsCreated && Sites.IsCreated;

        public SplitScreen3(Allocator allocator = Allocator.Temp)
        {
            m_Position0 = float3.zero;
            m_Position1 = float3.zero;
            m_Position2 = float3.zero;
            m_Scale = 0;
            VoronoiBuilder = new VoronoiBuilder(allocator);
            VoronoiDiagram = new VoronoiDiagram(allocator);
            Sites = new NativeArray<float2>(3, allocator);
        }

        /// <summary>
        /// Recreate split screen with set of player positions.
        /// </summary>
        public void Reset(float3 p0, float3 p1, float3 p2, float scale = 2, float aspectRatio = 1, float BoundsRadius = 0.3f)
        {
            CheckIsCreated();

            m_Position0 = p0;
            m_Position1 = p1;
            m_Position2 = p2;

            Sites[0] = p0.xy;
            Sites[1] = p1.xy;
            Sites[2] = p2.xy;

            var localBounds = VoronoiBounds.CreateFromSites(aspectRatio, Sites, BoundsRadius);
            m_Scale = scale / localBounds.width;

            VoronoiBuilder.SetSites(p0.xy, p1.xy, p2.xy);
            VoronoiBuilder.Construct(ref VoronoiDiagram, localBounds);
        }

        /// <summary>
        /// Balance screen regions to similar area.
        /// </summary>
        public void Balance(in Balancing balanceSettings)
        {
            CheckIsCreated();
            var normalization = new VoronoiNormalizationScope(ref VoronoiBuilder, ref VoronoiDiagram, ref Sites);
            {
                float4 position =  float4.zero;
                position = float4.zero;
                if (balanceSettings.Enabled)
                {
                    UniformTransformRelaxation.Execute(ref VoronoiBuilder, ref VoronoiDiagram, ref Sites, ref position, 200, 0.009f);
                }
            }
            normalization.Dispose(); // Burst do not support "using"
        }

        /// <summary>
        /// Returns the value of how much the screens are balanced. Where 0 means fully balanced.
        /// </summary>
        public float GetBalanceError()
        {
            CheckIsCreated();
            float area = (VoronoiDiagram.Bounds.width * VoronoiDiagram.Bounds.height);
            float3 values = new float3(
                VoronoiDiagram.GetRegionArea(VoronoiDiagram.Regions[0]),
                VoronoiDiagram.GetRegionArea(VoronoiDiagram.Regions[1]),
                VoronoiDiagram.GetRegionArea(VoronoiDiagram.Regions[2]));
            float3 diff = (values / area) - 1f/3f;
            return math.sqrt(math.dot(diff, diff));
        }

        public void UpdatePositions(float3 p0, float3 p1, float3 p2)
        {
            m_Position0 = p0;
            m_Position1 = p1;
            m_Position2 = p2;
        }

        /// <summary>
        /// Creates players screen regions.
        /// </summary>
        public void CreateScreens(in Translating translating, ref ScreenRegions screenRegions)
        {
            CheckIsCreated();

            var bounds = VoronoiDiagram.Bounds;

            VoronoiRegion S0 = VoronoiDiagram.Regions[0];
            VoronoiRegion S1 = VoronoiDiagram.Regions[1];
            VoronoiRegion S2 = VoronoiDiagram.Regions[2];

            float2 center0 = VoronoiDiagram.GetCentroid(S0);
            float2 center1 = VoronoiDiagram.GetCentroid(S1);
            float2 center2 = VoronoiDiagram.GetCentroid(S2);

            float2 player0 = m_Position0.xy;
            float2 player1 = m_Position1.xy;
            float2 player2 = m_Position2.xy;

            float distance01;
            float distance02;
            float distance12;
            if (translating.BlendShape == BlendShape.Region)
            {
                distance01 = VoronoiDiagram.GetDistanceBetweenRegions(S0, m_Scale, -center0 * m_Scale + player0, S1, m_Scale, -center1 * m_Scale + player1);
                distance02 = VoronoiDiagram.GetDistanceBetweenRegions(S0, m_Scale, -center0 * m_Scale + player0, S2, m_Scale, -center2 * m_Scale + player2);
                distance12 = VoronoiDiagram.GetDistanceBetweenRegions(S1, m_Scale, -center1 * m_Scale + player1, S2, m_Scale, -center2 * m_Scale + player2);
            }
            else
            {
                var circle0 = new Circle(player0, m_Scale * VoronoiDiagram.GetRegionInscribedCircleRadius(S0));
                var circle1 = new Circle(player1, m_Scale * VoronoiDiagram.GetRegionInscribedCircleRadius(S1));
                var circle2 = new Circle(player2, m_Scale * VoronoiDiagram.GetRegionInscribedCircleRadius(S2));

                distance01 = Circle.GetDistance(circle0, circle1);
                distance02 = Circle.GetDistance(circle0, circle2);
                distance12 = Circle.GetDistance(circle1, circle2);
            }

            float3 cc0;
            float3 cc1;
            float3 cc2;
            switch (translating.Centering)
            {
                case CameraCentering.PlayerCentered:
                    cc0 = m_Position0;
                    cc1 = m_Position1;
                    cc2 = m_Position2;
                    break;
                case CameraCentering.ScreenCentered:
                    {
                        float2 centerOffset0 = center0 - new float2(bounds.center);
                        centerOffset0 *= m_Scale;

                        float2 centerOffset1 = center1 - new float2(bounds.center);
                        centerOffset1 *= m_Scale;

                        float2 centerOffset2 = center2 - new float2(bounds.center);
                        centerOffset2 *= m_Scale;

                        cc0 = new float3(player0 - centerOffset0, m_Position0.z);
                        cc1 = new float3(player1 - centerOffset1, m_Position1.z);
                        cc2 = new float3(player2 - centerOffset2, m_Position2.z);
                        break;
                    }
                case CameraCentering.None:
                    {
                        float2 centerOffset0 = player0 - new float2(bounds.center);
                        centerOffset0 *= m_Scale;

                        float2 centerOffset1 = player1 - new float2(bounds.center);
                        centerOffset1 *= m_Scale;

                        float2 centerOffset2 = player2 - new float2(bounds.center);
                        centerOffset2 *= m_Scale;

                        cc0 = new float3(player0 - centerOffset0, m_Position0.z);
                        cc1 = new float3(player1 - centerOffset1, m_Position1.z);
                        cc2 = new float3(player2 - centerOffset2, m_Position2.z);
                        break;
                    }

                default:
                    throw new NotImplementedException();
            }

            // Calculate blend values
            var blend = translating.Blend;
            float start = blend;
            float end = 0;
            float blend01 = Blend.LinearSafe(distance01, start, end);
            float blend02 = Blend.LinearSafe(distance02, start, end);
            float blend12 = Blend.LinearSafe(distance12, start, end);
            Blend.ForceToTriangle(ref blend01, ref blend12, ref blend02);

            // Blend world positions
            Blend.BlendTriangle(
                cc0, cc1, cc2,
                blend01, blend12, blend02,
                out float3 p0, out float3 p1, out float3 p2);

                float2 boundsOffset = -new float2(bounds.center);
            float2 boundsScale = 2f / new float2(bounds.size);

            // Blend uv positions
            float2 u0;
            float2 u1;
            float2 u2;
            switch (translating.Centering)
            {
                case CameraCentering.PlayerCentered:
                    float2 au = (center0 + boundsOffset) * boundsScale;
                    float2 bu = (center1 + boundsOffset) * boundsScale;
                    float2 cu = (center2 + boundsOffset) * boundsScale;

                    Blend.BlendTriangle(
                        au, bu, cu,
                        blend01, blend12, blend02,
                        out u0, out u1, out u2);
                        break;
                case CameraCentering.ScreenCentered:
                case CameraCentering.None:
                    u0 = 0;
                    u1 = 0;
                    u2 = 0;
                    break;
                
                default:
                    throw new NotImplementedException();
            }

            // Update screen regions
            screenRegions.Clear();
            screenRegions.AddRegion(VoronoiDiagram, S0, p0, u0);
            screenRegions.AddRegion(VoronoiDiagram, S1, p1, u1);
            screenRegions.AddRegion(VoronoiDiagram, S2, p2, u2);
            screenRegions.AddSplit(VoronoiDiagram, S0, S1, blend01);
            screenRegions.AddSplit(VoronoiDiagram, S1, S2, blend12);
            screenRegions.AddSplit(VoronoiDiagram, S0, S2, blend02);
        }

        public void Dispose()
        {
            CheckIsCreated();
            VoronoiBuilder.Dispose();
            VoronoiDiagram.Dispose();
            Sites.Dispose();
        }

        public void DrawDelaunayDual()
        {
            CheckIsCreated();
            new Triangle(Sites[0], Sites[1], Sites[2]).Draw(Color.black);
        }

        public void DrawRegions(BlendShape blendShape)
        {
            CheckIsCreated();

            var regions = VoronoiDiagram.Regions;

            VoronoiRegion S0 = regions[0];
            VoronoiRegion S1 = regions[1];
            VoronoiRegion S2 = regions[2];

            float2 center0 = VoronoiDiagram.GetCentroid(S0);
            float2 center1 = VoronoiDiagram.GetCentroid(S1);
            float2 center2 = VoronoiDiagram.GetCentroid(S2);

            float2 player0 = m_Position0.xy;
            float2 player1 = m_Position1.xy;
            float2 player2 = m_Position2.xy;

            VoronoiDiagram.DrawRegion(S0, PlayerColor.PlayerA);
            VoronoiDiagram.DrawRegion(S1, PlayerColor.PlayerB);
            VoronoiDiagram.DrawRegion(S2, PlayerColor.PlayerC);

            // Draw centers
            Gizmos.DrawPoint(center0, 0.003f * VoronoiDiagram.Bounds.width, Color.white);
            Gizmos.DrawPoint(center1, 0.003f * VoronoiDiagram.Bounds.width, Color.white);
            Gizmos.DrawPoint(center2, 0.003f * VoronoiDiagram.Bounds.width, Color.white);

            // Draw screen regions
            if (blendShape == BlendShape.Region)
            {
                VoronoiDiagram.DrawRegion(S0, m_Scale, -center0 * m_Scale + player0, new Color32(255, 255, 255, 128));
                VoronoiDiagram.DrawRegion(S1, m_Scale, -center1 * m_Scale + player1, new Color32(255, 255, 255, 128));
                VoronoiDiagram.DrawRegion(S2, m_Scale, -center2 * m_Scale + player2, new Color32(255, 255, 255, 128));
            }
            else
            {
                Gizmos.DrawPoint(player0, m_Scale * VoronoiDiagram.GetRegionInscribedCircleRadius(S0), new Color32(255, 255, 255, 128));
                Gizmos.DrawPoint(player1, m_Scale * VoronoiDiagram.GetRegionInscribedCircleRadius(S1), new Color32(255, 255, 255, 128));
                Gizmos.DrawPoint(player2, m_Scale * VoronoiDiagram.GetRegionInscribedCircleRadius(S2), new Color32(255, 255, 255, 128));
            }

            // Draw inscribed circles
            Gizmos.DrawPointWire(center0, VoronoiDiagram.GetRegionInscribedCircleRadius(S0), Color.white);
            Gizmos.DrawPointWire(center1, VoronoiDiagram.GetRegionInscribedCircleRadius(S1), Color.white);
            Gizmos.DrawPointWire(center2, VoronoiDiagram.GetRegionInscribedCircleRadius(S2), Color.white);
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        void CheckIsCreated()
        {
            if (!IsCreated)
                throw new Exception("Split screen must be created.");
        }
    }
}
