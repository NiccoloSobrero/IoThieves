# Game Design Document: IoThieves                                

<pre>
iothieves@kali:~$ iothieves --scan 192.168.200.0/24

<img width="1070" height="240" alt="IoThieves_logo" src="https://github.com/user-attachments/assets/7800168a-a720-4a57-b17c-53daa18cb6c6" />
 <!-- 
██╗ ██████╗ ████████╗██╗  ██╗██╗███████╗██╗   ██╗███████╗███████╗
██║██╔═══██╗╚══██╔══╝██║  ██║██║██╔════╝██║   ██║██╔════╝██╔════╝
██║██║   ██║   ██║   ███████║██║█████╗  ██║   ██║█████╗  ███████╗
██║██║   ██║   ██║   ██╔══██║██║██╔══╝  ╚██╗ ██╔╝██╔══╝  ╚════██║
██║╚██████╔╝   ██║   ██║  ██║██║███████╗ ╚████╔╝ ███████╗███████║
╚═╝ ╚═════╝    ╚═╝   ╚═╝  ╚═╝╚═╝╚══════╝  ╚═══╝  ╚══════╝╚══════╝ -->      
                                                  
Copyright (c) 2025                
Team Leader: Niccolò Sobrero                          
Designer: Matilde Sobberi                          
Architect: Daniel Bonardi                             
AI: Federico Tabellini                                
UI: Emanuele Sapio   

<b>Polimi Game Collective</b> [████████████████████████████████████] 100%

-----------------------------------------------------------------------
<b>> INITIALIZING MISSION_LOG...</b>
<b>> ACCESSING PROJECT_IO_THIEVES...</b>
-----------------------------------------------------------------------
<h2><b>[OVERVIEW AND VISION STATEMENT]</b></h2> 
It's the 20XX. Technology has spread across the globe—everything is 
connected to the internet, and smart devices have become integral to 
daily life. But this hyperconnected world has created new opportunities 
for theft.

You're an ordinary person, just fired from your job by a wealthy, 
arrogant boss. Furious and desperate for revenge, you decide to 
infiltrate his life and expose his secrets. But you can't do it alone. 
So you build <b>PIP</b>—your Personal Infiltration Prototype—a stealth 
robot that will be your partner in crime.

Armed with hacking skills and a loyal bot, you'll sneak into homes 
and offices, hack smart devices to distract their owners, and steal 
the documents that will bring them down. But be careful: if you're 
spotted, it's game over. Can you pull off the heist without getting 
caught?
<!-- 
<img width="3817" height="281" alt="overview" src="https://github.com/user-attachments/assets/e4ba5168-6400-4cb2-9536-ccd9a4ac0dab" />

<img width="3817" height="281" alt="overview_black" src="https://github.com/user-attachments/assets/3a98a921-d701-44f3-8102-f85ba7caaebc" /> --> <h3><b>Genre:</b></h3>> Isometric Stealth
> Puzzle game
> Story-driven
> Low-poly

<i><b>__Changelog (17/10/25)</b></i>: Stealth-like is considered the best choice due 
to the fact that it is easier to implement both in terms of coding 
and design.

<i><b>__Changelog (7/12/25)</b></i>: Stealth-like is now considered a sub-genre, 
we are mainly focused on puzzle game.

<h3><b>Players:</b></h3>> Single player
> Co-op
 
<i><b>__Changelog (10/10/25)</b></i>: The base idea is to have a single player game since 
multiplayer is not a requirement for the course.


<h2><b>[ Technical Specs ]</b></h2><h3><b>Game view:</b></h3>> 3D, isometric, low-poly
    > <i>Visual Style:</i> Cartoonish rendering with bright, soft colors 
      and subtle shadows. Low-poly models with minimalistic shapes.
> 2D, top down, pixel art

<i><b>__Changelog(17/10/25)</b></i>: 3D is likely the best choice to fit the game style and mechanics.
2D still remains a valid option. This part requires further discussion, 
especially with the designer.
<h3><b>Language:</b></h3> Unity - C# 
<h3><b>Platform:</b></h3> PC - WebGL


<h2><b>[ GAMEPLAY ]</b></h2><h3><b>Game Flow</b></h3>The game is structured in multiple consecutive levels. Inside each level 
the player must explore the environment to find an item which is 
compelling with the story.

