using UnityEngine;
using UnityEngine.SceneManagement;

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

                The rooms can be accessed from anywhere in the code using 
                    Room.allRooms[index] where index is the index of the room in the array
                    or Room.GetRoomByName(string name)
        */

        // Add all the rooms here:
        new Room("Tutorial", SceneManager.GetSceneByName("Tutorial"));
        new Room("Dining Room", SceneManager.GetSceneByName("DiningRoom"));
        new Room("Master Bedroom", SceneManager.GetSceneByName("MasterBedroom"));

        /* TODO: Modify room parameters:

            Room.GetRoom("Master Bedroom").AllowRandomEntry(); 
            // The user can enter this room randomly
            // Says nothing about the exits
            // Each room should be added individually when the entry requirements are met
            // *calling LockInPlace will remove randomness

            Room.GetRoom("Dining Room").LockInPlace(); 
            // The user found the anchor item!!
            // This room's exits will not be random anymore
            // *calling AllowRandomEntry will cause the exits to be random again

            Room.GetRoom("Tutorial").LockRoom(); 
            // The user cannot exit the room until .UnlockRoom() is called
            // (presumably after the user finds the key or completes a puzzle)
        */


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
