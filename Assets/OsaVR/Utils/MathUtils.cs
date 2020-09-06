using UnityEngine;

namespace OsaVR.Utils
{
    public class MathUtils
    {

        public static float NormalizeAzimuth(float deg)
        {
            if (deg < 360f && deg >= 0f)
            {
                return deg;
            }
            else if (deg < 0f)
            {
                return 360f - NormalizeAzimuth(Mathf.Abs(deg));
            }
            else
            {
                var circles = Mathf.Floor(deg / 360f);
                return deg - circles * 360f;
            }
        }
    }
}