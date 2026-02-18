# Game Design Document: IoThieves                                

<pre>
iothieves@kali:~$ iothieves --scan 192.168.200.0/24

<img width="1070" height="240" alt="IoThieves_logo" src="https://github.com/user-attachments/assets/7800168a-a720-4a57-b17c-53daa18cb6c6" />
                                                  
Copyright (c) 2025
Team Leader: Niccolò Sobrero                          
Designer: Matilde Sobberi                          
Architect: Daniel Bonardi                             
AI: Federico Tabellini                                
UI: Emanuele Sapio   

<b>Polimi Game Collective</b> [████████████████████████████████████] 100%

------------------------------------------------------------------------------------------------------------------
<b>> INITIALIZING MISSION_LOG...</b>
<b>> ACCESSING PROJECT_IO_THIEVES...</b>
------------------------------------------------------------------------------------------------------------------
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
data that will bring them down. But be careful: if you're 
spotted, it's game over. Can you pull off the heist without getting 
caught?
 
<h3><b>Genre:</b></h3>> Isometric Stealth
> Puzzle game
> Story-driven
> Low-poly

<h3><b>Players:</b></h3>> Single player

------------------------------------------------------------------------------------------------------------------
<h2><b>[ Technical Specs ]</b></h2><h3><b>Game view:</b></h3>> 3D, isometric, low-poly
    > <i>Visual Style:</i> Cartoonish rendering with bright, soft colors 
      and subtle shadows. Low-poly models with minimalistic shapes.

<h3><b>Language:</b></h3> Unity - C# 
<h3><b>Platform:</b></h3> PC - WebGL

------------------------------------------------------------------------------------------------------------------
<h2><b>[ GAMEPLAY ]</b></h2><h3><b>Game Flow</b></h3>The game is structured in multiple consecutive levels. Inside each level 
the player must explore the environment to find an item which is 
compelling with the story.

The item may not be stated at the beginning of the level forcing the 
player to explore the map to find it. Once the item is found a message 
is given to the player. After that the player can .....
<!--
Inside each level there are multiple npcs around and the player must hack various devices to distract them and sneak around without being caught. To hack a device the player click on it. Each hacked device has a different behaviour. 
 
may increase the suspicion of the npcs and put them in alarm and if a certain threshold is exceeded they will find the player causing a game over and making him restart the level. 
-->
There are 2 main categories of objets:
<b>1) Interactable objects:</b> 
   > <b>Throwable:</b> distract enemies
   > <b>Pick-ups:</b> collectables
   > <b>Minigame:</b> starts a minigame interaction
 
<b>2) Hackable objects:</b>
   > <b>Attract:</b> redirects enemies path
   > <b>Stun:</b> stuns NPCs if activated nearby
   > <b>Minigame:</b> starts a hacking minigame
   > <b>Downloadable:</b> collectables inside the PCs

<b>Hackable Devices List:</b>
 - PCs (minigame)
 - Washing Machine (attract)
 - ACs (stunning)
 - Printers (generate the throwable)
 - Alexa (attract)
 - Blender (stunning)
 - Kitchen hood (stunning)

 <b>Interactable Devices List:</b>
 - Keypad (minigame) 
 - Printers (throwable)
 - Cup (throwable)
 - Crowbar (objective)
 - Books (throwable) 
 - Toilet Paper (throwable)
 - Key (objective)
 - Peluches (objective)
 - Car (objective)
 
<h3><b>Core Mechanics</b></h3>> <b>Hacking system:</b> Select devices to hack and distract/stun NPCs.
> <b>Suspicion system:</b> Increases when PIP walk near NPCs and make the NPCs patrolling around
  > Past a certain suspicion threshold is Instant Game Over.
  > There are 2 possible alert states: yellow and red.
     > yellow alert state starts when the player goes near the NPC. It makes NPCs start patrolling
     > red alert state make NPCs patrolling around faster
