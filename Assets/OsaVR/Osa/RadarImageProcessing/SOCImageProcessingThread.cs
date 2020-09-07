using System;
using System.Runtime.InteropServices;
using OsaVR.RadarSimulation;
using UnityEngine;

namespace OsaVR.Osa
{
    public class SOCImageProcessingThread: RadarProcessingThread
    {
        public static readonly uint ScopeIndex = 0;
        
        public float InputFarPlane = 50f;
        public float Azimuth = 0f;
        public float Fov = 0f;
        public float OutputNearPlane = 0f, OutputFarPlane = 90f;
        
        public SOCImageProcessingThread(RenderTexture inputRT) : base(inputRT)
        {
        }

        protected override unsafe void PerformProcessing()
        {
            fixed (Color32* inputPtr = _inputBuffer.array)
            {
                var outputBuffer = _outputBuffers[0];
                
                fixed (Color32* outputPtr = outputBuffer.array)
                {
                    
                    var input = new NativeInputStruct()
                    {
                        buf = (IntPtr)inputPtr,
                        width = _inputBuffer.width,
                        height = _inputBuffer.height,
                        channels = _inputBuffer.channels,
                        azimuth = Azimuth,
                        elevation = 0,
                        far_plane = InputFarPlane,
                        fov = Fov,
                    };

                    var output = new NativeOutputStruct()
                    {
                        buf = (IntPtr)outputPtr,
                        width = outputBuffer.width,
                        height = outputBuffer.height,
                        channels = outputBuffer.channels,
                        near_plane = OutputNearPlane,
                        far_plane = OutputFarPlane,
                    };

                    RadarProcNative.update_soc_image(input, output);
                }
            }
        }
    }
}