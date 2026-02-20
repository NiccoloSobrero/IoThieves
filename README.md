# Game Design Document: IoThieves                                
<blockquote>
<samp>
  
iothieves@kali:~$ iothieves --scan 192.168.200.0/24
  
<br>

<img width="1070" height="240" alt="IoThieves_logo" src="https://github.com/user-attachments/assets/7800168a-a720-4a57-b17c-53daa18cb6c6" />

<br>
&nbsp;
<br>  

### Copyright (c) 2025 <br>
&gt; **Team Leader**: Niccolò Sobrero                          
&gt; **Designer**: Matilde Sobberi                        
&gt; **Architect**: Daniel Bonardi                             
&gt; **AI**: Federico Tabellini                                
&gt; **UI**: Emanuele Sapio   

<br>

**Polimi Game Collective** [████████████████████████████████████] 100%

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
**> INITIALIZING MISSION_LOG...** <br>
**> ACCESSING PROJECT_IO_THIEVES...** 

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

## [OVERVIEW AND VISION STATEMENT] 
It's the 20XX. Technology has spread across the globe—everything is connected to the internet, and smart devices have become integral to daily life. But this hyperconnected world has created new opportunities for theft.

You're an ordinary person, just fired from your job by a wealthy, arrogant boss. <br>
Furious and desperate for revenge, you decide to infiltrate his life and expose his secrets. But you can't do it alone. So you build **_PIP_**, your **_Personal Infiltration Prototype_**, a stealth robot that will be your partner in crime.

Armed with hacking skills and a loyal bot, you'll sneak into homes and offices, hack smart devices to distract their owners, and steal data that will bring them down. But be careful: if you're spotted, it's **GAME OVER**. <br>
Can you pull off the heist without getting caught?
 
### Genre:
&gt; Isometric Stealth <br>
&gt; Puzzle game <br>
&gt; Story-driven <br>
&gt; Low-poly 


### Players:
&gt; Single player

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
## [ Technical Specs ]
### Game view:
&gt; 3D, isometric, low-poly <br>
&nbsp;&nbsp; &gt; _Visual Style:_ Cartoonish rendering with bright, soft colors and subtle shadows. <br>
&nbsp;&nbsp;&nbsp;&nbsp; Low-poly models with minimalistic shapes.

### Language:
&gt; Unity - C#

### Platform:
&gt; PC - WebGL

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
## [ GAMEPLAY ]
### Game Flow
The game is structured across three consecutive levels. <br>
Within each level, the player must explore the environment to find specific items necessary to complete the level objectives, compelled with the story. <br>

Some objectives are revealed sequentially as the player advances. Once an objective is successfully fulfilled, the next task is triggered. This cycle continues until the final objective is reached: _finding the exit and escaping the level._  <br>

Each level is populated by NPCs patrolling the area; the core mechanic of the game involves hacking the IoT devices to distract or bypass them. The player must strategically interact with the environment to sneak around without being caught. Every hackable device possesses a unique behavior, requiring the player to adapt their tactics based on the available technology in each room. <br>

Walking too closely to the NPCs may increase the their suspicion level and put them in alert state. If a certain threshold is exceeded, the player is detected, resulting in an immediate Game Over. In this event, the player can restart from the most recent checkpoint.

<br>

There are 2 main categories of objets: <br> <br>
**Interactable objects:** <br>
   &gt; **Throwable:** distract enemies <br>
   &gt; **Pick-ups:** collectables <br>
   &gt; **Minigame:** starts a minigame interaction <br>

   <br>
 
**Hackable objects:** <br>
   &gt; **Attract:** redirects enemies path <br>
   &gt; **Stun:** stuns NPCs if activated nearby  <br>
   &gt; **Minigame:** starts a hacking minigame <br>
   &gt; **Downloadable:** collectables inside the PCs <br>

<br>

