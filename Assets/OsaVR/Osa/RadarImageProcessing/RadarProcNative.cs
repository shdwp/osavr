using System;
using System.Runtime.InteropServices;

namespace OsaVR.RadarSimulation
{
    
    [StructLayout(LayoutKind.Sequential)]
    public struct NativeInputStruct {
        public IntPtr buf;
        public int width;
        public int height;
        public int channels;
        public float fov;
        public float far_plane;
        public float azimuth;
        public float elevation;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct NativeOutputStruct {
        public IntPtr buf;
        public int width;
        public int height;
        public int channels;

        public float near_plane;
        public float far_plane;
    };
    
    [StructLayout(LayoutKind.Sequential)]
    public struct NativeSSCTargetingGateStruct {
        public float near_plane;
        public float far_plane;
    };
    
    [StructLayout(LayoutKind.Sequential)]
    public struct NativeSSCDeviationInfoStruct {
        public float horizontal;
        public float vertical;
    };
    

    public class RadarProcNative
    {
        [DllImport("radarimg_proc")]
        public static extern int process_ssc_image(
            NativeInputStruct input, 
            NativeOutputStruct output, 
            NativeSSCTargetingGateStruct targeting_gate, 
            ref NativeSSCDeviationInfoStruct deviation_info
        );
    }
}