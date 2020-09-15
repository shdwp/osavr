## What's this?
__9K33M2 Îñà__ simulator built in Unity, currently in early stages of development. Short demonstration:

[![Osa](https://img.youtube.com/vi/q2nNWoEEIiA/0.jpg)](https://youtu.be/q2nNWoEEIiA)

## 9K33M
Better known in western world as a __SA-8 Gecko__, it's a Soviet surface-to-air missile system, designed to track (up to 45km) and engage (up to 27km) air targets.

## Simulator features
X band radar simulation calculating O2 molecular dispersion as well as weather dispersion based on predefined coefficients, radar beamshape losses, approximate system losses, radar cross-section fluctiation based on model geometry and normal maps, approximate occlusion calculations, doppler effect simulation, all done on GPU with C native plugin processing results and preparing data for the cockpit instruments.

## Systems progress
Below is the implementation status of various systems found in _9K33M2_, at least those that are currently in scope. In future it may be expanded with _BAZ-5939_ chassis, making it possible to move the vehicle around.

* __SOC__ - Target Search Radar
    * [done] General radar simulation, all 3 beams
    * [done] Automatic active beam cycling search modes
    * [done] Two modes of IFF
    * Doppler and wind doppler filters
    * Open/closed receive modes
    * Radar jamming modes and filters

* __SSC__ - Target Tracking Radar
    * [done] General radar simulation
    * [done] Target signal fluctuation and ground clutter
    * Target doppler and wind doppler filters
    * Moving-away jamming mode countermeasure

* __SUA__ - Target Tracking System
    * [done] Auto target acquisition
    * [done] Azimuth, elevation & distance SSC channels
    * [done] Semi-manual distance tracking mode
    * Low-flying target mode
    * Semi-manual target tracking (by TV set)

* __TOV__ - TV Set
    * General TV set simulation

* __SVR/SPK__ - Missile Guiding System
    * General missile launch and guiding simulation
    * SSC scope missile zoom modes
    * Jammed signal mode
