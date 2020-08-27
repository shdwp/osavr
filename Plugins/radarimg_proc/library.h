#ifndef RADARIMG_PROC_LIBRARY_H
#define RADARIMG_PROC_LIBRARY_H

#define EXPORT_API __declspec(dllexport)

int EXPORT_API fade_radar_image(
        unsigned char *output,
        int output_width,
        int output_height,
        int output_ch,
        int output_fade_speed
);

int EXPORT_API update_search_radar_image(
        unsigned char *input,
        int input_width,
        int input_height,
        int input_channels,
        float input_fov,
        float input_far_plane,
        float input_azimuth,

        unsigned char *output,
        int output_width,
        int output_height,
        int output_ch,
        float output_near_plane,
        float output_far_plane
);

int EXPORT_API render_tracking_radar_image(
        unsigned char *input,
        int input_width,
        int input_height,
        int input_channels,

        float input_fov,
        float input_far_plane,
        float input_azimuth,

        unsigned char *output,
        int output_width,
        int output_height,
        int output_ch
);

#endif //RADARIMG_PROC_LIBRARY_H
