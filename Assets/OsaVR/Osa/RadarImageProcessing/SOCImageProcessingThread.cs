using System;
using System.Runtime.InteropServices;
using OsaVR.RadarSimulation;
using UnityEngine;

namespace OsaVR.Osa
{
    public class SOCImageProcessingThread: RadarProcessingThread
    {
        public float InputFarPlane = 80f;
        public float Azimuth = 0f;
        public float Fov = 0f;
        public float OutputNearPlane = 0f, OutputFarPlane = 90f;
        
        public SOCImageProcessingThread(RenderTexture rt) : base(rt)
        {
        }

        protected override unsafe void PerformProcessing()
        {
            fixed (Color32* inputPtr = _inputBuffer)
            {
                fixed (Color32* outputPtr = _outputBuffer)
                {
                    
                    var input = new NativeInputStruct()
                    {
                        buf = (IntPtr)inputPtr,
                        width = _inputWidth,
                        height = _inputHeight,
                        channels = 4,
                        azimuth = Azimuth,
                        elevation = 0,
                        far_plane = InputFarPlane,
                        fov = Fov,
                    };

                    var output = new NativeOutputStruct()
                    {
                        buf = (IntPtr)outputPtr,
                        width = _outputWidth,
                        height = _outputHeight,
                        channels = 4,
                        near_plane = OutputNearPlane,
                        far_plane = OutputFarPlane,
                    };

                    RadarProcNative.update_soc_image(input, output);
                }
            }
        }
    }
}