using UnityEngine;

namespace OsaVR.Osa.DisplayControllers.SOCIndicators
{
    public class SOCAzimuthDialController: OsaCircularDialController
    {
        private void Update()
        {
            SetDialRotation(_state.SOC.azimuth * Mathf.Deg2Rad);
        }
    }
}