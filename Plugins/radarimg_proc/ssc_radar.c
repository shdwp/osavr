#include "ssc_radar.h"
#include "stdlib.h"
#include "stdio.h"
#include "string.h"

#define STATIC_RETURNS_ARRAY 1

void process_ssc_image(
        input_t input,
        output_t scope_output,
        output_t elev_output,
        ssc_targeting_gate_t targeting_info,
        ssc_deviation_info_t *deviation_info
) {
#ifdef STATIC_RETURNS_ARRAY
    float full_range_returns[256];
    float target_range_returns[256];
    float elevation_returns[256];

    memset(full_range_returns, 0, sizeof(float) * 256);
    memset(target_range_returns, 0, sizeof(float) * 256);
    memset(elevation_returns, 0, sizeof(float) * 256);
#else
    float *full_range_returns = malloc(sizeof(float) * scope_output.width);
    memset(full_range_returns, 0, scope_output.width * sizeof(float));

    float *target_range_returns = malloc(sizeof(float) * scope_output.width);
    memset(target_range_returns, 0, scope_output.width * sizeof(float));

    float *elevation_returns = malloc(sizeof(float) * elev_output.width);
    memset(elevation_returns, 0, elev_output.width * sizeof(float));
#endif

    float max_return = 0.f;

    for (int y = 0; y < input.height; y++) {
        for (int x = 0; x < input.width; x++) {
            size_t offset = input.channels * (x + y * input.width);
            unsigned char r = input.buf[offset++];
            unsigned char g = input.buf[offset++];
            unsigned char b = input.buf[offset++];
            unsigned char a = input.buf[offset++];

            if (b > 0) {
                float return_level = (float) b / 255.f;
                float distance = ((float) (r) / 255.f) * input.far_plane;
                float deviation_x = inverseLerp(0, input.width, (float)x) - 0.5f;
                float deviation_y = inverseLerp(0, input.height, (float)y) - 0.5f;

                float output_distance_factor = distance / scope_output.far_plane;
                if (output_distance_factor <= 1.f) {
                    int full_range_idx = output_distance_factor * (scope_output.width - 1);
                    full_range_returns[full_range_idx] += return_level;

                    if (distance >= targeting_info.near_plane && distance <= targeting_info.far_plane) {
                        int target_range_idx = ((distance - targeting_info.near_plane) /
                                                (targeting_info.far_plane - targeting_info.near_plane)) *
                                               (scope_output.width - 1);
                        target_range_returns[target_range_idx] += return_level;

                        elevation_returns[target_range_idx] = deviation_y;

                        max_return = max(max_return, target_range_returns[target_range_idx]);
                    }
                }
            }
        }
    }

    int targeting_start_idx = (targeting_info.near_plane / scope_output.far_plane) * scope_output.width;
    int targeting_end_idx = (targeting_info.far_plane / scope_output.far_plane) * scope_output.width;

    int full_range_y = (float)(scope_output.height) * 0.7f;
    int full_range_height = (float)(scope_output.height) * 0.3f;

    int target_range_y = (float)(scope_output.height) * 0.2f;
    int target_range_height = (float)(scope_output.height) * 0.3f;
    float target_return_per_px = (float)(target_range_height) / max_return;

    float current_value = 0.f;
    float draw_value = 0.f;
    int changing = 0;
    int previous_y = 0;

    for (int x = 0; x < scope_output.width; x++) {
        // full range

        for (int y = 0; y < scope_output.height; y++) {
            blank_px(scope_output, x, y);
        }

        int y_start = full_range_y;
        if (x >= targeting_start_idx && x <= targeting_end_idx) {
            y_start += 15;
        }

        if (x == targeting_start_idx || x == targeting_end_idx) {
            for (int y = full_range_y; y < y_start; y++) {
                fill_px(scope_output, x, y);
            }
        }

        fill_px(scope_output, x, y_start);

        if (full_range_returns[x] > 0.f) {
            for (int y = y_start; y < full_range_y + full_range_height; y++) {
                fill_px(scope_output, x, y);
            }
        }

        // target gate

        if (target_range_returns[x] > 0.f) {
            current_value = target_range_returns[x];
        } else if (changing == 0) {
            current_value = 0.f;
        }

        float diff = current_value - draw_value;
        float correction = max(fabs(diff / 3.f), 0.1f);
        if (fabs(diff) > correction) {
            draw_value += current_value > draw_value ? +correction : -correction;
            changing = 1;
        } else {
            changing = 0;
        }

        int current_y = draw_value * ((float)target_range_height / max_return);
        if (current_y > target_range_height) {
            current_y = target_range_height;
        }

        for (int y = current_y; abs(previous_y - y) > 0; y += previous_y > y ? +1 : -1) {
            fill_px(scope_output, x, target_range_y + y);
        }

        previous_y = current_y;
        fill_px(scope_output, x, target_range_y + current_y);
    }

    for (int x = 0; x < elev_output.width; x++) {
        for (int y = 0; y < elev_output.height; y++) {
            blank_px(elev_output, x, y);
        }

        if (elevation_returns[x] != 0.f) {
            int y = elev_output.height / 2.f + elevation_returns[x] * elev_output.height;
            int size = 6;

            for (int mx = max(0, x - size); mx < min(x + size, elev_output.width - 1); mx++) {
                for (int my = max(0, y - size); my < min(y + size, elev_output.height - 1); my++) {
                    fill_px(elev_output, mx, my);
                }
            }
        }
    }

#ifndef STATIC_RETURNS_ARRAY
    free(full_range_returns);
    free(target_range_returns);
#endif
}
