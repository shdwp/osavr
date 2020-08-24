using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

class TestPass : CustomPass
{
    public RenderTexture targetTex;
    
    private Shader _shader;
    private Material _material;
    private MaterialPropertyBlock _materialPropertyBlock;
    private RTHandle _rt;
    private ShaderTagId[] _shaderTags;
    
    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        _shader = Shader.Find("Shader Graphs/radar_return_shader");
        _material = CoreUtils.CreateEngineMaterial(_shader);
        _materialPropertyBlock = new MaterialPropertyBlock();
        _rt = RTHandles.Alloc(
            Vector2.one, TextureXR.slices, dimension: TextureXR.dimension,
            colorFormat: GraphicsFormat.B10G11R11_UFloatPack32,
            useDynamicScale: true, name: "TestPassBuf"
        );
        _shaderTags = new ShaderTagId[]
        {
            new ShaderTagId("ForwardOnly"),
            new ShaderTagId("Forward"),
            new ShaderTagId("SRPDefaultUnlit")
        };
    }
    
    void DrawMeshses(ScriptableRenderContext renderContext, CommandBuffer cmd, HDCamera hdCamera, CullingResults cullingResult)
    {
        if (hdCamera.camera.name != "@UtilityCamera:SOC_3Beam")
        {
            return;
        }
        
        var result = new RendererListDesc(_shaderTags, cullingResult, hdCamera.camera)
        {
            rendererConfiguration = PerObjectData.LightProbe | PerObjectData.LightProbeProxyVolume | PerObjectData.Lightmaps | PerObjectData.OcclusionProbe | PerObjectData.OcclusionProbeProxyVolume | PerObjectData.ShadowMask,
            renderQueueRange = GetRenderQueueRange(RenderQueueType.AllOpaque),
            sortingCriteria = SortingCriteria.BackToFront,
            excludeObjectMotionVectors = false,
            layerMask = 1,
            overrideMaterial = _material,
            overrideMaterialPassIndex = 5,
        };

        CoreUtils.SetRenderTarget(cmd, targetTex.colorBuffer, ClearFlag.All);
        // SetCameraRenderTarget(cmd);
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