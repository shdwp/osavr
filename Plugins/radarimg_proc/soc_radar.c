#include "soc_radar.h"
#include "stdio.h"

int fade_radar_image(
        output_t output,
        int output_fade_speed
) {
    for (int y = 0; y < output.height; y++) {
        for (int x = 0; x < output.width; x++) {
            size_t offset = (x + y * output.width) * output.channels;

            for (int i = offset; i < offset + 3; i++) {
                output.buf[i] -= output.buf[i] > output_fade_speed ? output_fade_speed : output.buf[i];
            }
        }
    }

    return 0;
}

int update_soc_image(
        input_t input,
        output_t output
) {
    for (int y = 0; y < input.height; y++) {
        for (int x = 0; x < input.width; x++) {
            size_t offset = (x + y * input.width) * input.channels;
            unsigned char r = input.buf[offset++];
            unsigned char g = input.buf[offset++];
            unsigned char b = input.buf[offset++];
            unsigned char a = input.buf[offset++];

            if (b > 0) {
                float return_level = (float)b / 255.f;
                float distance = ((float)(r) / 255.f) * input.far_plane;

                if (return_level > 0.f && distance >= output.near_plane && distance <= output.far_plane) {
                    int center_x = output.width / 2, center_y = output.height / 2;
                    int radius = (int)((distance - output.near_plane) / output.far_plane * (float)output.height / 2.f);

                    float arc_start = degToRad(input.azimuth - input.fov / 2.f);
                    float arc_end = degToRad(input.azimuth + input.fov / 2.f);

                    int x_a = radius * sin(arc_start);
                    int x_b = radius * sin(arc_end);

                    int x_from = x_a < x_b ? x_a : x_b;
                    int x_to = x_a > x_b ? x_a : x_b;

                    int y_a = radius * cos(arc_start);
                    int y_b = radius * cos(arc_end);

                    int y_from = y_a < y_b ? y_a : y_b;
                    int y_to = y_a > y_b ? y_a : y_b;

                    // @TODO: make it a proper arc
                    for (int mx = x_from; mx <= x_to; mx++) {
                        for (int my = y_from; my <= y_to; my++) {
                            fill_px(output, center_x + mx, center_y + my);
                        }
                    }
                }
            }
        }
    }

    return 0;
}

