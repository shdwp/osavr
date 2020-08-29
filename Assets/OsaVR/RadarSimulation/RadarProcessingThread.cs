using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace OsaVR.RadarSimulation
{
    public class RadarProcessingThread
    {
        public float InputFarPlane = 80f;
        public float Azimuth = 0f;
        public float Fov = 0f;
        public float OutputNearPlane = 0f, OutputFarPlane = 90f;
        
        public Color32[] outputBuffer;
        public int outputWidth, outputHeight;
        
        private RenderTexture _rt;
        private Texture2D _inputTex;

        private Color32[] _inputBuffer;

        private int _inputWidth, _inputHeight,  _channels = 4;
        
        private AutoResetEvent _synch = new AutoResetEvent(false);
        private AutoResetEvent _handle = new AutoResetEvent(false);
        
        private bool _run = true;
        private Thread _thread;

        public RadarProcessingThread(RenderTexture rt)
        {
            _rt = rt;
            _inputTex = new Texture2D(_rt.width, _rt.height);
            _inputWidth = _inputTex.width;
            _inputHeight = _inputTex.height;
        }

        public void Start()
        {
            _thread = new Thread(Main);
            _thread.Name = "RadarProcessingThread_" + _rt.name;
            _thread.Start();
            
        }

        public void Stop()
        {
            _run = false;
            _thread.Interrupt();
        }

        public WaitHandle ProcessImage()
        {
            RenderTexture.active = _rt;
            _inputTex.ReadPixels(new Rect(0, 0, _rt.width, _rt.height), 0, 0);
            _inputBuffer = _inputTex.GetPixels32();
            
            _synch.Set();

            return _handle;
        }

        private unsafe void Main()
        {
            while (_run)
            {
                try
                {
                    _synch.WaitOne();
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }

                if (outputBuffer == null)
                {
                    continue;
                }

                fixed (Color32* inputPtr = _inputBuffer)
                {
                    fixed (Color32* outputPtr = outputBuffer)
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
                            outputWidth,
                            outputHeight,
                            _channels,
                            OutputNearPlane,
                            OutputFarPlane
                        );
                    }
                }
                
                _handle.Set();
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