#ifndef SOC_RADAR_H
#define SOC_RADAR_H

#include "library.h"

int EXPORT_API fade_radar_image(
        output_t output,
        int output_fade_speed
);

int EXPORT_API update_soc_image(
        input_t input,
        output_t output
);

#endif
