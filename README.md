# Game Design Document: IoThieves

## Game Title: IoThieves

### Overview and Vision Statement
In a world where everything is connected to the Internet and fully automated, the Internet of Things has not only improved 
people’s quality of life, but has also reinvented the art of theft. 
Hack smart buildings to obtain sensitive informations from people's connected devices and computers by distracting them hacking their smart devices and letting your robotic companion sneak inside. But beware: if the owners notice you, they will call the police. Can you succeed without being caught? 

*Genre*:
 - Stealth-like
 - Puzzle game

Changelog (17/10/25): Stealth-like is considered the best choice due to the fact that it is easier to implement both in terms of coding and design.


*Players*:
 - Single player

## Technical Specs

*Game view*:
 - 3D, isometric, low poly
    - Visual Style: Cartoonish rendering with bright, soft colors and subtle shadows. Low-poly models with minimalistic shapes.
 - 2D, top down, pixel art

__*Changelog (17/10/25)*__: 3D is likely the best choice to fit the game style and mechanics. 2D still remains a valid option. This part requires further discussion, especially with the designer.

*Language*:
Unity - C#

*Platform*:
PC - WebGL

---

## Gameplay


### Game Flow
TBD

### Core Mechanics
 - Hacking system: The player can select a device to hack. Hacked devices could cause different interactions but most of the time they catch the attention of the homeowners.
 - Suspicion system: Hacking too many devices increases the owner's suspicion level
    - Past a certain suspicion threshold is instant "Game Over”
    - Increasing the suspicion level gives the owner a larger FoV
 - Overload system: Too many hacked devices cause a game over.
    - Changelog (17/10/25): Overload system has been excluded from the list of mechanics.
 - Minigame to hack a device → puzzle game + timer
 - Interactable devices:
    - Smart lights → ON/OFF
    - Alexa → ON/OFF (longer time to disable)
    - TV ON/OFF + Audio level (Louder noise attracts people from further away and takes more time to fix but raises suspicion level)
    - Roomba → Set a cyclic path, the owner will follow it for some time and then turn it off
    - Something with a huge detection range that forces the owner to “fix" it, even if he is fixing something else causing a timer reset on the fix
    - Microwave → ON/OFF
    - Smart fridge → OFF
    - Thermostat/Thermometer/Air conditioning → change the temperature in the house
    - Bimbi → ON/OFF
    - Washing machine → ON/OFF + Audio level 
    - Shutters → up and down
    - Japanese toilet →  starts spitting water (hard → must dry the floor)/flushing (easy → only suspicion)
    - Induction stove → heating up the surface and burns things in the pans
      

### Player actions
 - Device hacking: Point and click (mouse)
 - Camera movement:
    - Automatically follows the bot or some point of interest
    - WASD (keyboard)
    - Pan and zoom (mouse)
 - Bot movement: 
    - WASD (keyboard)
    - Point and click (mouse)
 - __*Changelog (17/10/25)*__: Indecision on inputs for camera movement and bot movement.


### Challenges
- *Time Pressure*
- *Strategic Path*

---

## Design

### Level progression
 - Devices: Start with device with simple actions, for example smart lights with just an On/Off feature, then on harder levels introduce new devices with also more actions, for example TV also has an audio level 
 - "Home owner" :Start on easy levels with just one person to handle, later levels will require handling more people. Home owners might have their own routine but it might happen that something disrupts it, like a random phone call or a package delivery.
 - Random events might decrease the suspicion level.
 - Suspicion system: In easy levels each person has an individual suspicion level, harder levels might implement a suspicion level shared among all the people (on an "organization" level) In easy level the suspicion level might not affect the homeowner's FoV, this mechanic could be implemented in later levels 
 - Hacking limitations: We could set a maximum number of devices hacked at the same time (Might be different depending on the level)
    - __*Changelog (17/10/25)*__: Hacking limitations has been removed from the list of mechanics for level progression since it overlaps with the suspicion system.
 - Game difficulty increases by:
   - adding more people
   - consecutive objectives
   - more difficult hacking puzzles
   - different suspicion threshold depending on the context
- No grading system for the performance inside a level, you either pass or fail.
 - Skill Tree(?)
 - 

### Win and lose conditions
 - Win conditions: 
    - Reaching a certain threshold of stolen money(?)
    - Stealing the assigned item/data
 - Lose conditions:
    - Being spotted from an “enemy”
    - Suspicion system reaches a certain threshold



---

## User Interface
- *Main Menu*
- *In-Game Interface*:
  - Minigame Interface
  - World Interface

---

## Characters
- *The Hacker (Player)*
- *The Bot (Player)*
- *Home owners (Enemies)*
- *NPCs* (TBD)

---

## Story
To be written later.
 - Possible final: What if the bot and the hacker were the same entity? But what was his motive?

---

## World
Each level represents...

1. *Tutorial Level*
2. ...

---

## Media List

### Scene Assets
1. *Tutorial Scene*: Hacker's garage
2. ...

### Music
Something tense

### Sound Effects
- *Movement Sounds*: Footsteps based on surface type (wood, carpet, tile, etc.).
- *Environmental Sounds*: Interactions with hackable devices (e.g. flickering, switches, phone sounds...).
- *UI Sounds*: Menu navigation, time bar, game start and end cues.
- *Tension Indicators*: Subtle audio cues for the suspicion system (e.g., heartbeat, home owner anger).
