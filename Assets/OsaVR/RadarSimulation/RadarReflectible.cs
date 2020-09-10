using System;
using UnityEngine;

namespace OsaVR.RadarSimulation
{
    public class RadarReflectible: MonoBehaviour
    {
        private Renderer _rend;
        private MaterialPropertyBlock _block;
        
        private static readonly int _shaderidVelocityVector = Shader.PropertyToID("_VelocityVector");
        private static readonly int _shaderidIFFResponse = Shader.PropertyToID("_IFFResponse");
        private static readonly int _shaderidWeatherFactor = Shader.PropertyToID("_WeatherFactor");
        
        private void Awake()
        {
            _rend = GetComponentInChildren<Renderer>();
            _block = new MaterialPropertyBlock();
        }

        private void Update()
        {
            _block.SetVector(_shaderidVelocityVector, new Vector3(0f, 0f, 300f));
            _block.SetInt(_shaderidIFFResponse, 1);
            _block.SetFloat(_shaderidWeatherFactor, 0f);
            _rend.SetPropertyBlock(_block);
        }
    }
}