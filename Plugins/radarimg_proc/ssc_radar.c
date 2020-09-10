#include "ssc_radar.h"
#include "stdlib.h"
#include "stdio.h"
#include "string.h"

// #define STATIC_RETURNS_ARRAY

//
static float PREDEFINED_RETURNS_BEGINNING[] = {
        0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f,
        0.2f, 0.3f, 0.3f, 0.3f, 0.1f,
        -0.2f, -0.4f, -0.6f, -0.6f, -0.5f, -0.4f, -0.4f, -0.4f, -0.2f, -0.1f, -0.05f,
};


void process_ssc_image(
        input_t input,
        output_t scope_output,
        output_t elev_output,
        ssc_general_state_t state,
        ssc_deviation_info_t *deviation_info
) {
#ifdef STATIC_RETURNS_ARRAY
    float full_range_returns[256];
    float gate_range_returns[256];
    int elevation_range_returns[256];

    memset(full_range_returns, 0, sizeof(float) * 256);
    memset(gate_range_returns, 0, sizeof(float) * 256);
    memset(elevation_range_returns, 0, sizeof(int) * 256);
#else
    float *full_range_returns = malloc(sizeof(float) * scope_output.width);
    memset(full_range_returns, 0, scope_output.width * sizeof(float));

    float *gate_range_returns = malloc(sizeof(float) * scope_output.width);
    memset(gate_range_returns, 0, scope_output.width * sizeof(float));

    int *elevation_range_returns = malloc(sizeof(int) * elev_output.width);
    memset(elevation_range_returns, 0, elev_output.width * sizeof(int));
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
                float distance = lerp(input.far_plane, 0.f, (float) (r) / 255.f);
                float deviation_x = inverseLerp(0, input.width, (float)x) - 0.5f;
                float deviation_y = inverseLerp(0, input.height, (float)y) - 0.5f;

                float output_distance_factor = distance / scope_output.far_plane;
                if (output_distance_factor <= 1.f) {
                    int full_range_idx = output_distance_factor * (scope_output.width - 1);
                    full_range_returns[full_range_idx] = 0.6f;

                    if (distance >= state.tgate_near_plane && distance <= state.tgate_far_plane) {
                        float tgate_position = inverseLerp(state.tgate_near_plane, state.tgate_far_plane, distance);
                        float deviation_z = tgate_position - 0.5f;
                        int tgate_returns_idx = tgate_position * (scope_output.width - 1);
                        gate_range_returns[tgate_returns_idx] += return_level;
                        elevation_range_returns[tgate_returns_idx] = y;

                        max_return = max(max_return, gate_range_returns[tgate_returns_idx]);

                        if (deviation_info->defined) {
                            deviation_info->x = (deviation_info->x + deviation_x) / 2.f;
                            deviation_info->y = (deviation_info->y + deviation_y) / 2.f;
                            deviation_info->z = (deviation_info->z + deviation_z) / 2.f;
                        } else {
                            deviation_info->x = deviation_x;
                            deviation_info->y = deviation_y;
                            deviation_info->z = deviation_z;
                            deviation_info->defined = true;
                        }
                    }
                }
            }
        }
    }

    if (state.emitting) {
        for (int i = 0; i < sizeof(PREDEFINED_RETURNS_BEGINNING) / sizeof(float); i++) {
            full_range_returns[i] += PREDEFINED_RETURNS_BEGINNING[i];
        }
    }

    int overview_y_offset = (float)(scope_output.height) * 0.75f;
    int overview_max_height = (float)scope_output.height * 0.25f;

    int tgate_start_x = (state.tgate_near_plane / scope_output.far_plane) * scope_output.width;
    int tgate_end_x = (state.tgate_far_plane / scope_output.far_plane) * scope_output.width;
    int tgate_y_offset = (float)(scope_output.height) * 0.25f;
    int tgate_max_height = (float)scope_output.height * 0.15f;
    int tgate_reticle_start_x = (scope_output.width / 6.f) * 2.5f;
    int tgate_reticle_end_x = (scope_output.width / 6.f) * 3.5f;
    int tgate_reticle_width = 3;

    // Overview output
    int overview_preceding_level = 0;
    for (int x = 0; x < scope_output.width; x++) {
        for (int y = 0; y < scope_output.height; y++) {
            blank_px(scope_output, x, y);
        }

        float signal_level = full_range_returns[x];
        int current_level = signal_level * overview_max_height;

        // random noise
        current_level += rand() % 6 - 3;

        // targeting gate stand
        if (x > tgate_start_x && x < tgate_end_x) {
            current_level += 18;
        }

        current_level = clamp(-overview_max_height, overview_max_height, current_level);

        for (int y = current_level; y != overview_preceding_level; y += (current_level > overview_preceding_level) ? -1 : 1) {
            fill_px(scope_output, x, y + overview_y_offset);
        }

        fill_px(scope_output, x, current_level + overview_y_offset);
        overview_preceding_level = current_level;
    }

    // Gate output
    int x = 0;
    float y = 0;
    float tgate_random_deviation = 1.f - ((rand() % 10) - 5) / 10.f;
    for (int d = 0; d < scope_output.width; d++) {
        float value = gate_range_returns[d];

        if (value > 0.f) {
            value *= tgate_random_deviation;

            float target_y = (value / max_return) * tgate_max_height;
            // printf("Point at %d (draw from %d to %d, y from %f to %f): ", d, x, d, y, target_y);
            float range = (target_y - y);
            float curve_per_iter = 1.f / (float)(d - x);
            float curve_value = 0.f;

            for (; x < d; x++) {
                if (min(abs(tgate_reticle_start_x - x), abs(tgate_reticle_end_x - x)) < tgate_reticle_width) {
                    continue;
                }

                float value = pow(curve_value, 2);
                int new_y = y + value * range;

                if (curve_per_iter > 0.f) {
                    for (int dy = (int) y; dy <= new_y; dy++) {
                        fill_px(scope_output, x, dy + tgate_y_offset);
                    }
                } else if (curve_per_iter < 0.f) {
                    for (int dy = (int) y; dy >= y + new_y; dy--) {
                        fill_px(scope_output, x, dy + tgate_y_offset);
                    }
                }

                curve_value += curve_per_iter;
                y = new_y;
            }
            // printf("\n");

        } else if (d - x >= 8) {
            // printf("Catching up (draw from %d to %d; y %f)\n", x, d - 4, y);
            for (; x < d - 4; x++) {

                for (int dy = y; dy > max(y - 3, -1); dy--) {
                    if (min(abs(tgate_reticle_start_x - x), abs(tgate_reticle_end_x - x)) < tgate_reticle_width) {
                        continue;
                    }

                    fill_px(scope_output, x, dy + tgate_y_offset);
                }

                if (y > 0) {
                    y = max(y - 3, 0);
                }
            }
        }
    }

    // Elevation output
    for (int x = 0; x < elev_output.width; x++) {
        for (int y = 0; y < elev_output.height; y++) {
            blank_px(elev_output, x, y);
        }

        if (elevation_range_returns[x] != 0) {
            int y = inverseLerp(0, elev_output.height, lerp(0, input.height, elevation_range_returns[x]));
            int size_w = 6, size_h = 4;

            for (int mx = max(0, x - size_w); mx < min(x + size_w, elev_output.width - 1); mx++) {
                for (int my = max(0, y - size_h); my < min(y + size_h, elev_output.height - 1); my++) {
                    fill_px(elev_output, mx, my);
                }
            }
        }
    }

#ifndef STATIC_RETURNS_ARRAY
    free(full_range_returns);
    free(gate_range_returns);
#endif
}
