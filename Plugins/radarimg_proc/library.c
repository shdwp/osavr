#include "library.h"

#include <stdio.h>

#define _USE_MATH_DEFINES
#include <math.h>

#define degToRad(angleInDegrees) ((angleInDegrees) * M_PI / 180.0)
#define radToDeg(angleInRadians) ((angleInRadians) * 180.0 / M_PI)

void fill_pixel(unsigned char *ptr, int w, int h, int ch, int x, int y) {
    size_t output_offset = (w * h * ch) - (y * w + x) * ch;

    ptr[output_offset++] = 255;
    ptr[output_offset++] = 255;
    ptr[output_offset++] = 255;
    ptr[output_offset++] = 255;
}

int fade_radar_image(
        unsigned char *output,
        int output_width,
        int output_height,
        int output_ch,
        int output_fade_speed
) {
    for (int y = 0; y < output_height; y++) {
        for (int x = 0; x < output_width; x++) {
            size_t offset = (x + y * output_width) * output_ch;

            for (int i = offset; i < offset + 3; i++) {
                output[i] -= output[i] > output_fade_speed ? output_fade_speed : output[i];
            }
        }
    }

    return 0;
}

int update_search_radar_image(
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
) {
    for (int y = 0; y < input_height; y++) {
        for (int x = 0; x < input_width; x++) {
            size_t offset = input_channels * (x + y * input_width);
            unsigned char r = input[offset++];
            unsigned char g = input[offset++];
            unsigned char b = input[offset++];
            unsigned char a = input[offset++];

            if (b > 0) {
                float return_level = (float)b / 255.f;
                float distance = ((float)(r) / 255.f) * input_far_plane;

                if (return_level > 0.f && distance >= output_near_plane && distance <= output_far_plane) {
                    int center_x = output_width / 2, center_y = output_height / 2;
                    int vertical_offset = (int)((distance - output_near_plane) / output_far_plane * (float)output_height / 2.f);

                    float arc_start = degToRad(input_azimuth - input_fov / 2.f);
                    float arc_middle = degToRad(input_azimuth);
                    float arc_end = degToRad(input_azimuth + input_fov / 2.f);

                    int x_a = vertical_offset * sin(arc_start);
                    int x_b = vertical_offset * sin(arc_end);

                    int x_from = x_a < x_b ? x_a : x_b;
                    int x_to = x_a > x_b ? x_a : x_b;

                    for (int mx = x_from; mx < x_to; mx++) {
                        float rad = asin(mx == 0 ? 0 : (float)mx / (float)vertical_offset);
                        int my = -vertical_offset * cos(rad);

                        int marker_x = center_x + mx;
                        int marker_y = center_y + my;

                        fill_pixel(output, output_width, output_height, output_ch, marker_x, marker_y);
                    }

                    int y_a = vertical_offset * cos(arc_start);
                    int y_b = vertical_offset * cos(arc_end);

                    int y_from = y_a < y_b ? y_a : y_b;
                    int y_to = y_a > y_b ? y_a : y_b;

                    for (int my = y_from; my < y_to; my++) {
                        float rad = acos(my == 0 ? 0 : (float)my / (float)vertical_offset);
                        int mx = vertical_offset * sin(rad);

                        int marker_x = center_x + mx;
                        int marker_y = center_y - my;

                        fill_pixel(output, output_width, output_height, output_ch, marker_x, marker_y);
                    }

                    int x = center_x + vertical_offset * sin(arc_middle);
                    int y = center_y - vertical_offset * cos(arc_middle);

                    fill_pixel(output, output_width, output_height, output_ch, x, y);
                }
            }
        }
    }

    return 0;
}

int render_tracking_radar_image(
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
) {
    return 0;
}

