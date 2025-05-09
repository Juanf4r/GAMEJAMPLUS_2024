#if HIGH_DEFINITION_RENDER_PIPELINE
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace ProjectDawn.SplitScreen.HighDefinition
{
    public sealed class SplitScreenCustomPass : CustomPass
    {
#if UNITY_2020_3_OR_NEWER
        protected override void Execute(CustomPassContext ctx)
        {
            if (ctx.hdCamera.camera.TryGetComponent(out SplitScreenEffect splitScreen) && splitScreen.isActiveAndEnabled)
            {
                splitScreen.UpdateCommandBuffer(ctx.cmd);
            }
        }
#else
        protected override void Execute(ScriptableRenderContext ctx, CommandBuffer cmd, HDCamera hdCamera, CullingResults cullingResult)
        {
            if (hdCamera.camera.TryGetComponent(out SplitScreenEffect splitScreen) && splitScreen.isActiveAndEnabled)
            {
                splitScreen.UpdateCommandBuffer(cmd);
            }
        }
#endif
    }
}
#endif
