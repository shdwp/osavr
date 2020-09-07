using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace OsaVR.Osa
{
    public interface IRadarProcessingThread
    {
        void Start();
        void Stop();

        void SetOutput(uint idx, Color32[] array, int width, int height);

        WaitHandle StartProcessing();
    }

    public abstract class RadarProcessingThread : IRadarProcessingThread
    {
        public struct Buffer
        {
            public Color32[] array;
            public int width, height;
            public int channels;

            public Buffer(Color32[] array, int width, int height) : this()
            {
                this.array = array;
                this.width = width;
                this.height = height;
                this.channels = 4;
            }
        }

        protected Buffer _inputBuffer;
        protected Dictionary<uint, Buffer> _outputBuffers = new Dictionary<uint, Buffer>();
        
        private RenderTexture _inputRT;
        private Texture2D _inputTex;
        
        private AutoResetEvent _synch = new AutoResetEvent(false);
        private AutoResetEvent _handle = new AutoResetEvent(false);
        
        private bool _run = true;
        private Thread _thread;

        public RadarProcessingThread(RenderTexture inputRT)
        {
            _inputRT = inputRT;
            _inputTex = new Texture2D(_inputRT.width, _inputRT.height);
        }

        public void Start()
        {
            _thread = new Thread(Main);
            _thread.Name = "RadarProcessingThread_" + _inputRT.name;
            _thread.Start();
            
        }

        public void Stop()
        {
            _run = false;
            _thread.Interrupt();
        }

        public void SetOutput(uint idx, Color32[] array, int width, int height)
        {
            _outputBuffers[idx] = new Buffer(array, width, height);
        }

        public WaitHandle StartProcessing()
        {
            RenderTexture.active = _inputRT;
            _inputTex.ReadPixels(new Rect(0, 0, _inputRT.width, _inputRT.height), 0, 0);
            _inputBuffer = new Buffer(_inputTex.GetPixels32(), _inputRT.width, _inputRT.height);
            
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

                if (_outputBuffers.Count == 0)
                {
                    continue;
                }

                PerformProcessing();
                _handle.Set();
            }
        }
    }
}