using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace OsaVR.RadarSimulation
{
    public struct RadarProcessingJob: IJob
    {
        [NativeDisableUnsafePtrRestriction]
        private IntPtr _input_buffer;
        private int _input_width, _input_height;
        
        [NativeDisableUnsafePtrRestriction]
        private IntPtr _output_buffer;
        private int _output_width, _output_height;

        private float _azimuth, _input_fov, _input_far_plane, _output_near_plane, _output_far_plane;

        public RadarProcessingJob(IntPtr inputBuffer, int inputWidth, int inputHeight, IntPtr outputBuffer, int outputWidth, int outputHeight, float azimuth, float inputFOV, float inputFarPlane, float outputNearPlane, float outputFarPlane)
        {
            _input_buffer = inputBuffer;
            _input_width = inputWidth;
            _input_height = inputHeight;
            _output_buffer = outputBuffer;
            _output_width = outputWidth;
            _output_height = outputHeight;
            _azimuth = azimuth;
            _input_fov = inputFOV;
            _input_far_plane = inputFarPlane;
            _output_near_plane = outputNearPlane;
            _output_far_plane = outputFarPlane;
        }

        public void Execute()
        {
            update_search_radar_image(
                _input_buffer,
                _input_width,
                _input_height,
                4,
                _input_fov,
                _input_far_plane,
                _azimuth,
                        
                _output_buffer,
                _output_width,
                _output_height,
                4,
                _output_near_plane,
                _output_far_plane
            );
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