#include "test_application.h"
#include <sys\timeb.h>
#include "string.h"

long long system_current_time_millis() {
#if defined(_WIN32) || defined(_WIN64)
    struct _timeb timebuffer;
    _ftime(&timebuffer);
    return (long long)(((timebuffer.time * 1000) + timebuffer.millitm));
#else
    struct timeb timebuffer;
    ftime(&timebuffer);
    return (uint64_t)(((timebuffer.time * 1000) + timebuffer.millitm));
#endif
}

void test_soc_processing() {
    int w, h, ch;
    unsigned char *data = stbi_load("input__2Beam.png", &w, &h, &ch, 0);

    int output_w = 256, output_h = 256, output_ch = 4;
    unsigned char *target_data = malloc(output_w * output_h * output_ch);
    memset(target_data, 0, output_w * output_h * output_ch);

    input_t input = {
            .buf = data,
            .width = w,
            .height = h,
            .channels = ch,
            .far_plane = 50.f,
            .fov = 4.f,
            .azimuth = 0.f,
            .elevation = 0
    };

    output_t output = {
            .buf = target_data,
            .width = output_w,
            .height = output_h,
            .channels = output_ch,
            .near_plane = 0.f,
            .far_plane = 50.f,
    };

    printf("image %dx%d\n", w, h);

    long long start = system_current_time_millis();

    update_soc_image(input, output);

    printf("did it in %llu\n", (system_current_time_millis() - start));
    stbi_write_png("output.png", output_w, output_h, output_ch, target_data, output_w * output_ch);
}

void test_ssc_processing() {
    int w, h, ch;
    unsigned char *data = stbi_load("ssc_input.png", &w, &h, &ch, 0);

    int output_w = 256, output_h = 256, output_ch = 4;
    unsigned char *target_data = malloc(output_w * output_h * output_ch);
    memset(target_data, 0, output_w * output_h * output_ch);

    printf("image %dx%d\n", w, h);

    input_t input = {
            .buf = data,
            .width = w,
            .height = h,
            .channels = ch,
            .far_plane = 50,
            .azimuth = 0,
            .elevation = 0
    };

    output_t output = {
            .buf = target_data,
            .width = output_w,
            .height = output_h,
            .channels = output_ch,
            .near_plane = 0,
            .far_plane = 28,
    };

    ssc_targeting_gate_t targeting_info = {
            .near_plane = 25,
            .far_plane = 28,
    };

    ssc_deviation_info_t deviation_info;

    long long start = system_current_time_millis();

    process_ssc_image(input, output, targeting_info, &deviation_info);

    printf("did it in %llu\n", (system_current_time_millis() - start));
    stbi_write_png("ssc_output.png", output_w, output_h, output_ch, target_data, output_w * output_ch);
}

void main(void) {
    test_soc_processing();
    test_ssc_processing();
}