Vanishing Spaces

Created for CS 426 Spring 2026 at UIC

Design Document link: https://docs.google.com/document/d/1L-gJC_7u1B60RrOgDq0BcuWIfCJ1m-tXZyQgh-zSz-s/edit?usp=sharing

Link to Game: https://mphilip.itch.io/vanishing-spaces 

This game is a first person psychological horror game. An explorer enters an abandoned mansion and gets trapped as the building shifts around them. The mansion rearranges itself whenever the areas are out of sight of the player. The player must find "anchor objects". These objects continue the storyline while locking the rooms and the objects in them in place. All the while the player is haunted by the previous explorers who failed to escape.

------------Assignment Details------------

Assignment 6:

3D physics: Gives the game a real feeling by letting the statue be able to come into contact with you
Added the ability for the Statue to push the player which eventually will be worked into you getting attacked by it. Did so using colliders
Added a Rigidbody component to the flashlight, allowing it to react to gravity and physics forces when dropped in the scene.
Friction adjustments allow for a more realistic feel as the player explores the level. 

Lights: All of them add to the ambience with the flashing lights and keeping it dark and spooky
Starting Room lights
Flashlights
Ceiling lights
Added a Point Light in the tutorial room to illuminate the environment and create atmosphere.

Textures: All of these help to give the rooms shape and keep the broken down mansion experience
Walls, Floors, Roof
Cabinets
Dining room set
Applied texture to the flashlight model and wallpaper texture to the walls of the tutorial room.

AI Techniques:
Pathfinding in the statue to let it follow the player. Used navmesh + navagent to map out where it could move
Monster AI using a Finite State Machine with three states:
- Patrol: Monster moves between patrol points using NavMesh pathfinding.
- Jumpscare: When player is detected, monster teleports in front of player and plays scream sound.
- Disappear: After the jumpscare, monster disappears after a short delay.
Room swapping uses a Finite State Machine to control which room the player goes to when swapping

Mecanim:
Statue walking model so that the player can see it walking if it isn't in the flashlight beam
Player Animator Controller with Idle, Walking, and Running states driven by movement speed parameter. (First person view)
Ghost Monster Animator Controller handling its movement and idle animations.
NPC Butler has an idle animation in the dining room.


Assignment 7:

Matthew:I edited the game by adding more rooms and more detail work to each of them.

Sound Design: I also added movement to the statues where the movement stops the frame when the flashlight is on it. As the statue steps it creates a stomping sound effect and when it walks it makes a whispering sound effect. Both effects stop when the user points the flashlight on them. Along with this when the user points the flashlight on the statue it makes a jumpscare sound effect for the statue getting closer. For the start button I also added a feature that when pressed it makes a jumpscare sound.

UI Design: Along with this for UI design I increased the size of the elements on the starting screen to fit it bettter. I also added features to the buttons where if they are hovered over they turn red and then turn white when pressed. Along with this I added a control button on the start screne that shows the user the controls before they start. After this the title and buttons all fade and then the camera begins to move down the hallway. As it goes down the hallway when it gets to the doors they both open and the screen fades to black. The buttons make the user have an easier time navigating the game giving an easier way to look at it and then also knowing the controls before they start. The camera pan works to help the player feel more immersed in the game so they don't feel like it just randomly starts.


Bhavani

Sound design: Picture place sound: The sound made when the player places a picture in a slot. It is a soft thud, like putting a light object on wood; Ambient Sounds which give the player a feeling of tension. Also door creak sounds in all rooms but the tutorial room.

UI design: Scroll open had no background so it was hard to see; added background so that the text could be seen clearly. When walking up to the door it would only tell you that you need a key in the split second you press E; modified so you need a key is the default and only shows e to open after unlocking. 

James

Sound Design: I added a chest opening sound effect that plays when the player successfully enters the correct code into the chest puzzle. I also added a door opening sound effect that plays when the player uses the key to unlock the exit door. Both sounds were implemented by adding AudioSource components to their respective GameObjects and triggering them in code at the right moment.

