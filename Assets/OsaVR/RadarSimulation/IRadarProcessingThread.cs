using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace OsaVR.Osa
{
    public interface IRadarProcessingThread
    {
        void Start();
        void Stop();

        void SetOutput(Color32[] buffer, int width, int height);

        WaitHandle StartProcessing();
    }

    public abstract class RadarProcessingThread : IRadarProcessingThread
    {
        protected Color32[] _inputBuffer;
        protected int _inputWidth, _inputHeight,  _channels = 4;
        
        protected Color32[] _outputBuffer;
        protected int _outputWidth, _outputHeight;
        
        private RenderTexture _rt;
        private Texture2D _inputTex;
        
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

        public void SetOutput(Color32[] buffer, int width, int height)
        {
            _outputBuffer = buffer;
            _outputWidth = width;
            _outputHeight = height;
        }

        public WaitHandle StartProcessing()
        {
            RenderTexture.active = _rt;
            _inputTex.ReadPixels(new Rect(0, 0, _rt.width, _rt.height), 0, 0);
            _inputBuffer = _inputTex.GetPixels32();
            
            _synch.Set();
            return _handle;
        }

        protected abstract void PerformProcessing();

        private void Main()
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

                if (_outputBuffer == null)
                {
                    continue;
                }

                PerformProcessing();
                _handle.Set();
            }
        }
    }
}