using System;
using UnityEngine;

namespace OsaVR.RadarSimulation
{
    public class RadarReflectible: MonoBehaviour
    {
        private MaterialPropertyBlock _block;
        
        private static readonly int _shaderidVelocityVector = Shader.PropertyToID("_VelocityVector");
        private static readonly int _shaderidIFFResponse = Shader.PropertyToID("_IFFResponse");
        
        private void Awake()
        {
            var rend = GetComponent<Renderer>();
            
            _block = new MaterialPropertyBlock();
            rend.GetPropertyBlock(_block);
            rend.SetPropertyBlock(_block);
        }

        private void Update()
        {
            _block.SetVector(_shaderidVelocityVector, new Vector4(0f, 0f, 0f, 0f));
            _block.SetInt(_shaderidIFFResponse, 0);
        }
    }
}