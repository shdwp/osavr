using UnityEngine;

namespace OsaVR.InputControllers
{
    public class KeyboardInputController: MonoBehaviour
    {
        public Camera leftCamera, rightCamera, freeCamera;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                leftCamera.enabled = true;
                rightCamera.enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                leftCamera.enabled = false;
                rightCamera.enabled = true;
            } 
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                freeCamera.enabled = !freeCamera.enabled;
                leftCamera.enabled = !freeCamera.enabled;
                rightCamera.enabled = false;
            }
        }
    }
}