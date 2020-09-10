# SOC
range max 45km
turn speed = 33 RPM
square-type impulse length 0.45 mks
freq 1 GHz
1 beam - 2deg horizontal, 4deg vert
2 beam - 2deg horizontal, 4deg vert
3 beam - 2deg horizontal, 5deg vert
available when moving (due to stabilization)

# SOC Scope
Available scope ranges: 
* 0-15:
    * 2km, 7km 12km
* 0-35:
    * 7 each 5km apart from each other

SOC azimuth indicator in russian imperial - 60-00 is 360deg

# SSC
range max 28km
azimuth +-330 deg
elev. -12deg +78deg
resolution 0.2deg, 55m
beam horizontal 0.17-0.24, vert 0.13 - 0.17
two beam types - ultranarrow and wide (1st used when tracking and 2st used for target capture)
not available when moving
target gate 1.5km
SHT->SDC - decides what to amplify on the scope - all of it or 1.5 tgate
max azimuth rate 30deg/s (0.6 per 20ms), elevation = 15deg/s (0.3 per 20ms)
SSC elevation angle in russian imperial

# Radar Numbers
mig19 detection range at 1000m - 35km (+7km) = 42km (42011.9m)
clean weather loss -33.6db
0.8dB clouds loss - -67dB
mig19 doppler filter range at 1000m - 27km (+7km) = 34km

system loss 11 dB
beamshape loss x^2, -3dB at FOV edge
radar crossection loss for B26 is 5-30dB

nominal level at ideal 0:
67dB + 11 dB + 15dB = 93dB
