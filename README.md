# Mori Tenebris

**Latin meaning:** *Mori Tenebris* roughly translates to "Dying in Darkness" or "Death in Shadows".

---

## Overview

*Mori Tenebris* is a pixel isometric soulslike game with roguelike elements. A hardcore action RPG where the player takes on the role of a knight journeying through 10 unique zones, defeating enemies and powerful bosses.

---

## Core Gameplay

- The game is divided into 10 progressively challenging zones.
- Defeating a zone's boss unlocks the next zone and saves progress.
- Between runs, players return to a central hub to upgrade stats and purchase gear.

---

## Current Features & Additional Systems

- ✅ Fully working player and enemy combat systems.
- ✅ Player core systems: stats, health, armor, damage, level progression, save/load.
- ✅ FSM-based movement and state system with generic switching via dictionary.
- ✅ Flexible combo attack system with overridable logic and event-based effects.
- ✅ Inventory system: item slot management and initialization.
- ✅ Effects system including buffs and debuffs.
- ✅ Full AI assembly featuring scalable attack architecture, movement, effect application, and EnemyData structures.
- ✅ Basic UI including hitpoints and effect status indicators.
- ✅ In the `MetaScene` branch: development of the dialog system featuring:
  - N-ary dialog trees.
  - Dialog nodes.
  - Custom GraphView editor for nodes.
  - Conversion of serializable nodes to runtime nodes.
  - FSM for dialog state switching.

---

## Tech Stack

- **Engine:** Unity 2022.3.59f1
- **Language:** C#
- **Architecture:** Zenject (Dependency Injection)
- **Visuals & FX:** DOTween
- **Assets Management:** Addressables

---

## Contacts & Support

Author: Ivan Shulgin (Ar0cka)  
GitHub: [https://github.com/Ar0cka](https://github.com/Ar0cka)  
Telegram: @ar0cka  
Email: vanoshul@gmail.com

---

## License

Mozilla Public License (MPL) 2.0

# Build & Run Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/Ar0cka/Mori-tenebris.git
   ```
   Open the project in Unity 2022.3.59f1.
   Build and run the game through the Unity Editor.
   For more details on usage and ongoing development, refer to the documentation in the MetaScene branch (dialog system).
