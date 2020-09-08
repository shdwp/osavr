#include "soc_radar.h"
#include "stdlib.h"
#include "stdio.h"
#include "string.h"

struct soc_radar_return {
    float val;
    float velocity;
    float distance;
    unsigned int iff_response;
};

typedef struct soc_radar_return soc_radar_return_t;

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
    soc_radar_return_t *returns = malloc(sizeof(soc_radar_return_t) * input.width);
    memset(returns, 0, sizeof(soc_radar_return_t) * input.width);

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
                float angular_velocity = (float)g / 255.f;
                unsigned int iff_response = (unsigned int)a;

                returns[r].val += (float)b / 255.f;
                returns[r].velocity = angular_velocity;
                returns[r].iff_response = max(iff_response, returns[r].iff_response);
                returns[r].distance = distance;
            }
        }
    }

    for (int w = 0; w < input.width; w++) {
        soc_radar_return_t ret = returns[w];

        if (ret.val > 0.f && ret.distance >= output.near_plane && ret.distance <= output.far_plane) {
            int center_x = output.width / 2, center_y = output.height / 2;
            float radius = ((ret.distance - output.near_plane) / output.far_plane * (float)output.height / 2.f);

            float arc_start = degToRad(input.azimuth - input.fov / 2.f);
            float arc_middle = degToRad(input.azimuth);
            float arc_end = degToRad(input.azimuth + input.fov / 2.f);
            float arc_step = 0.00793f; // precalculated based on tex resolution

            for (float angle = arc_start; angle < arc_end; angle += arc_step) {
                for (float rad = radius; rad < radius + 2.f; rad += 0.5f) {
                    int mx = (int)(rad * sinf(angle));
                    int my = (int)(rad * cosf(angle));

                    fill_px(output, center_x + mx, center_y + my);
                }
            }

            if ((ret.iff_response & 2) != 0) {
                for (float angle = arc_middle; angle < arc_end; angle += arc_step) {
                    for (float rad = radius + 2.f; rad < radius + 4.f; rad += 0.5f) {
                        int mx = (int) floorf(rad * sinf(angle));
                        int my = (int) floorf(rad * cosf(angle));

                        fill_px(output, center_x + mx, center_y + my);
                    }
                }
            }

            if ((ret.iff_response & 1) != 0) {
                for (float angle = arc_start; angle < arc_end; angle += arc_step) {
                    int mx = (int) floorf((radius - 4.f) * sinf(angle));
                    int my = (int) floorf((radius - 4.f) * cosf(angle));

                    fill_px(output, center_x + mx, center_y + my);
                }
            }
        }
    }

    return 0;
}

