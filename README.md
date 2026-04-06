Assignment 6:

3D physics: Gives the game a real feeling by letting the statue be able to come into contact with you
Added the ability for the Statue to push the player which eventually will be worked into you getting attacked by it. Did so using colliders
Added a Rigidbody component to the flashlight, allowing it to react to gravity and physics forces when dropped in the scene.

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