The item may not be stated at the beginning of the level forcing the 
player to explore the map to find it. Once the item is found a message 
is given to the player. After that the player can reach the exit.
<!--
Inside each level there are multiple npcs around and the player must hack various devices to distract them and sneak around without being caught. To hack a device the player must solve a small puzzle/minigame. Each hacked device has a different behaviour and must be fixed from the npcs. Hacking too much may increase the suspicion of the npcs and put them in alarm and if a certain threshold is exceeded they will find the player causing a game over and making him restart the level. 
-->
There are 2 main categories of objets:
<b>1) Interactable objects:</b> 
   > <b>Throwable:</b> distract enemies
   > <b>Pick-ups:</b> collectables
   > <b>Minigame:</b> starts a minigame interaction

<i><b>__Changelog (10/11/25)</b></i>: pick-ups removed and added 
Downloadable in Hackables 
 
<b>2) Hackable objects:</b>
   > <b>Attract:</b> redirects enemies path
   > <b>Stun:</b> stuns NPCs if activated nearby
   > <b>Minigame:</b> starts a hacking minigame
   > <b>Downloadable:</b> collectables inside the PCs

<h3><b>Core Mechanics</b></h3>> <b>Hacking system:</b> Select devices to hack and distract/stun NPCs.
> <b>Suspicion system:</b> Increases with hacking activity.
  > Past a certain suspicion threshold is Instant Game Over.
  > Higher suspicion increases NPCs FoV.
> Overload system: Too many hacked devices cause a game over. 
> <b>Minigame:</b> Puzzle game to hack devices --> puzzle game + timer
 
<i><b>__Changelog (17/10/25)</b></i>: Overload system has been excluded from the list of mechanics.

<i><b>__Changelog (7/12/25)</b></i>: Now Suspicion system increases when PIP walk near NPCs 
and make the NPCs patrolling around

<i><b>__Changelog (7/12/25)</b></i>: No timer for the minigames <!--only for important devices--></i>

<b>Hackable Devices List:</b>
 - Smart lights, Alexa, TV (louder noise = more suspicion)
 - Roomba (cyclic path distraction)
 - Microwave, Induction stove, Smart fridge
 - Japanese toilet (spitting water/flushing)

<i><b>__Changelog (29/01/26)</b></i>: No smart lights, maybe Microwave, Induction stove,
 Smart fridge, Japanese toilet, Roomba.
<!--
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
 -->
<h3><b>Player Actions</b></h3>> <b>Hacking/Interactions:</b> Point and click (mouse)
> <b>Camera Movement:</b> 3 possible solutions:
   > Automatically follows the bot 
   > WASD (keyboard)
   > Pan and zoom (mouse)
> <b>Bot Movement:</b> Two possible solutions:
   > WASD (keyboard)
   > Point and click (mouse)

 <i><b>__Changelog (17/10/25)</b></i>: Indecision on inputs for camera movement and bot movement.

 <i><b>__Changelog (7/12/25)</b></i>: We decided to make the automatic movement of the camera 
 and the WASD movement for the bot
 
<h3><b>Challenges</b></h3>> Strategic Path
> Level Goals
> Puzzle Solving
> Time Pressure
> Optional Collectable Items
 
<i><b>__Changelog (7/12/25)</b></i>: Time Pressure removed

-----------------------------------------------------------------------
<b>> DISCONNECTING...</b>
_
</pre>

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
 __*Changelog (7/12/25)*__: no more skill tree


### Win and lose conditions
 - Win conditions: 
    - satisfying objectives of each level
    - exit the level without noticing
    
 - Lose conditions:
    - Being spotted from an “enemy”
    - Suspicion system reaches a certain threshold


---

## User Interface
- *Main Menu*
- *In-Game Interface*:
  - Minigame Interface
  - World Interface
  - Gameover menu
  - Pause level menu
  - End level menu

---

## Characters
- *The Hacker (Player)*
- *PIP: The Bot (Player)*
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



### Milestones:
- Hacking System
- Suspicion system
- Minigames
- UI:
    - MainMenu:
        - Play
        - Settings
        - Skills
        - Select Level
        - Quit
    - WorldUI:
        - Suspicion level (top-center)
        - Map (top-left) (?)
    - MinigamesUI
    - PauseMenu:
        - Play
        - Settings
        - Quit
    - EndLevelMenu:
        - NextLevel
        - Skills
        - MainMenu
    - GameOverMenu:
        - Start
        - Settings
        - Quit
- Level Design (for each level):
    - Environment
    - Characters
    - Hackable devices
    - Design a path to the goal (intended solution(s))
- Cinematics
- Camera System
- NPC patrolling:
    - movement by code (NavMesh is expensive)
    - FOV visible to the player with a raycast
- PIP movement system:
    - NavMesh
- Save/Load system (?)
- Economy system (Balancing money stolen with skills price) - Emanuele (?)
- Skill Tree system




