using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using System;

namespace ProjectDawn.SplitScreen
{
    /// <summary>
    /// <see cref="SplitScreenEffect"/> will invoke on all components <see cref="OnSplitScreenTargetPosition"/> that implement this interface during the target position fetching.
    /// This is useful if you want to modify target position for specific screen.
    /// </summary>
    public interface ISplitScreenTargetPosition
    {
        float3 OnSplitScreenTargetPosition(int screenIndex, float3 positionWS);
    }

    public interface ISplitScreenTranslatingPosition
    {
        float3 OnSplitScreenTranslatingPosition(int screenIndex, float3 positionWS);
    }

    /// <summary>
    /// <see cref="SplitScreenEffect"/> will invoke on all components <see cref="OnSplitScreenCommandBuffer"/> that implement this interface after command buffer is filled.
    /// This is useful if you want to additional graphics commands into the command buffer.
    /// Check <see cref="DrawSplitsModifier"/>.
    /// </summary>
    public interface ISplitScreenCommandBuffer
    {
        void OnSplitScreenCommandBuffer(CommandBuffer commandBuffer, in ScreenRegions screenRegions);
#if RENDER_PIPELINES_CORE_7_0_13_OR_NEWER
        void OnSplitScreenCommandBuffer(RasterCommandBuffer commandBuffer, in ScreenRegions screenRegions);
#endif
    }

    /// <summary>
    /// <see cref="SplitScreenEffect"/> will invoke on all components <see cref="OnSplitScreenVoronoi"/> that implement this interface after Voronoi construction.
    /// This is useful if you want to to modify voronoi diagram.
    /// </summary>
    public interface ISplitScreenVoronoi
    {
        void OnSplitScreenVoronoi(ref VoronoiBuilder voronoiBuilder, ref VoronoiDiagram voronoiDiagram, ref NativeArray<float2> sites);
    }

    [Obsolete("Use ISplitScreenVoronoi")]
    public interface ISplitScreenBalancing
    {
        void OnSplitScreenBalancing(VoronoiBuilder voronoiBuilder, VoronoiDiagram voronoiDiagram, NativeArray<float2> sites, in Balancing balancing);
    }
}
