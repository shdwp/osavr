#include "library.h"

#include <stdio.h>

void fill_px(output_t output, int x, int y) {
    fill_pixel(output.buf, output.width, output.height, output.channels, x, y);
}

void fill_pixel(unsigned char *ptr, int w, int h, int ch, int x, int y) {
    size_t output_offset = ((h - y) * w + x) * ch;

    ptr[output_offset++] = 255;
    ptr[output_offset++] = 255;
    ptr[output_offset++] = 255;
    ptr[output_offset++] = 255;
}

