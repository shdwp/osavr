# CMAKE generated file: DO NOT EDIT!
# Generated by "NMake Makefiles" Generator, CMake Version 3.16

# Delete rule output on recipe failure.
.DELETE_ON_ERROR:


#=============================================================================
# Special targets provided by cmake.

# Disable implicit rules so canonical targets will work.
.SUFFIXES:


.SUFFIXES: .hpux_make_needs_suffix_list


# Suppress display of executed commands.
$(VERBOSE).SILENT:


# A target that is always out of date.
cmake_force:

.PHONY : cmake_force

#=============================================================================
# Set environment variables for the build.

!IF "$(OS)" == "Windows_NT"
NULL=
!ELSE
NULL=nul
!ENDIF
SHELL = cmd.exe

# The CMake executable.
CMAKE_COMMAND = "C:\Program Files\JetBrains\CLion 2020.1.2\bin\cmake\win\bin\cmake.exe"

# The command to remove a file.
RM = "C:\Program Files\JetBrains\CLion 2020.1.2\bin\cmake\win\bin\cmake.exe" -E remove -f

# Escaping for special characters.
EQUALS = =

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = D:\UnityProjects\osavr\Plugins\radarimg_proc

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = D:\UnityProjects\osavr\Plugins\radarimg_proc\cmake-build-debug

# Include any dependencies generated for this target.
include CMakeFiles\radarimg_proc.dir\depend.make

# Include the progress variables for this target.
include CMakeFiles\radarimg_proc.dir\progress.make

# Include the compile flags for this target's objects.
include CMakeFiles\radarimg_proc.dir\flags.make

CMakeFiles\radarimg_proc.dir\library.c.obj: CMakeFiles\radarimg_proc.dir\flags.make
CMakeFiles\radarimg_proc.dir\library.c.obj: ..\library.c
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=D:\UnityProjects\osavr\Plugins\radarimg_proc\cmake-build-debug\CMakeFiles --progress-num=$(CMAKE_PROGRESS_1) "Building C object CMakeFiles/radarimg_proc.dir/library.c.obj"
	C:\PROGRA~2\MICROS~1\2019\COMMUN~1\VC\Tools\MSVC\1424~1.283\bin\Hostx64\x64\cl.exe @<<
 /nologo $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) /FoCMakeFiles\radarimg_proc.dir\library.c.obj /FdCMakeFiles\radarimg_proc.dir\ /FS -c D:\UnityProjects\osavr\Plugins\radarimg_proc\library.c
<<

CMakeFiles\radarimg_proc.dir\library.c.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing C source to CMakeFiles/radarimg_proc.dir/library.c.i"
	C:\PROGRA~2\MICROS~1\2019\COMMUN~1\VC\Tools\MSVC\1424~1.283\bin\Hostx64\x64\cl.exe > CMakeFiles\radarimg_proc.dir\library.c.i @<<
 /nologo $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -E D:\UnityProjects\osavr\Plugins\radarimg_proc\library.c
<<

CMakeFiles\radarimg_proc.dir\library.c.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling C source to assembly CMakeFiles/radarimg_proc.dir/library.c.s"
	C:\PROGRA~2\MICROS~1\2019\COMMUN~1\VC\Tools\MSVC\1424~1.283\bin\Hostx64\x64\cl.exe @<<
 /nologo $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) /FoNUL /FAs /FaCMakeFiles\radarimg_proc.dir\library.c.s /c D:\UnityProjects\osavr\Plugins\radarimg_proc\library.c
<<

CMakeFiles\radarimg_proc.dir\soc_radar.c.obj: CMakeFiles\radarimg_proc.dir\flags.make
CMakeFiles\radarimg_proc.dir\soc_radar.c.obj: ..\soc_radar.c
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=D:\UnityProjects\osavr\Plugins\radarimg_proc\cmake-build-debug\CMakeFiles --progress-num=$(CMAKE_PROGRESS_2) "Building C object CMakeFiles/radarimg_proc.dir/soc_radar.c.obj"
	C:\PROGRA~2\MICROS~1\2019\COMMUN~1\VC\Tools\MSVC\1424~1.283\bin\Hostx64\x64\cl.exe @<<
 /nologo $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) /FoCMakeFiles\radarimg_proc.dir\soc_radar.c.obj /FdCMakeFiles\radarimg_proc.dir\ /FS -c D:\UnityProjects\osavr\Plugins\radarimg_proc\soc_radar.c
<<

CMakeFiles\radarimg_proc.dir\soc_radar.c.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing C source to CMakeFiles/radarimg_proc.dir/soc_radar.c.i"
	C:\PROGRA~2\MICROS~1\2019\COMMUN~1\VC\Tools\MSVC\1424~1.283\bin\Hostx64\x64\cl.exe > CMakeFiles\radarimg_proc.dir\soc_radar.c.i @<<
 /nologo $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -E D:\UnityProjects\osavr\Plugins\radarimg_proc\soc_radar.c
<<

CMakeFiles\radarimg_proc.dir\soc_radar.c.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling C source to assembly CMakeFiles/radarimg_proc.dir/soc_radar.c.s"
	C:\PROGRA~2\MICROS~1\2019\COMMUN~1\VC\Tools\MSVC\1424~1.283\bin\Hostx64\x64\cl.exe @<<
 /nologo $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) /FoNUL /FAs /FaCMakeFiles\radarimg_proc.dir\soc_radar.c.s /c D:\UnityProjects\osavr\Plugins\radarimg_proc\soc_radar.c
