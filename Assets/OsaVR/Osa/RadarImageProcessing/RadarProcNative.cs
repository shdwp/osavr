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
    public struct NativeSSCStateStruct {
        public float near_plane;
        public float far_plane;

        public bool emitting;
        
        public bool guiding_missile1, guiding_missile2;
        public float missile1_distance, missile2_distance;
    };
    
    [StructLayout(LayoutKind.Sequential)]
    public struct NativeSSCDeviationInfoStruct
    {
        public bool defined;
        public float x;
        public float y;
        public float z;
    };

    public class RadarProcNative
    {
        [DllImport("radarimg_proc")]
        public static extern int process_ssc_image(
            NativeInputStruct input, 
            NativeOutputStruct scope_output, 
            NativeOutputStruct elevation_scope_output, 
            NativeSSCStateStruct state, 
            ref NativeSSCDeviationInfoStruct deviation_info
        );

        [DllImport("radarimg_proc")]
        public static extern int update_soc_image(NativeInputStruct input, NativeOutputStruct output);

        [DllImport("radarimg_proc")]
        public static extern int fade_radar_image(NativeOutputStruct output, int speed);
    }
}