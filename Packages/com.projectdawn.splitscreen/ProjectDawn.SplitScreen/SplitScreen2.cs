using System;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;

namespace ProjectDawn.SplitScreen
{
    /// <summary>
    /// Split screen implementation for 2 players.
    /// </summary>
    public struct SplitScreen2 : ISplitScreen, IDisposable
    {
        public VoronoiBuilder VoronoiBuilder;
        public VoronoiDiagram VoronoiDiagram;
        public NativeArray<float2> Sites;

        float3 m_Position0;
        float3 m_Position1;
        float m_Scale;

        public bool IsCreated => VoronoiDiagram.IsCreated && Sites.IsCreated;

        public SplitScreen2(Allocator allocator = Allocator.Temp)
        {
            m_Position0 = float3.zero;
            m_Position1 = float3.zero;
            m_Scale = 0;
            VoronoiBuilder = new VoronoiBuilder(allocator);
            VoronoiDiagram = new VoronoiDiagram(allocator);
            Sites = new NativeArray<float2>(2, allocator);
        }

        /// <summary>
        /// Recreate split screen with set of player positions.
        /// </summary>
        public void Reset(float3 p0, float3 p2, float scale = 2, float aspectRatio = 1, float BoundsRadius = 0.3f)
        {
            CheckIsCreated();

            m_Position0 = p0;
            m_Position1 = p2;

            Sites[0] = p0.xy;
            Sites[1] = p2.xy;

            var localBounds = VoronoiBounds.CreateFromSites(aspectRatio, Sites, BoundsRadius);
            m_Scale = scale / localBounds.width;

            VoronoiBuilder.SetSites(p0.xy, p2.xy);
            VoronoiBuilder.Construct(ref VoronoiDiagram, localBounds);
        }

        public void UpdatePositions(float3 p0, float3 p1)
        {
            m_Position0 = p0;
            m_Position1 = p1;
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

            float2 center0 = VoronoiDiagram.GetCentroid(S0);
            float2 center1 = VoronoiDiagram.GetCentroid(S1);

            float2 player0 = m_Position0.xy;
            float2 player1 = m_Position1.xy;

            float distance01;
            if (translating.BlendShape == BlendShape.Region)
            {
                distance01 = VoronoiDiagram.GetDistanceBetweenRegions(S0, m_Scale, -center0 * m_Scale + player0, S1, m_Scale, -center1 * m_Scale + player1);
            }
            else
            {
                var circle0 = new Circle(player0, m_Scale * VoronoiDiagram.GetRegionInscribedCircleRadius(S0));
                var circle1 = new Circle(player1, m_Scale * VoronoiDiagram.GetRegionInscribedCircleRadius(S1));

                distance01 = Circle.GetDistance(circle0, circle1);
            }

            float3 cc0;
            float3 cc1;
            switch (translating.Centering)
            {
                case CameraCentering.PlayerCentered:
                    cc0 = m_Position0;
                    cc1 = m_Position1;
                    break;
                case CameraCentering.ScreenCentered:
                    {
                        float2 centerOffset0 = center0 - new float2(bounds.center);
                        centerOffset0 *= m_Scale;

                        float2 centerOffset1 = center1 - new float2(bounds.center);
                        centerOffset1 *= m_Scale;

                        cc0 = new float3(player0 - centerOffset0, m_Position0.z);
                        cc1 = new float3(player1 - centerOffset1, m_Position1.z);
                        break;
                    }
                case CameraCentering.None:
                    {
                        float2 centerOffset0 = player0 - new float2(bounds.center);
                        centerOffset0 *= m_Scale;

                        float2 centerOffset1 = player1 - new float2(bounds.center);
                        centerOffset1 *= m_Scale;

                        cc0 = new float3(player0 - centerOffset0, m_Position0.z);
                        cc1 = new float3(player1 - centerOffset1, m_Position1.z);
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
            Blend.BlendLine(
                cc0, cc1,
                blend01,
                out float3 p0, out float3 p1);

            float2 boundsOffset = -new float2(bounds.center);
            float2 boundsScale = 2f / new float2(bounds.size);

            // Blend uv positions
            float2 u0;
            float2 u1;
            switch (translating.Centering)
            {
                case CameraCentering.PlayerCentered:
                    float2 au = (center0 + boundsOffset) * boundsScale;
                    float2 bu = (center1 + boundsOffset) * boundsScale;

                    Blend.BlendLine(
                        au, bu,
                        blend01,
                        out u0, out u1);
                        break;
                case CameraCentering.ScreenCentered:
                case CameraCentering.None:
                    u0 = 0;
                    u1 = 0;
                    break;
                
                default:
                    throw new NotImplementedException();
            }

            // Update screen regions
            screenRegions.Clear();
            screenRegions.AddRegion(VoronoiDiagram, S0, p0, u0);
            screenRegions.AddRegion(VoronoiDiagram, S1, p1, u1);
            screenRegions.AddSplit(VoronoiDiagram, S0, S1, blend01);
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
            var player0 = Sites[0];
            var player1 = Sites[1];
            new Line(player0, player1).Draw(Color.black);
        }

        public void DrawRegions(BlendShape blendShape)
        {
            CheckIsCreated();

            var regions = VoronoiDiagram.Regions;

            VoronoiRegion S0 = regions[0];
            VoronoiRegion S1 = regions[1];

            float2 center0 = VoronoiDiagram.GetCentroid(S0);
            float2 center1 = VoronoiDiagram.GetCentroid(S1);

            float2 player0 = m_Position0.xy;
            float2 player1 = m_Position1.xy;

            VoronoiDiagram.DrawRegion(S0, PlayerColor.PlayerA);
            VoronoiDiagram.DrawRegion(S1, PlayerColor.PlayerB);

            // Draw centers
            Gizmos.DrawPoint(center0, 0.01f * VoronoiDiagram.Bounds.width, Color.white);
            Gizmos.DrawPoint(center1, 0.01f * VoronoiDiagram.Bounds.width, Color.white);

            // Draw screen regions
            if (blendShape == BlendShape.Region)
            {
                VoronoiDiagram.DrawRegion(S0, m_Scale, -center0 * m_Scale + player0, new Color32(255, 255, 255, 128));
                VoronoiDiagram.DrawRegion(S1, m_Scale, -center1 * m_Scale + player1, new Color32(255, 255, 255, 128));
            }
            else
            {
                Gizmos.DrawPoint(player0, m_Scale * VoronoiDiagram.GetRegionInscribedCircleRadius(S0), new Color32(255, 255, 255, 128));
                Gizmos.DrawPoint(player1, m_Scale * VoronoiDiagram.GetRegionInscribedCircleRadius(S1), new Color32(255, 255, 255, 128));
            }

            // Draw inscribed circles
            Gizmos.DrawPointWire(center0, VoronoiDiagram.GetRegionInscribedCircleRadius(S0), Color.white);
            Gizmos.DrawPointWire(center1, VoronoiDiagram.GetRegionInscribedCircleRadius(S1), Color.white);
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        void CheckIsCreated()
        {
            if (!IsCreated)
                throw new Exception("Split screen must be created.");
        }
    }
}