<<

CMakeFiles\radarimg_proc.dir\ssc_radar.c.obj: CMakeFiles\radarimg_proc.dir\flags.make
CMakeFiles\radarimg_proc.dir\ssc_radar.c.obj: ..\ssc_radar.c
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=D:\UnityProjects\osavr\Plugins\radarimg_proc\cmake-build-debug\CMakeFiles --progress-num=$(CMAKE_PROGRESS_3) "Building C object CMakeFiles/radarimg_proc.dir/ssc_radar.c.obj"
	C:\PROGRA~2\MICROS~1\2019\COMMUN~1\VC\Tools\MSVC\1424~1.283\bin\Hostx64\x64\cl.exe @<<
 /nologo $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) /FoCMakeFiles\radarimg_proc.dir\ssc_radar.c.obj /FdCMakeFiles\radarimg_proc.dir\ /FS -c D:\UnityProjects\osavr\Plugins\radarimg_proc\ssc_radar.c
<<

CMakeFiles\radarimg_proc.dir\ssc_radar.c.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing C source to CMakeFiles/radarimg_proc.dir/ssc_radar.c.i"
	C:\PROGRA~2\MICROS~1\2019\COMMUN~1\VC\Tools\MSVC\1424~1.283\bin\Hostx64\x64\cl.exe > CMakeFiles\radarimg_proc.dir\ssc_radar.c.i @<<
 /nologo $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -E D:\UnityProjects\osavr\Plugins\radarimg_proc\ssc_radar.c
<<

CMakeFiles\radarimg_proc.dir\ssc_radar.c.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling C source to assembly CMakeFiles/radarimg_proc.dir/ssc_radar.c.s"
	C:\PROGRA~2\MICROS~1\2019\COMMUN~1\VC\Tools\MSVC\1424~1.283\bin\Hostx64\x64\cl.exe @<<
 /nologo $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) /FoNUL /FAs /FaCMakeFiles\radarimg_proc.dir\ssc_radar.c.s /c D:\UnityProjects\osavr\Plugins\radarimg_proc\ssc_radar.c
<<

# Object files for target radarimg_proc
radarimg_proc_OBJECTS = \
"CMakeFiles\radarimg_proc.dir\library.c.obj" \
"CMakeFiles\radarimg_proc.dir\soc_radar.c.obj" \
"CMakeFiles\radarimg_proc.dir\ssc_radar.c.obj"

# External object files for target radarimg_proc
radarimg_proc_EXTERNAL_OBJECTS =

radarimg_proc.dll: CMakeFiles\radarimg_proc.dir\library.c.obj
radarimg_proc.dll: CMakeFiles\radarimg_proc.dir\soc_radar.c.obj
radarimg_proc.dll: CMakeFiles\radarimg_proc.dir\ssc_radar.c.obj
radarimg_proc.dll: CMakeFiles\radarimg_proc.dir\build.make
radarimg_proc.dll: CMakeFiles\radarimg_proc.dir\objects1.rsp
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --bold --progress-dir=D:\UnityProjects\osavr\Plugins\radarimg_proc\cmake-build-debug\CMakeFiles --progress-num=$(CMAKE_PROGRESS_4) "Linking C shared library radarimg_proc.dll"
	"C:\Program Files\JetBrains\CLion 2020.1.2\bin\cmake\win\bin\cmake.exe" -E vs_link_dll --intdir=CMakeFiles\radarimg_proc.dir --rc=C:\PROGRA~2\WI3CF2~1\10\bin\100183~1.0\x64\rc.exe --mt=C:\PROGRA~2\WI3CF2~1\10\bin\100183~1.0\x64\mt.exe --manifests  -- C:\PROGRA~2\MICROS~1\2019\COMMUN~1\VC\Tools\MSVC\1424~1.283\bin\Hostx64\x64\link.exe /nologo @CMakeFiles\radarimg_proc.dir\objects1.rsp @<<
 /out:radarimg_proc.dll /implib:radarimg_proc.lib /pdb:D:\UnityProjects\osavr\Plugins\radarimg_proc\cmake-build-debug\radarimg_proc.pdb /dll /version:0.0 /machine:x64 /debug /INCREMENTAL  kernel32.lib user32.lib gdi32.lib winspool.lib shell32.lib ole32.lib oleaut32.lib uuid.lib comdlg32.lib advapi32.lib  
<<

# Rule to build all files generated by this target.
CMakeFiles\radarimg_proc.dir\build: radarimg_proc.dll

.PHONY : CMakeFiles\radarimg_proc.dir\build

CMakeFiles\radarimg_proc.dir\clean:
	$(CMAKE_COMMAND) -P CMakeFiles\radarimg_proc.dir\cmake_clean.cmake
.PHONY : CMakeFiles\radarimg_proc.dir\clean

CMakeFiles\radarimg_proc.dir\depend:
	$(CMAKE_COMMAND) -E cmake_depends "NMake Makefiles" D:\UnityProjects\osavr\Plugins\radarimg_proc D:\UnityProjects\osavr\Plugins\radarimg_proc D:\UnityProjects\osavr\Plugins\radarimg_proc\cmake-build-debug D:\UnityProjects\osavr\Plugins\radarimg_proc\cmake-build-debug D:\UnityProjects\osavr\Plugins\radarimg_proc\cmake-build-debug\CMakeFiles\radarimg_proc.dir\DependInfo.cmake --color=$(COLOR)
.PHONY : CMakeFiles\radarimg_proc.dir\depend

