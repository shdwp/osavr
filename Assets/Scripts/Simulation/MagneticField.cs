using System.Linq;
using UnityEngine;

namespace Osa.Model
{
    public class MagneticField: MonoBehaviour
    {
        public void Test()
        {
            var beamCam = Camera.allCameras.Where(a => a.name == "Camera").FirstOrDefault();
            Debug.Assert(beamCam);

            var reflectives = FindObjectsOfType<ReflectiveObject>();
            var mat = beamCam.worldToCameraMatrix;
            foreach (var rf in reflectives)
            {
                var worldPos = rf.transform.position;
                Debug.LogFormat("Position of {0} - {1};{2};{3}", rf.name, worldPos.x, worldPos.y, worldPos.z);
                var camPos = beamCam.projectionMatrix * beamCam.worldToCameraMatrix * new Vector4(worldPos.x, worldPos.y, worldPos.z, 1f);
                Debug.LogFormat("Cam position - {0};{1};{2};{3}", camPos.x / camPos.w, camPos.y / camPos.w, camPos.z / camPos.w, camPos.w);
            }
        }
    }
}