using UnityEngine;

namespace OsaVR.Osa.DisplayControllers.SSCIndicators
{
    public class SSCElevationDialController: OsaCircularDialController
    {
        private void Update()
        {
            SetDialRotation(_state.SSC.elevation * Mathf.Deg2Rad);
        }
    }
}