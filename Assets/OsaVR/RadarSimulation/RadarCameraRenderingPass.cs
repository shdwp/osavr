using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace OsaVR.RadarSimulation
{
    class RadarCameraRenderingPass : CustomPass
    {
        public Camera TargetCamera;
        public RenderTexture RT;
    
        private Shader _shader;
        private Material _material;
        private MaterialPropertyBlock _materialPropertyBlock;
        private ShaderTagId[] _shaderTags;
    
        protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
        {
            _shader = Shader.Find("Shader Graphs/RadarViewShader");
            _material = CoreUtils.CreateEngineMaterial(_shader);
            _materialPropertyBlock = new MaterialPropertyBlock();
            _shaderTags = new ShaderTagId[]
            {
                new ShaderTagId("Forward"),
            };
        }
    
        void DrawMeshses(ScriptableRenderContext renderContext, CommandBuffer cmd, HDCamera hdCamera, CullingResults cullingResult)
        {
            if (!hdCamera.camera.Equals(TargetCamera))
            {
                return;
            }
        
            var result = new RendererListDesc(_shaderTags, cullingResult, hdCamera.camera)
            {
                rendererConfiguration = PerObjectData.None,
                renderQueueRange = GetRenderQueueRange(RenderQueueType.AllOpaque),
                sortingCriteria = SortingCriteria.BackToFront,
                excludeObjectMotionVectors = false,
                layerMask = 1 << 11,
                overrideMaterial = _material,
                overrideMaterialPassIndex = 5,
            };

            CoreUtils.SetRenderTarget(cmd, RT.colorBuffer, ClearFlag.All);
            HDUtils.DrawRendererList(renderContext, cmd, RendererList.Create(result));
        }

        protected override void Execute(ScriptableRenderContext renderContext, CommandBuffer cmd, HDCamera hdCamera, CullingResults cullingResult)
        {
            DrawMeshses(renderContext, cmd, hdCamera, cullingResult);
        }

        protected override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }
    }
}