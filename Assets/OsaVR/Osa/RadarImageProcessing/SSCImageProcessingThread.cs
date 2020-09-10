using System;
using OsaVR.RadarSimulation;
using UnityEngine;

namespace OsaVR.Osa
{
    public class SSCImageProcessingThread: RadarProcessingThread 
    {
        public static readonly uint SignalScopeIndex = 0;
        public static readonly uint ElevationScopeIndex = 1;
        
        public float InputFarPlane = 28f;
        public float Azimuth = 0f;
        public float Elevation = 0f;
        public float Fov = 0;
        public bool Emitting;
        public float OutputNearPlane = 0f, OutputFarPlane = 28f;
        public float TargetGateNearPlane = 10f, TargetGateFarPlane = 13f;

        public NativeSSCDeviationInfoStruct DeviationInfo;
        
        public SSCImageProcessingThread(RenderTexture inputRT) : base(inputRT)
        {
        }

        protected override unsafe void PerformProcessing()
        {

            fixed (Color32* inputPtr = _inputBuffer.array)
            {
                var scopeBuffer = _outputBuffers[SignalScopeIndex];
                var elevScopeBuffer = _outputBuffers[ElevationScopeIndex];
                
                fixed (Color32* scopeOutputPtr = scopeBuffer.array)
                {
                    fixed (Color32* elevScopeOutputPtr = elevScopeBuffer.array)
                    {
                        var input = new NativeInputStruct()
                        {
                            buf = (IntPtr) inputPtr,
                            width = _inputBuffer.width,
                            height = _inputBuffer.height,
                            channels = _inputBuffer.channels,
                            azimuth = Azimuth,
                            elevation = Elevation,
                            far_plane = InputFarPlane,
                            fov = Fov,
                        };

                        var scopeOutput = new NativeOutputStruct()
                        {
                            buf = (IntPtr) scopeOutputPtr,
                            width = scopeBuffer.width,
                            height = scopeBuffer.height,
                            channels = scopeBuffer.channels,
                            near_plane = OutputNearPlane,
                            far_plane = OutputFarPlane,
                        };

                        var elevScopeOutput = new NativeOutputStruct()
                        {
                            buf = (IntPtr) elevScopeOutputPtr,
                            width = elevScopeBuffer.width,
                            height = elevScopeBuffer.height,
                            channels = elevScopeBuffer.channels
                        };

                        var state = new NativeSSCStateStruct()
                        {
                            near_plane = TargetGateNearPlane,
                            far_plane = TargetGateFarPlane,
                            emitting = Emitting,
                            guiding_missile1 = false,
                            guiding_missile2 = false,
                        };

                        DeviationInfo = new NativeSSCDeviationInfoStruct();
                        RadarProcNative.process_ssc_image(input, scopeOutput, elevScopeOutput, state, ref DeviationInfo);
                    }
                }
            }
        }
    }
}