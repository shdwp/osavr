#include "ssc_radar.h"
#include "stdlib.h"
#include "stdio.h"
#include "string.h"

void process_ssc_image(
        input_t input,
        output_t output,
        ssc_targeting_gate_t targeting_info,
        ssc_deviation_info_t *deviation_info
) {
    float *full_range_returns = malloc(sizeof(float) * output.width);
    memset(full_range_returns, 0, output.width * sizeof(float));

    float *target_range_returns = malloc(sizeof(float) * output.width);
    memset(target_range_returns, 0, output.width * sizeof(float));

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

                float output_distance_factor = distance / output.far_plane;
                if (output_distance_factor <= 1.f) {
                    int full_range_idx = output_distance_factor * (output.width - 1);
                    full_range_returns[full_range_idx] += return_level;

                    if (distance >= targeting_info.near_plane && distance <= targeting_info.far_plane) {
                        int target_range_idx = ((distance - targeting_info.near_plane) /
                                                (targeting_info.far_plane - targeting_info.near_plane)) *
                                               (output.width - 1);
                        target_range_returns[target_range_idx] += return_level;

                        max_return = max(max_return, target_range_returns[target_range_idx]);
                    }
                }
            }
        }
    }

    int targeting_start_idx = (targeting_info.near_plane / output.far_plane) * output.width;
    int targeting_end_idx = (targeting_info.far_plane / output.far_plane) * output.width;

    int full_range_y = (float)(output.height) * 0.7f;
    int full_range_height = (float)(output.height) * 0.3f;

    int target_range_y = (float)(output.height) * 0.2f;
    int target_range_height = (float)(output.height) * 0.3f;
    float target_return_per_px = (float)(target_range_height) / max_return;

    float current_value = 0.f;
    float draw_value = 0.f;
    int changing = 0;
    int previous_y = 0;

    for (int x = 0; x < output.width; x++) {
        for (int y = 0; y < output.height; y++) {
            blank_px(output, x, y);
        }

        int y_start = full_range_y;
        if (x >= targeting_start_idx && x <= targeting_end_idx) {
            y_start += 15;
        }

        if (x == targeting_start_idx || x == targeting_end_idx) {
            for (int y = full_range_y; y < y_start; y++) {
                fill_px(output, x, y);
            }
        }

        fill_px(output, x, y_start);

        if (full_range_returns[x] > 0.f) {
            for (int y = y_start; y < full_range_y + full_range_height; y++) {
                fill_px(output, x, y);
            }
        }

        if (target_range_returns[x] > 0.f) {
            current_value = target_range_returns[x];
        } else if (changing == 0) {
            current_value = 0.f;
        }

        float diff = current_value - draw_value;
        float correction = max(fabs(diff / 3.f), target_return_per_px * 0.3);
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
            fill_px(output, x, target_range_y + y);
        }

        previous_y = current_y;
        fill_px(output, x, target_range_y + current_y);
    }

    free(full_range_returns);
    free(target_range_returns);
}
