using UnityEngine;

namespace OsaVR.Osa
{
    public class OsaLightIndicatorController: OsaIndicatorController
    {
        protected Color color = Color.red;
        
        protected new void Awake()
        {
            dataWidth = 1;
            dataHeight = 1;

            _baseTex = new Texture2D(1, 1);
            _baseTex.SetPixel(0, 0, Color.black);
            _baseTex.Apply();
            
            _illuminatedTex = new Texture2D(1, 1);
            _illuminatedTex.SetPixel(0, 0, color);
            _illuminatedTex.Apply();

            base.Awake();
        }

    }
}