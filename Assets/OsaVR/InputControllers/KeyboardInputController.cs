using UnityEngine;

namespace OsaVR.InputControllers
{
    public class KeyboardInputController: MonoBehaviour
    {
        public Camera leftCamera, rightCamera;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                leftCamera.enabled = true;
                rightCamera.enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                leftCamera.enabled = false;
                rightCamera.enabled = true;
            }
        }
    }
}