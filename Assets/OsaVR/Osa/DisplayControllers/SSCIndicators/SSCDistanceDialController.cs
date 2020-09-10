using UnityEngine;

namespace OsaVR.Osa.DisplayControllers.SSCIndicators
{
    public class SSCDistanceDialController: OsaCircularDialController
    {
        private void Update()
        {
            SetDialRotation(_state.SSC.distance / _state.SSC.maxDistance * Mathf.PI * 0.9f);
        }
    }
}