using UnityEngine;

namespace OsaVR.Utils
{
    public class MathUtils
    {

        public static float NormalizeAzimuth(float deg)
        {
            if (deg < 360f)
            {
                return deg;
            }
            else
            {
                var circles = Mathf.Ceil(deg / 360f);
                return deg - circles * 360f;
            }
        }
    }
}