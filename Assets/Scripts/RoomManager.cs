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
            Room start = new Room("StartScene");
            Room tutorial = new Room("Tutorial");
            Room diningRoom = new Room("DiningRoom");
            Room livingRoom = new Room("LivingRoom");
            Room library = new Room("Library");
            Room masterBedroom = new Room("MasterBedroom");
            Room bathroom = new Room("Bathroom");

            /* Set exits BOTH WAYS
                dining -> living
                dining -> bedroom
                dining -> bathroom
                dining -> tutorial

                living -> dining
                living -> library            
            */

            // Tutorial Room
            tutorial.SetExit1(diningRoom);
            // Dining Room
            diningRoom.SetExit1(livingRoom);
            diningRoom.SetExit2(masterBedroom);
            diningRoom.SetExit3(bathroom);
            diningRoom.SetExit4(tutorial);
            // Living Room
            livingRoom.SetExit1(diningRoom);
            livingRoom.SetExit2(library);
            // Library
            library.SetExit1(livingRoom);
            // Master Bedroom
            masterBedroom.SetExit1(diningRoom);
            // Bathroom
            bathroom.SetExit1(diningRoom);

            // Start scene will always exit to tutorial
            // After that it is inaccessible to the user
            //      because it isn't linked from the accessible rooms
            start.SetExit1(tutorial);
            start.LockInPlace();
            
            Debug.Log("[RoomManager.cs] Room list size " + Room.allRooms.Count);

            Room.SetScene("StartScene");
            hasStarted = true;
            Debug.Log("[RoomManager.cs] Finished loading rooms");
        }

        Room.ResetRooms();

    }

}
