#ifndef SSC_RADAR_H
#define SSC_RADAR_H

#include "library.h"
#include "stdbool.h"

struct ssc_general_state_struct {
    float tgate_near_plane;
    float tgate_far_plane;

    bool emitting;

    bool guiding_missile_1, guiding_missile_2;
    float missile_1_distance;
    float missile_2_distance;
};

typedef struct ssc_general_state_struct ssc_general_state_t;

struct ssc_deviation_info_struct {
    bool defined;
    float x;
    float y;
    float z;
};

typedef struct ssc_deviation_info_struct ssc_deviation_info_t;

void EXPORT_API process_ssc_image(
        input_t input,
        output_t scope_output,
        output_t elev_output,
        ssc_general_state_t targeting_info,
        ssc_deviation_info_t *deviation_info
);

#endif