**Hackable Devices List:** <br>
 &gt; PCs (minigame) <br>
 &gt; Washing Machine (attract) <br>
 &gt; ACs (stunning) <br>
 &gt; Printers (generate the throwable) <br>
 &gt; Alexa (attract) <br>
 &gt; Blender (stunning) <br>
 &gt; Kitchen hood (stunning) <br>

 <br>

 **Interactable Devices List:** <br>
 &gt; Keypad (minigame)  <br>
 &gt; Printers (throwable)<br>
 &gt; Cup (throwable) <br>
 &gt; Crowbar (objective) <br>
 &gt; Books (throwable)  <br>
 &gt; Toilet Paper (throwable) <br>
 &gt; Key (objective) <br>
 &gt; Peluches (objective) <br>
 &gt; Car (objective) <br>

<br>
 
### Core Mechanics
&gt; **Hacking system:** Select devices to hack and distract/stun NPCs. <br>
&gt; **Suspicion system:** Increases when PIP walk near NPCs and make the NPCs patrolling around. <br>
  &nbsp;&nbsp; &gt; Past a certain suspicion threshold is Instant Game Over. <br>
  &nbsp;&nbsp; &gt; There are 2 possible alert states: yellow and red. <br>
      &nbsp;&nbsp; &nbsp;&nbsp; &gt; yellow alert state starts when the player goes near the NPC. It makes NPCs start patrolling. <br>
      &nbsp;&nbsp; &nbsp;&nbsp; &gt; red alert state make NPCs patrolling around faster. <br>
&gt; **Minigame:** Puzzle game that starts when you interact with some hackable and interactable devices: <br>
  &nbsp;&nbsp; &gt; _Keypad Insertion:_ insert a code into the Keypad. <br>
  &nbsp;&nbsp; &gt; _Password Decryptor_: find the password to access the pc. <br>
  &nbsp;&nbsp; &gt; _Desktop Navigation:_ navigate the folders and download files. <br>
  &nbsp;&nbsp; &gt; _HR Complaint:_ write a complaint on your Boss. <br>

### Player Actions 
&gt; **Interactions:** Point and click (mouse) <br>
&gt; **Hacking:** Point and click (mouse) <br>
&gt; **Typing:** keyboard <br>
&gt; **Player Movement:** WASD (keyboard) <br>
&gt; **Camera Movement:** Automatically follows the player <br>

### Challenges 
&gt; Strategic Path <br>
&gt; Level Goals <br>
&gt; Puzzle Solving <br>

### Win and lose conditions:
&gt; **Win conditions:** <br>
  &nbsp;&nbsp; &gt; Complete the level objectives <br>
&gt; **Lose conditions:** <br>
  &nbsp;&nbsp; &gt; Being spotted by a NPC <br>
  &nbsp;&nbsp; &gt; Suspicion system reaches a certain threshold <br>
  
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
## [ LEVEL DESIGN ]
### Structure:
&gt; The gameplay is story-driven, so the levels are connected by the lore of the game.

### Levels:
 &gt; Garage <br>
 &gt; HR Offices <br>
 &gt; Boss House <br>

### Level progression
&gt; **Devices:** Start with few devices to interact with, then new devices with different behaviours are introduced. <br>
&gt; **Enemies:** Start on easy levels with few enemies to handle, later levels will require handling more enemies. <br>
  &nbsp;&nbsp; &gt; Paths: NPCs have their own routine, but it will be disrupted by the increase of suspicion level 
over a certain treshold, that change their state to patrolling/alert. <br>
  &nbsp;&nbsp; &gt; Suspicion system: In easy levels each person has an individual suspicion level. <br>
        &nbsp;&nbsp; &nbsp;&nbsp; &gt; Avoiding alerted NPCs decreases the suspicion level with time. <br>
