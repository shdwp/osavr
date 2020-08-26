cmake_minimum_required(VERSION 3.16)
project(native_plugin C)

set(CMAKE_C_STANDARD 99)

add_library(native_plugin SHARED library.c library.h)