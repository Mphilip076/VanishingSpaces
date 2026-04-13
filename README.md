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