&gt; More level goals to achieve before leaving the level. <br>

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
## User Interface
&gt; **Main Menu** <br>
&gt; **In-Game Interface** <br>
  &nbsp;&nbsp; &gt; World Interface (MAP) <br>
  &nbsp;&nbsp; &gt; Gameover menu <br>
  &nbsp;&nbsp; &gt; Pause level menu <br>
  &nbsp;&nbsp; &gt; End level menu <br>
  &nbsp;&nbsp; &gt; Minigame Interface <br>
    &nbsp;&nbsp; &nbsp;&nbsp; &gt; Keypad  <br>
    &nbsp;&nbsp; &nbsp;&nbsp; &gt; Password Decryptor <br>
    &nbsp;&nbsp; &nbsp;&nbsp; &gt; Desktop <br>
    &nbsp;&nbsp; &nbsp;&nbsp; &gt; HR Complaint <br>

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

## Characters
&gt; **The Hacker** (Narrator/Player: Hacking Interaction) <br>
&gt; **_PIP:_** The **_Personal Infiltration Prototype_** robot. (Player: In-Game Interaction)  <br>
&gt; **Enemies**  <br>
  &nbsp;&nbsp; &gt; Camera (Tutorial)  <br>
  &nbsp;&nbsp; &gt; **NPCs:**  <br>
    &nbsp;&nbsp; &nbsp;&nbsp; &gt; Security guards x3 (Nina, Pinta and Santa Maria)  <br>
    &nbsp;&nbsp; &nbsp;&nbsp; &gt; **Big Boss**(Boss)  <br>
    &nbsp;&nbsp; &nbsp;&nbsp; &gt; Franco (Boss friend)  <br>
    &nbsp;&nbsp; &nbsp;&nbsp; &gt; Rosalina (Boss' wife)  <br>
    &nbsp;&nbsp; &nbsp;&nbsp; &gt; Claire (Boss' child 1)  <br>
    &nbsp;&nbsp; &nbsp;&nbsp; &gt; Obscure ((Boss' child 2)  <br>
    &nbsp;&nbsp; &nbsp;&nbsp; &gt; Piola (Boss' child 3)  <br>

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
## [ RESOURCES ]
### Graphical Elements:
&gt; **Setting:** Indoor environments (garage, offices, house).  <br>
&gt; **Assets:** Furniture, Electronic devices, Household items, NPCs, Nature, Buildings, Cars.  

### SFX & VFX:
&nbsp; **SFX:**  <br>
   &nbsp;&nbsp; &gt; Dialogues: Typing sound.  <br>
   &nbsp;&nbsp; &gt; Environmental: Interactions with hackable devices:  <br>
     &nbsp;&nbsp; &nbsp;&nbsp; &gt; Electricity buzz  <br>
     &nbsp;&nbsp; &nbsp;&nbsp; &gt; Washing machine noise  <br>
     &nbsp;&nbsp; &nbsp;&nbsp; &gt; AC & Kitchen Hood sound  <br>
     &nbsp;&nbsp; &nbsp;&nbsp; &gt; Doors  <br>
     &nbsp;&nbsp; &nbsp;&nbsp; &gt; Printer  <br>
&nbsp; **VFX:**  <br>
  &nbsp;&nbsp; &gt; Electric Sparks  <br>
  &nbsp;&nbsp; &gt; Smoke  <br>
  &nbsp;&nbsp; &gt; Particles fireflies  <br>
  &nbsp;&nbsp; &gt; Music Notes  <br>
  &nbsp;&nbsp; &gt; Visual indicators for suspicion (?!!)  <br>
&nbsp; **Music:** IPod with 4 songs.  <br>
 
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
## [ MILESTONES ]
### Design phase - 2/11/2025
&nbsp; **Game concept**  <br>
&nbsp; **Game design document**  <br>

### Prototype - 7/12/2025
**Core gameplay systems:**  <br>
<table>
<tr><th>Feature</th><th>Owner</th><th>Date</th><th>Status</th></tr>

<tr><td> Movement </td><td> Daniel Bonardi </td><td> 4/11/25 </td><td> New </td></tr>
<tr><td> Interactables </td><td> Daniel Bonardi </td><td> 24/11/25 </td><td> New </td></tr>
<tr><td> Throwing system </td><td> Daniel Bonardi </td><td> 1/12/25 </td><td> New </td></tr>
<tr><td> Dialogue system </td><td> Daniel Bonardi </td><td> 7/12/25 </td><td> New </td></tr>

</table>

**AI & Enemies:**  <br>
<table>
<tr><th>Feature</th><th>Owner</th><th>Date</th><th>Status</th></tr>

<tr><td> CCTV enemy </td><td> Daniel Bonardi </td><td> 7/12/25 </td><td> New </td></tr>

</table>

**UI & Menus:**  <br>
<table>
<tr><th>Feature</th><th>Owner</th><th>Date</th><th>Status</th></tr>

<tr><td> Main menu </td><td> Emanuele Sapio </td><td> 7/12/25 </td><td> New </td></tr>
<tr><td> Pause menu </td><td> Emanuele Sapio </td><td> 22/11/25 </td><td> New </td></tr>
<tr><td> Game over menu </td><td> Emanuele Sapio </td><td> 28/11/25 </td><td> New </td></tr>
<tr><td> End level menu </td><td> Emanuele Sapio </td><td> 7/12/25 </td><td> New </td></tr>
<tr><td> Dialogue UI </td><td> Daniel Bonardi </td><td> 7/12/25 </td><td> New </td></tr>

</table>

**Minigame systems & Content:** <br>
<table>
<tr><th>Feature</th><th>Owner</th><th>Date</th><th>Status</th></tr>

<tr><td> Desktop minigame </td><td> Daniel Bonardi<br>Niccolo Sobrero </td><td> 18/11/25 </td><td> New </td></tr>
<tr><td> Password decryptor </td><td> Daniel Bonardi<br>Niccolo Sobrero </td><td> 18/11/25 </td><td> New </td></tr>
<tr><td> File download </td><td> Daniel Bonardi </td><td> 24/11/25 </td><td> New </td></tr>
<tr><td> Safe code </td><td> Daniel Bonardi<br>Niccolo Sobrero </td><td> 28/11/25 </td><td> New </td></tr>
<tr><td> Desktop minigame files </td><td> Niccolo Sobrero </td><td> 7/12/25 </td><td> New </td></tr>

</table>

**Levels & Progression system:** <br>
<table>
<tr><th>Feature</th><th>Owner</th><th>Date</th><th>Status</th></tr>

<tr><td> Tutorial level manager </td><td> Daniel Bonardi </td><td> 7/12/25 </td><td> New </td></tr>

</table>

**Art, Audio & Narrative content:** <br>
<table>
<tr><th>Feature</th><th>Owner</th><th>Date</th><th>Status</th></tr>

<tr><td> Main menu assets </td><td> Emanuele Sapio </td><td> 6/12/25 </td><td> New </td></tr>
<tr><td> Tutorial level asset </td><td> Matilde Sobberi </td><td> 5/12/25 </td><td> New </td></tr>
<tr><td> Hackable devices assets </td><td> Matilde Sobberi </td><td> 5/12/25 </td><td> New </td></tr>
<tr><td> Dialogue lines </td><td> Niccolo Sobrero </td><td> 7/12/25 </td><td> New </td></tr>

</table>

### Beta - 11/1/2026
&nbsp; **Core gameplay systems:**  <br>
<table>
<tr><th>Feature</th><th>Owner</th><th>Date</th><th>Status</th></tr>

<tr><td> Interactables </td><td> Daniel Bonardi </td><td> 8/1/26 </td><td> Expanded </td></tr>
<tr><td> Stunning enemies </td><td> Federico Tabellini<br>Daniel Bonardi </td><td> 10/1/26 </td><td> New </td></tr>
<tr><td> Dialogue system </td><td> Daniel Bonardi </td><td> 7/1/26 </td><td> Expanded </td></tr>
<tr><td> Objectives system </td><td> Daniel Bonardi </td><td> 10/1/26 </td><td> New </td></tr>

</table>

**AI & Enemies:**  <br>
<table>
<tr><th>Feature</th><th>Owner</th><th>Date</th><th>Status</th></tr>

<tr><td> Enemy patrolling </td><td> Federico Tabellini </td><td> 8/12/25 </td><td> New </td></tr>
<tr><td> Enemy path cycling </td><td> Federico Tabellini </td><td> 30/12/25 </td><td> New </td></tr>
<tr><td> Alert system </td><td> Federico Tabellini<br>Daniel Bonardi </td><td> 2/1/26 </td><td> New </td></tr>
<tr><td> Enemy FOV </td><td> Federico Tabellini<br>Daniel Bonardi </td><td> 9/1/26 </td><td> New </td></tr>
<tr><td> Enemies animations </td><td> Federico Tabellini </td><td> 11/1/26 </td><td> New </td></tr>

</table>

**UI & Menus:**  <br>
<table>
<tr><th>Feature</th><th>Owner</th><th>Date</th><th>Status</th></tr>

<tr><td> End level menu </td><td> Emanuele Sapio </td><td> 10/1/26 </td><td> Expanded </td></tr>
<tr><td> Dialogue UI </td><td> Daniel Bonardi </td><td> 7/1/26 </td><td> Expanded </td></tr>
<tr><td> Level selection </td><td> Daniel Bonardi </td><td> 7/1/26 </td><td> New </td></tr>
<tr><td> Main menu </td><td> Emanuele Sapio </td><td> 10/1/26 </td><td> Expanded </td></tr>
<tr><td> Game over menu </td><td> Emanuele Sapio </td><td> 10/1/26 </td><td> Expanded </td></tr>
<tr><td> Background music system </td><td> Daniel Bonardi </td><td> 11/1/26 </td><td> New </td></tr>
<tr><td> Objectives UI </td><td> Daniel Bonardi </td><td> 10/1/26 </td><td> New </td></tr>

</table>

**Minigame systems & Content:** <br>
<table>
<tr><th>Feature</th><th>Owner</th><th>Date</th><th>Status</th></tr>

<tr><td> Desktop minigame </td><td> Daniel Bonardi </td><td> 10/1/26 </td><td> Expanded </td></tr>
<tr><td> Desktop minigame files </td><td> Niccolo Sobrero </td><td> 9/1/26 </td><td> New </td></tr>
<tr><td> HR complaint </td><td> Daniel Bonardi </td><td> 10/1/26 </td><td> New </td></tr>

</table>

**Levels & Progression system:** <br>
<table>
<tr><th>Feature</th><th>Owner</th><th>Date</th><th>Status</th></tr>

<tr><td> Tutorial level manager </td><td> Daniel Bonardi </td><td> 11/1/26 </td><td> Expanded </td></tr>
<tr><td> HR level manager </td><td> Daniel Bonardi </td><td> 2/1/26 </td><td> New </td></tr>
<tr><td> Boss house level manager </td><td> Daniel Bonardi </td><td> 11/1/26 </td><td> New </td></tr>

</table>

**Art, Audio & Narrative content:** <br>
<table>
<tr><th>Feature</th><th>Owner</th><th>Date</th><th>Status</th></tr>

<tr><td> Player animations </td><td> Emanuele Sapio </td><td> 18/12/25 </td><td> New </td></tr>
<tr><td> HR offices asset </td><td> Matilde Sobberi </td><td> 27/12/25 </td><td> New </td></tr>
<tr><td> Boss house asset </td><td> Matilde Sobberi </td><td> 27/12/25 </td><td> New </td></tr>
<tr><td> Hackables VFX </td><td> Emanuele Sapio </td><td> 28/12/25 </td><td> New </td></tr>
<tr><td> Hackables SFX </td><td> Emanuele Sapio </td><td> 28/12/25 </td><td> New </td></tr>
<tr><td> Dialogue lines </td><td> Niccolo Sobrero </td><td> 9/1/26 </td><td> New </td></tr>

</table>

### Final submission - 24/2/2026


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
&gt; **DISCONNECTING...** <br>

</samp>
</blockquote>