> <b>Minigame:</b> Puzzle game that starts when you interact with some hackable and interactable devices:
  > Keypad Insertion: insert a code into the Keypad
  > Password Decryptor: find the password to access the pc
  > Desktop Navigation: navigate the folders and download files
  > HR Complaint: write a complaint on your Boss
 
<h3><b>Player Actions</b></h3>> <b>Interactions:</b> Point and click (mouse)
> <b>Hacking:</b> Point and click (mouse)
> <b>Typing:</b>b> keyboard 
> <b>Player Movement:</b> WASD (keyboard)
> <b>Camera Movement:</b> Automatically follows the player 

<h3><b>Challenges</b></h3>> Strategic Path
> Level Goals
> Puzzle Solving

<h3><b>Win and lose conditions:</b></h3>
> <b>Win conditions:</b>
  > Complete the level objectives
> <b>Lose conditions:</b>
  > Being spotted by a NPC
  > Suspicion system reaches a certain threshold
  
------------------------------------------------------------------------------------------------------------------
<h2><b>[ LEVEL DESIGN ]</b></h2><h3><b>Structure:</b></h3>>The gameplay is story-driven, so the levels are connected by the lore of the game.
<h3><b>Levels:</b></h3>
 > Garage
 > HR Offices
 > Boss House

<h3><b>Level progression</b></h3>> <b>Devices</b>: Start with few devices to interact with, then new devices with different behaviours are introduced.
 > <b>Enemies</b>: Start on easy levels with few enemies to handle, later levels will require handling more enemies. 
    > Paths: NPCs have their own routine, but it will be disrupted by the increase of suspicion level 
over a certain treshold, that change their state to patrolling/alert. 
    > Suspicion system: In easy levels each person has an individual suspicion level.
        > Avoiding alerted NPCs decreases the suspicion level with time.
> More level goals to achieve before leaving the level.

------------------------------------------------------------------------------------------------------------------
<h2><b>User Interface</b></h2>> <b>Main Menu</b>
> <b>In-Game Interface</b>
  > World Interface (MAP)
  > Gameover menu
  > Pause level menu
  > End level menu
  > Minigame Interface
    > Keypad 
    > Password Decryptor
    > Desktop 
    > HR Complaint

------------------------------------------------------------------------------------------------------------------

<h2><b>Characters</b></h2>> <b>The Hacker</b> (Narrator/Player: Hacking Interaction)
> <b>PIP:</b> The <i>Personal Infiltration Prototype</i> (Player: In-Game Interaction)
> <b>Enemies</b> 
  > Camera (Tutorial)
  > <b>NPCs:</b> 
    > Security guards x3 (Nina, Pinta and Santa Maria)
    > <b>Big Boss</b> (Boss)
    > Franco (Boss friend)
    > Rosalina (Boss' wife)
    > Claire (Boss' child 1)
    > Obscure ((Boss' child 2)
    > Piola (Boss' child 3)

------------------------------------------------------------------------------------------------------------------
<h2><b>[ RESOURCES ]</b></h2><h3><b>Graphical Elements:</b></h3>> <b>Setting:</b> Indoor environments (garage, offices, house).
> <b>Assets:</b> Furniture, Electronic devices, Household items, NPCs, Nature, Buildings, Cars.

<h3><b>SFX & VFX:</b></h3>> <b>SFX:</b> 
   > Dialogues: Typing sound.
   > Environmental: Interactions with hackable devices:
     > Electricity buzz
     > Washing machine noise
     > AC & Kitchen Hood sound
     > Doors
     > Printer
> <b>VFX:</b> 
  > Electric Sparks
  > Smoke
  > Particles fireflies
  > Music Notes
  > Visual indicators for suspicion (?!!)
> <b>Music:</b> IPod with 4 songs.
 
------------------------------------------------------------------------------------------------------------------
<b>> DISCONNECTING...</b>
_
</pre>


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




