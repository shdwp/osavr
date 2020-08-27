cmake_minimum_required(VERSION 3.16)
project(radarimg_proc C)

set(CMAKE_C_STANDARD 99)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR})
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR})

add_library(radarimg_proc SHARED library.c library.h)

add_executable(test_application stb/stb.c test_application.c)
target_link_libraries(test_application radarimg_proc)

file(COPY cmake-build-debug/radarimg_proc.dll DESTINATION ../../..)
