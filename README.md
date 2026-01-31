# MoveTogether

A co-located multiplayer mixed reality game for Meta Quest headsets that explores physical cooperative gameplay in shared spaces.

**Research Paper:** "MoveTogether: Exploring Physical Co-op Gameplay in Mixed-Reality"  
*Accepted for CHI'26 (forthcoming)*

## Overview

MoveTogether is a multiplayer MR experience where players in the same physical space collaborate to complete cooperative challenges. The game features multiple themed scenarios (Earth, Fire, Water) with physics-based interactions, requiring players to coordinate their movements and actions to succeed.

## Technical Requirements

### Software
- **Unity**: 2022.3.26f1
- **Meta XR SDK**: 71.0.0
- **Photon Fusion**: 1.1.10
- **Universal Render Pipeline (URP)**: 14.0.11
- **TextMesh Pro**: 3.0.6

### Hardware
- **Meta Quest 3, or Pro** (minimum 2 headsets for multiplayer)
- Shared physical play space with sufficient room for movement

### Additional Requirements
- **Photon Fusion Account**: Free tier available at [Photon Engine](https://www.photonengine.com/fusion)
- Active internet connection for multiplayer networking

## Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/ntu-hci-lab/MoveTogether.git
cd MoveTogether
```

### 2. Open in Unity
1. Launch Unity Hub
2. Click "Open" and select the `MoveTogether` folder
3. Ensure Unity version **2022.3.26f1** is installed
4. Wait for Unity to import and compile all assets

### 3. Configure Photon Fusion
1. Create a free account at [Photon Engine](https://www.photonengine.com/fusion)
2. Create a new Photon Fusion application in your dashboard
3. Copy your **App ID**
4. In Unity, navigate to: `Window > Photon Fusion > Fusion Hub`
5. Paste your App ID in the configuration settings
6. Alternatively, edit the Photon settings file directly at: `Assets/Resources/PhotonAppSettings.asset`

### 4. Meta Quest Build Configuration
1. Go to `File > Build Settings`
2. Select **Android** as the platform
3. Click "Switch Platform" if not already selected
4. Go to `Edit > Project Settings > XR Plug-in Management`
5. Enable **Oculus** under the Android tab
6. Configure build settings:
   - **Texture Compression**: ASTC
   - **Minimum API Level**: Android 10.0 (API Level 29) or higher

### 5. Build and Deploy
1. Connect your Meta Quest headset via USB or use Air Link
2. Enable Developer Mode on your headset
3. In Unity: `File > Build Settings > Build And Run`
4. Repeat the process for each headset you'll use

## Running the Game

### Starting a Session
1. Launch the game on the first headset (this player will be the host)
2. The game will automatically create a multiplayer room
3. Launch the game on additional headsets
4. Players will automatically connect to the same room when in proximity

### Gameplay
- The game features multiple cooperative scenarios requiring coordination between players
- **Earth Scene**: Collect and manage earth spirits with trampolines and physics interactions
- **Fire Scene**: Navigate fire-based challenges with timing and positioning
- **Water Scene**: Control bubble mechanics and water spirits cooperatively
- Players earn points by successfully completing objectives together
- Physical movement and spatial coordination are key to success

### Controls
- **Hand Tracking**: Natural hand gestures for object interaction
- **Controllers**: Meta Quest Touch controllers for precise manipulation
- **Movement**: Physical walking within your play space

## Project Structure

### Key Scenes
- `StartScene.unity` - Main menu and multiplayer lobby
- `GameScene.unity` - Primary gameplay hub
- `EarthScene.unity` - Earth-themed cooperative challenge
- `FireScene.unity` - Fire-themed cooperative challenge  
- `WaterScene.unity` - Water-themed cooperative challenge
- `TubeScene.unity` - Transition/portal scene

### Important Directories
- `Assets/Scripts/` - Core gameplay logic and networking code
- `Assets/Photon/` - Photon Fusion networking framework
- `Assets/Resources/` - Prefabs for networked objects and game elements
- `Assets/Scenes/` - All game scenes

## License

This project is open source under the MIT License. See [LICENSE](LICENSE) for details.

## Citation

If you use this project in your research, please cite our CHI'26 paper:

```
@inproceedings{movetogether2026,
  title={MoveTogether: Exploring Physical Co-op Gameplay in Mixed-Reality},
  booktitle={CHI Conference on Human Factors in Computing Systems},
  year={2026}
}
```

---

*Developed by NTU HCI Lab*
