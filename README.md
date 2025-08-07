# The Fox has a Gun

## Preview

[![Watch the demo](https://img.shields.io/badge/Watch-Video-red?logo=youtube)](https://youtu.be/FFSx_IFpFU0)

[Play it on Itch.io](https://saaami.itch.io/the-fox-has-a-gun)

## Project Overview

This project is a Game AI simulation built for the **Game AI** module. The objective is to simulate a simple combat arena between a player and AI enemies using:

- Procedural terrain generation (Perlin noise + layers)
- Scripted FSM (Finite State Machine) enemy behavior
- Collision handling, health, and basic combat
- AI state transitions and responsive feedback


## Terrain Generation

The terrain is procedurally generated using a **custom Perlin noise implementation**, with enhancements such as:

- **Octaves** – Multiple layers of noise for finer details  
- **Lacunarity** – Controls spacing between octaves  
- **Persistence** – Balances influence of small features  
- **Strength & Frequency** – Adjusts hill size and detail level

### Height-Based Coloring
- **Blue** = lowest areas (valleys/water)
- **Green** = mid-ground
- **White** = highest peaks

### Technical Details
- Mesh dynamically generated using vertices, triangles, and UVs
- All terrain parameters (e.g. size, frequency) are public and customizable
- Efficient mesh recalculation avoids unnecessary performance hits
- Fully scalable via variables like `terrainWidth` and `terrainLength`


## Player Features

- 4-directional movement using **WASD**
- Shoots bullets (small spheres) with cooldown
- Manual collision handling with terrain and enemies
- Health point (HP) system implemented


## AI Enemy Design

### Patrolling NPC (FSM)
States:
- **Patrol** – Moves between predefined patrol points
- **Idle** – Pauses at patrol points or when losing player
- **Shooting** – Fires at the player if within range

Transitions based on:
- Distance to player
- Cooldown timers for shooting
- State entry conditions (sight, proximity)

### Chasing NPC (FSM)
States:
- **Idle** – Awaits player presence
- **Chasing** – Pursues player if detected
- **Retreat** – Returns to idle if player escapes

### Visual Feedback
- HUD displays each bot's:
  - **Bot ID**
  - **Current HP**
  - **Current State**

## Extended Behaviour Features

- Adjustable patrol speed, detection radius, and fire rate for difficulty scaling
- Cooldown systems to fix FSM transition bugs (prevent looping)
- FSM diagrams and behavior logic explained in the report

## License

This project is for educational and demonstration purposes only.
