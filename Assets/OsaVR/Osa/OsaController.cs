using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using OsaVR.Osa.Model;
using OsaVR.Osa.ViewControllers;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace OsaVR.Osa
{
    public class OsaController: MonoBehaviour
    {
        public RenderTexture RT_SOCBeam1, RT_SOCBeam2, RT_SOCBeam3;
        
        private OsaState _state;
        private SignalScopeController _scope;
        
        private void Start()
        {
            _scope = FindObjectOfType<SignalScopeController>();
            _state = FindObjectOfType<OsaState>();
            
            _state.WorldPosition = transform.position;
            _state.ForwardVector = transform.forward;
        }

        /*
        private void LateUpdate()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.S))
            {
                var sw = Stopwatch.StartNew();
                var dataTex = _scope._dataTex;
                var dataWidth = _scope._dataTex.width;
                var dataHeight = _scope._dataTex.height;
                
                Color[] zero = new Color[dataWidth * dataHeight];
                dataTex.SetPixels(zero);
                
                Debug.Log($"Blanked in {sw.ElapsedMilliseconds}");
                
                RenderTexture.active = RT;
                Texture2D img = new Texture2D(RT.width, RT.height);
                img.ReadPixels(new Rect(0, 0, RT.width, RT.height), 0, 0);
                img.Apply();
                File.WriteAllBytes("b.png", img.EncodeToPNG());
                Debug.Log($"Read in {sw.ElapsedMilliseconds}");

                float[] data = new float[dataWidth];
                float max_energy = 0f;
                for (int x = 0; x < RT.width; x++)
                {
                    for (int y = 0; y < RT.height; y++)
                    {
                        var p = img.GetPixel(x, y);
                        var distance = p.r;
                        var energy = p.b;

                        if (energy > 0f || distance > 0f)
                        {
                            var idx = (int) (distance * dataWidth);
                            var total_energy = data[idx] + energy;
                            data[idx] = total_energy;
                            if (total_energy > max_energy)
                            {
                                max_energy = total_energy;
                            }
                        }
                    }
                }

                Debug.Log($"Processed in {sw.ElapsedMilliseconds}");
                for (int x = 0; x < dataWidth; x++)
                {
                    var signal_strength = (int) (data[x] / max_energy * dataHeight);
                    signal_strength = signal_strength > 255 ? 255 : signal_strength;

                    dataTex.SetPixel(x, 0, Color.cyan);
                    for (int y = 0; y < signal_strength; y++)
                    {
                        dataTex.SetPixel(x, y, Color.cyan);
                    }
                }

                _scope._dataTex.Apply();
                File.WriteAllBytes("a.png", _scope._dataTex.EncodeToPNG());

                Debug.Log($"Drawn in {sw.ElapsedMilliseconds}");
            }
        }
        */
    }
}