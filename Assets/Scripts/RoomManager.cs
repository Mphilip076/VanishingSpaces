using UnityEngine;
using UnityEngine.SceneManagement;

/*  RoomManager is responsible for creating and managing the rooms in the game. 
    It initializes the array of all rooms and creates the rooms at the start of the game. 
    The rooms can be accessed from anywhere in the code using Room.allRooms[index], where index is the index of the room in the array.

    Please read the comments in the Start function for more information on how to create the rooms and set their exits.
*/
public class RoomManager : MonoBehaviour
{
    static bool hasStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*  IMPORTANT! PLEASE READ THIS BEFORE ADDING THE ROOMS:
                Scenes can be created in the Unity editor and then loaded using the SceneManager. 
                Make sure to add the scenes to the build settings in Unity, otherwise it won't work.
            
                Rooms will automatically be added to the array of all rooms when they are created, 
                    so there is no need to add them manually to the array. 

                The rooms can be accessed from anywhere in the code using 
                    Room.allRooms[index] where index is the index of the room in the array
                    or Room.GetRoomByName(string name)
        */

        // Make sure this runs once across all instances
        if (!hasStarted)
        {
            Debug.Log("[RoomManager.cs] Adding rooms");

            // Add all the rooms here:
            Room t = new Room("Tutorial");
            Room dr = new Room("DiningRoom");
            Room lr = new Room("LivingRoom");

            // Make rooms accessible
            dr.AllowRandomEntry();
            lr.AllowRandomEntry();
            t.AllowRandomEntry();

            // Set exits
            t.SetExit1(dr);
            lr.SetExit1(dr);
            dr.SetExit1(t);

            Debug.Log("[RoomManager.cs] Room list size " + Room.allRooms.Count);

            hasStarted = true;
            Debug.Log("[RoomManager.cs] Finished loading rooms");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.G)) {
            Room.SetScene("Tutorial");
        }
    }
}
