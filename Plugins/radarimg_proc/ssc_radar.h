#ifndef SSC_RADAR_H
#define SSC_RADAR_H

#include "library.h"

struct ssc_targeting_gate_struct {
    float near_plane;
    float far_plane;
};

typedef struct ssc_targeting_gate_struct ssc_targeting_gate_t;

struct ssc_deviation_info_struct {
    float horizontal;
    float vertical;
};

typedef struct ssc_deviation_info_struct ssc_deviation_info_t;

void EXPORT_API process_ssc_image(
        input_t input,
        output_t output,
        ssc_targeting_gate_t targeting_info,
        ssc_deviation_info_t *deviation_info
);

#endif