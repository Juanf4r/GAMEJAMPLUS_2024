#define USE_URP_RENDER_GRAPH
#if UNIVERSAL_RENDER_PIPELINE
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if RENDER_PIPELINES_CORE_7_0_13_OR_NEWER
using UnityEngine.Rendering.RenderGraphModule;
#endif

namespace ProjectDawn.SplitScreen.Universal
{
    [DisallowMultipleComponent]
    public class SplitScreenRendererFeature : ScriptableRendererFeature
    {
        public static bool AnyCreated { private set; get; }


        public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

        class SplitScreenRenderPass : ScriptableRenderPass
        {
            private ProfilingSampler m_ProfilingSampler;

            public SplitScreenRenderPass(RenderPassEvent ev)
            {
                m_ProfilingSampler = new ProfilingSampler("Split Screen Render");
                renderPassEvent = ev;
            }

#if RENDER_PIPELINES_CORE_7_0_13_OR_NEWER
            [Obsolete]
#endif
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                var camera = renderingData.cameraData.camera;

                if (!camera.TryGetComponent<SplitScreenEffect>(out SplitScreenEffect splitScreen))
                    return;

                if (!splitScreen.isActiveAndEnabled)
                    return;

                SplitScreenRendererFeature.AnyCreated = true;

                var cmd = splitScreen.GetCommandBuffer();
                context.ExecuteCommandBuffer(cmd);
            }

#if RENDER_PIPELINES_CORE_7_0_13_OR_NEWER
            class PassData
            {
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

                var camera = cameraData.camera;

                if (!camera || !camera.TryGetComponent(out SplitScreenEffect splitScreen))
                    return;

                if (!splitScreen.isActiveAndEnabled)
                    return;

                using (var builder = renderGraph.AddRasterRenderPass<PassData>("Split Screen Pass", out var passData, m_ProfilingSampler))
                {
                    UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
                    UniversalRenderingData renderingData = frameData.Get<UniversalRenderingData>();
                    
                    UniversalLightData lightData = frameData.Get<UniversalLightData>();

                    builder.SetRenderAttachment(resourceData.activeColorTexture, 0, AccessFlags.Write);
                    builder.SetRenderAttachmentDepth(resourceData.activeDepthTexture, AccessFlags.Read);

                    TextureHandle[] importedTexture = new TextureHandle[splitScreen.Screens.Count];
                    for (int i = 0; i < splitScreen.Screens.Count; i++)
                    {
                        importedTexture[i] = renderGraph.ImportTexture(RTHandles.Alloc(splitScreen.Screens[i].RenderTarget));
                        builder.UseTexture(importedTexture[i]);
                    }

                    SplitScreenRendererFeature.AnyCreated = true;

                    builder.AllowGlobalStateModification(true);

                    builder.SetRenderFunc((PassData data, RasterGraphContext rgContext) =>
                    {
                        splitScreen.UpdateCommandBuffer(rgContext.cmd, importedTexture);
                    });
                }
            }
#endif
        }

        SplitScreenRenderPass m_SplitScreenPass;

        public override void Create()
        {
            m_SplitScreenPass = new SplitScreenRenderPass(Event);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(m_SplitScreenPass);
        }
    }
}
#endif

