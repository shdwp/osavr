using System;
using OsaVR.RadarSimulation;
using UnityEngine;

namespace OsaVR.Osa
{
    public class SSCImageProcessingThread: RadarProcessingThread 
    {
        public float InputFarPlane = 50f;
        public float Azimuth = 0f;
        public float Elevation = 0f;
        public float Fov = 0;
        public float OutputNearPlane = 0f, OutputFarPlane = 28f;
        public float TargetGateNearPlane = 10f, TargetGateFarPlane = 13f;

        public NativeSSCDeviationInfoStruct DeviationInfo = new NativeSSCDeviationInfoStruct();
        
        public SSCImageProcessingThread(RenderTexture rt) : base(rt)
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
                        elevation = Elevation,
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

                    var targetGate = new NativeSSCTargetingGateStruct()
                    {
                        near_plane = TargetGateNearPlane,
                        far_plane = TargetGateFarPlane,
                    };
                    
                    RadarProcNative.process_ssc_image(input, output, targetGate, ref DeviationInfo);
                }
            }
        }
    }
}