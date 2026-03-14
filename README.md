# Llama Masters

![Unity](https://img.shields.io/badge/Unity-2022+-000000?style=flat&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-Game%20Logic-239120?style=flat&logo=csharp&logoColor=white)
![HLSL](https://img.shields.io/badge/HLSL-Shaders-5C2D91?style=flat)
![UP GDL](https://img.shields.io/badge/UP-Campus%20Guadalajara-8B0000?style=flat)

Party game built in Unity. Players compete across a series of minigames — each with its own ruleset, point system, and debuff mechanics. The game tracks cumulative scores across rounds and supports duel-based elimination.

---

## Gameplay Systems

- **Minigames** — MG1 and MG2 with independent logic and win conditions
- **Duel mode** — direct head-to-head elimination round
- **Points system** — `LlamaPoints.cs` tracks and persists scores across scenes via `KeepOnLoad`
- **Debuffs** — `Debuff.cs` applies temporary handicaps to players
- **Manager scripts** — handle scene transitions, state, and game flow

## Project Structure

```
LlamaRemaster/
└── Assets/
    ├── Scripts/
    │   ├── Manager Scripts/   — game state and scene management
    │   ├── MG1/               — minigame 1 logic
    │   ├── MG2 Scripts/       — minigame 2 logic
    │   ├── Duel/              — duel mode logic
    │   ├── LlamaPoints.cs     — score tracking
    │   ├── PlayerData.cs      — player state
    │   ├── Debuff.cs          — debuff application
    │   └── KeepOnLoad.cs      — persistent data across scenes
    ├── Animations/
    ├── Audio/
    ├── Prefabs/
    ├── Scenes/
    └── Materias/
```

## Running the Project

Open the project in Unity 2022 or later. Load the main scene from `Assets/Scenes/` and hit Play. No external dependencies required.
