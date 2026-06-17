AfricaTech: Smart Farm Simulation
A 2D interactive farming simulation built in Unity 6 as part of a coursework project exploring how technology can support sustainable agriculture in Africa.

🌍 Project Idea

Crop diseases spread fast and farmers often don't notice until it's too late. This simulation prototypes an early warning system where a handheld laser scanner and an autonomous drone work together to detect diseased crops and alert the farmer in real time.
🚀 Live Builds

🌐 WebGL (Play in Browser): https://play.unity.com/en/games/341c7f9c-5370-4c20-aada-07f18ec2376b/africatech-smart-farm-simulation

📲 Android APK: https://drive.google.com/drive/folders/1DHYzvypmvmnb5nnX5l1GxesEK_BI0iSZ?usp=sharing

🎮 How to Play


| Action | Desktop | Mobile | What it does |
|---|---|---|---|
| Move farmer | WASD or Arrow Keys | Left , right , top and down arrows | Moves the farmer around the field |
| Scan crops | click Spacebar | click right button | Fires the laser scanner |
| Drone telemetry | Automatic | Automatic | Drone flies and counts plants |
| Disease alerts | Automatic | Automatic | Warning pops up on screen |


## ⚙️ Features

- **Laser Scanner** — Hold Space to scan crops. The beam turns green for healthy plants and red for diseased ones
- **Autonomous Drone** — Flies over crops automatically, counts plants, and sends data to the HUD
- **Disease Alerts** — When the drone detects infection, a warning pops up on screen instantly
- **Animal Enclosure** — Animated cattle and sheep with idle and walking animations
- **Main Menu** — Includes instructions and a start button
- **Spatial Audio** — Drone sound pans left and right as it moves across the field
- **Background Music** — Layered ambient audio that doesn't clash with the drone sounds
- **Volume Control** — In-game volume slider and mute button to control audio on the fly
- **Mobile UI Controls** — On-screen virtual joystick and touch buttons for full mobile playability

## 🛠️ Built With

- **Unity 6** (6000.0.x LTS)
- **C#** Scripting
- **Unity New Input System**
- **Line Renderer**
- **Raycast**
- **Unity UI**
- **2D Sprite Animation**

💻 How to Open the Project Locally

1. Download or clone this repo and unzip it
2. Open **Unity Hub** and click **Add project from disk**
3. Select the extracted folder
4. Make sure you are using **Unity 6 (6000.0.x)**
5. Open `Assets/Scenes/MainMenu.unity` first
6. Press the ▶️ **Play** button to run it

📁 Project Structure
```text
Assets/
├── Animations/       # Animal animations
├── Audio/            # Background music and sound effects
├── Background/       # Background image
├── Controls/         # Input Action Maps (New Input System)
├── ImportedAs.../    # Imported third-party assets
├── Prefabs/          # Crops
├── Resources/        # Runtime-loaded assets
├── Scenes/           # MainMenu and SmartFarm scenes
└── Scripts/          # All C# game logic scripts
```

👨‍💻 Developer
Built by Bruno — passionate about using tech to solve real problems in Africa 🌍