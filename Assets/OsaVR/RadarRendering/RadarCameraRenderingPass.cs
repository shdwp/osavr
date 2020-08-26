using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace OsaVR.RadarRendering
{
    class RadarCameraRenderingPass : CustomPass
    {
        public RenderTexture targetTex;
    
        private Shader _shader;
        private Material _material;
        private MaterialPropertyBlock _materialPropertyBlock;
        private RTHandle _rt;
        private ShaderTagId[] _shaderTags;
    
        protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
        {
            _shader = Shader.Find("Shader Graphs/RadarViewShader");
            _material = CoreUtils.CreateEngineMaterial(_shader);
            _materialPropertyBlock = new MaterialPropertyBlock();
            _rt = RTHandles.Alloc(
                Vector2.one, TextureXR.slices, dimension: TextureXR.dimension,
                colorFormat: GraphicsFormat.B10G11R11_UFloatPack32,
                useDynamicScale: true, name: "TestPassBuf"
            );
            _shaderTags = new ShaderTagId[]
            {
                new ShaderTagId("Forward"),
            };
        }
    
        void DrawMeshses(ScriptableRenderContext renderContext, CommandBuffer cmd, HDCamera hdCamera, CullingResults cullingResult)
        {
            if (!hdCamera.camera.name.StartsWith("@UtilityCamera:SOC_2"))
            {
                return;
            }
        
            var result = new RendererListDesc(_shaderTags, cullingResult, hdCamera.camera)
            {
                rendererConfiguration = PerObjectData.None,
                renderQueueRange = GetRenderQueueRange(RenderQueueType.AllOpaque),
                sortingCriteria = SortingCriteria.BackToFront,
                excludeObjectMotionVectors = false,
                layerMask = 1,
                overrideMaterial = _material,
                overrideMaterialPassIndex = 5,
            };

            CoreUtils.SetRenderTarget(cmd, targetTex.colorBuffer, ClearFlag.All);
            HDUtils.DrawRendererList(renderContext, cmd, RendererList.Create(result));
        }

        protected override void Execute(ScriptableRenderContext renderContext, CommandBuffer cmd, HDCamera hdCamera, CullingResults cullingResult)
        {
            DrawMeshses(renderContext, cmd, hdCamera, cullingResult);
        }

        protected override void Cleanup()
        {
            CoreUtils.Destroy(_material);
            _rt.Release();
        }
    }
}