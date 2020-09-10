#ifndef RADARIMG_PROC_LIBRARY_H
#define RADARIMG_PROC_LIBRARY_H

#define EXPORT_API __declspec(dllexport)

#define _USE_MATH_DEFINES
#include <math.h>

#define degToRad(angleInDegrees) ((angleInDegrees) * M_PI / 180.0)
#define radToDeg(angleInRadians) ((angleInRadians) * 180.0 / M_PI)
#define lerp(a, b, f) a + f * (b - a)
#define inverseLerp(a, b, f) (f - a) / (b - a)
#define clamp(a, b, v) v < a ? a : (v > b) ? b : v

struct input_struct {
    unsigned char *buf;
    int width;
    int height;
    int channels;
    float fov;
    float far_plane;
    float azimuth;
    float elevation;
};

struct output_struct {
    unsigned char *buf;
    int width;
    int height;
    int channels;

    float near_plane;
    float far_plane;
};

typedef struct input_struct input_t;
typedef struct output_struct output_t;

void fill_px(output_t output, int x, int y);
void blank_px(output_t output, int x, int y);
void fill_pixel(unsigned char *ptr, int w, int h, int ch, int x, int y);

#endif //RADARIMG_PROC_LIBRARY_H
