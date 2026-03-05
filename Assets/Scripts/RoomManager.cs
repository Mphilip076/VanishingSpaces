using UnityEngine;

/*  RoomManager is responsible for creating and managing the rooms in the game. 
    It initializes the array of all rooms and creates the rooms at the start of the game. 
    The rooms can be accessed from anywhere in the code using Room.allRooms[index], where index is the index of the room in the array.

    Please read the comments in the Start function for more information on how to create the rooms and set their exits.
*/
public class RoomManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*  IMPORTANT! PLEASE READ THIS BEFORE ADDING THE ROOMS:
                Scenes can be created in the Unity editor and then loaded using SceneManager.LoadScene("SceneName"). 
                Make sure to add the scenes to the build settings in Unity, otherwise it won't work.
            
                Rooms will automatically be added to the array of all rooms when they are created, 
                    so there is no need to add them manually to the array. 

                The rooms can be accessed from anywhere in the code using Room.allRooms[index], where index is the index of the room in the array.
        */

        /* Create the rooms here. Example:

            Room room1 = new Room("Room 1", SceneManager.GetSceneByName("Room1Scene"));
            Room room2 = new Room("Room 2", SceneManager.GetSceneByName("Room2Scene"));
            Room room3 = new Room("Room 3", SceneManager.GetSceneByName("Room3Scene"));

            room1.SetExit1(room2);
            room1.SetExit2(room3);
            // room 1 exit 3 is not used, so it doesn't need to be set.
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
