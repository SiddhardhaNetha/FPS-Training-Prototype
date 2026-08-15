# FPS Training Prototype

A mobile-first FPS training prototype built with Unity as the foundation for a future LAN multiplayer FPS.

This project currently focuses on building and validating the core FPS gameplay loop, mobile controls, UI systems, target practice mechanics, and overall game flow before moving into multiplayer networking.

## V0.1 — Training Prototype

### What This Prototype Is

The V0.1 prototype provides a standalone FPS training environment where the player can:

* Navigate through the Home and Modes menus.
* Enter the Training Ground.
* Move using a dynamic mobile joystick.
* Aim using mobile touch controls.
* Fire and aim simultaneously with one finger.
* Shoot practice targets.
* Reload and manage ammunition.
* Deal damage to targets.
* Reset all training targets.
* Use the HUD and mini-map.
* Adjust gameplay settings.
* Return to the Home screen.

The purpose of V0.1 is to establish a stable gameplay foundation before multiplayer functionality is introduced.

## How to Install the APK

1. Download the latest V0.1 APK from the GitHub Release.
2. Transfer the APK to an Android device if necessary.
3. Open the APK on the device.
4. Allow installation from the requested source if Android asks for permission.
5. Install the application.
6. Launch **FPS Training Prototype**.

The V0.1 release is intended for Android devices.

## Controls

### Mobile

**Movement**

* Touch anywhere on the left side of the screen.
* The movement joystick appears at the touch location.
* Drag your thumb to move.
* Release your thumb to stop.

**Aiming**

* Drag on the right side of the screen to look around.

**Fire + Aim**

* Hold the Fire button to shoot.
* Continue dragging the same finger to aim while firing.

**Reload**

* Tap the Reload button to reload the weapon.

**Targets**

* Shoot targets to deal damage.
* Use **Reset Targets** to restore the training targets.

## Current Features

* Home menu
* Modes menu
* Training Ground
* Mobile movement system
* Dynamic mobile joystick
* Mobile aiming
* Simultaneous fire and aim
* Shooting system
* Fire rate control
* Magazine and reserve ammunition
* Reload system
* Weapon recoil
* Muzzle flash
* Weapon audio
* Hit detection
* Target damage system
* Target reset system
* Hit marker
* Health and ammunition HUD
* Mini-map
* Target markers
* Settings menu
* Android APK build
* Git version control and V0.1 release workflow

## Known Bugs / Limitations

V0.1 is a prototype and is not intended to represent the final game.

Known limitations include:

* LAN multiplayer networking is not implemented yet.
* The prototype currently focuses on training gameplay rather than complete multiplayer functionality.
* Mobile UI and controls may require further tuning across different screen sizes and aspect ratios.
* Performance and device compatibility have not been tested across a wide range of Android devices.
* Additional polish, animations, audio, and visual effects are planned for future versions.

## Next Milestone

### V0.2 — LAN Networking

The next major milestone is to introduce LAN multiplayer functionality.

Planned work includes:

* LAN host and client functionality
* Player connection and synchronization
* Multiplayer player spawning
* Networked movement
* Networked shooting
* Networked damage
* Match/session management
* Basic Team Deathmatch functionality

The goal is to evolve the current single-player training prototype into a functional LAN multiplayer FPS prototype.

## Project Status

**Version:** V0.1 — Training Prototype
**Status:** Frozen
**Platform:** Android
**Engine:** Unity
**Next Milestone:** LAN Networking

## Documentation

Screenshots of the V0.1 prototype are available in:

`Documentation/Screenshots/v0.1/`

The repository also contains the development history and version information for the project.

## License

This project is currently a personal development project and is not licensed for redistribution or commercial use.
