using System;
using System.Runtime.InteropServices;
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
                    update_search_radar_image(
                        (IntPtr)inputPtr,
                        _inputWidth,
                        _inputHeight,
                        _channels,
                        Fov,
                        InputFarPlane,
                        Azimuth,
                        (IntPtr)outputPtr,
                        _outputWidth,
                        _outputHeight,
                        _channels,
                        OutputNearPlane,
                        OutputFarPlane
                    );
                }
            }
        }
        
        
        [DllImport("radarimg_proc")]
        private static extern int update_search_radar_image(
            IntPtr input,
            int input_width,
            int input_height,
            int input_channels,
            float input_fov,
            float input_far_plane,
            float input_azimuth,

            IntPtr output,
            int output_width,
            int output_height,
            int output_ch,
            float output_near_plane,
            float output_far_plane
        );
    }
}