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

void main(void) {
    int w, h, ch;
    unsigned char *data = stbi_load("input.png", &w, &h, &ch, 0);

    int output_w = 256, output_h = 256, output_ch = 4;
    unsigned char *target_data = malloc(output_w * output_h * output_ch);
    memset(target_data, 0, output_w * output_h * output_ch);

    float azimuth = 90.f;
    float fov = 4.f;
    float input_far_plane = 80.f;
    float output_distance = 160.f;

    printf("image %dx%d\n", w, h);

    long long start = system_current_time_millis();

    update_search_radar_image(data, w, h, ch, fov, input_far_plane, azimuth, target_data, output_w, output_h, output_ch, 0.f, output_distance);

    printf("did it in %llu\n", (system_current_time_millis() - start));
    stbi_write_png("output.png", output_w, output_h, output_ch, target_data, output_w * output_ch);
}