UI Design: I fixed a critical bug where the inventory UI was showing on the start screen by adding Hide() and Show() methods to the InventoryManager. I also fixed the CodeBoxUI not appearing when the chest puzzle was opened by creating a separate Canvas for it, preventing it from being destroyed by DontDestroyOnLoad. I implemented the scroll note UI panel that opens when the player interacts with the scroll, displaying the code hint text clearly to the player.

Puzzles: I implemented the chest puzzle system where the player must enter a correct 4-digit code to receive the key item. I also implemented the door lock system where the player must have the key in their inventory to unlock the exit door and progress to the next room. Additionally I created the scroll note system that allows the player to read in-game notes by pressing E near the scroll.


Assignment 8:

James

- Implemented a **Rim Lighting shader** (`RimLighting.shader`) using URP HLSL
- Applied rim lighting materials to objects in the game:
  - `PictureLightMaterial` - applied to picture frame
  - `ScrollLightMaterial` - applied to scroll item

Matthew

- Implemented a simple custom shader (`SimpleStatueShader.shader`) for statue enemies
- Designed a subtle edge glow effect to enhance visibility and create a horror aesthetic
- Created and tuned a material (`StatueMaterial`) with adjustable color and glow strength
- Applied the material to statue enemy models across scenes

  Bhavani
- Implemented a simple fog shader which obscures items the further away you go from them
- Added to a material (`Fog`) with adjustable fog distance and color
- Applied to a picture frame so the player cannot see it until they get close, forcing them to move around

Feedback from Alpha Release and Response

Control guide is not large enough
- Players complained they were unable to read the text on the control guide because it was too small
- Solution: The tutorial screen was made bigger so that players can read it

Make interactable items more obvious
- Interactable items looked the same as non-interactable items
- Solution: Certain key items now appear highlighted to distinguish them

Make sure that players are not trapped in certain rooms
- One of the scenes could not be exited (Bug)
- The bug has been fixed. Players can now move freely between all rooms.

Make users have to interact with every part of a set of motions
- Users got confused when opening a chest when the item went straight into their inventory.
- Solution: Make users pick up the item from the chest after they have unlocked it to make it feel like they get to see inside the chest and grab what they find.

Make UI larger and easier to read
- Users had a hard time reading the control guide, instructions, or seeing their inventory
- Solution: Increase the size of all of these things to be easier to read

Make flashlight a part of base mobility rather than an item
- Users were confused that in control screen it said press F to turn on flashlight but they had to have the item selected to use the flashlight
- Solution: Make the flashlight a part of the base movement and remove it from the inventory.

Make items in inventory easier to know on first glance what it is
- User had a hard time discerning what items were in their inventory so they didn’t know what was in each slot
- Solution: Adjusted item images to be more clear and more easily discernable

Sensitivity on track pad should be higher
- Users had a hard time turning the screen to look around or keep the statue at bay
- Solution: Make mouse sensitivity higher so that users could look around more freely

Get rid of redundant items
- Users found that there were multiple places to pick up flashlights but no use to having more than one
- Solution: remove the extra flashlights and implement a battery life system for the flashlight

Make the monster faster / Make statue scarier 
- Players said that the monster was too slow for it to be a real threat
- Solution: The monster’s speed was increased and it jumpscares the player if they are caught

Add monster in all rooms
- The player commented that after having the monster chasing them in one scene, it felt odd to have the monster not in the next room; it lowered the tension
- Solution: Monsters were added into more rooms to make the game more difficult

Make sure object movement is intentional
- Users found a couch that had a rigid body and a collider where it had no reason to move
- Solution: Removed these to make sure that object movement is only intentional.


Beta Release - Changes Made
- Made controls and prompts larger 
- Fixed room swapping to ensure that players are able to move between rooms 
- Added statues to multiple rooms so that the challenging aspect remains throughout 
- Increased the speed of the statues so that there is more fear involved
- Made UI more user friendly by making it more legible
- Added higher sensitivity to make the game easier to maneuver
- Added flashlight as part of games base functionality 
- Added better icons for inventory
- Added several more rooms to improve experience 
- Added puzzles increase difficulty of game
- Removed unnecessary object movement
- Made some rooms larger to let users feel less cramped 
- Added shader to statues to make them appear like ghosts
