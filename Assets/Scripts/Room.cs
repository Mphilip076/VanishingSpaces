using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


/*  Room represents a room in the game. It has a name, a scene, and up to three exits to other rooms.

    The list of all rooms in the game is stored in the static array Room.allRooms, which should be initialized in the RoomManager class.
    They can be accessed from anywhere in the code using Room.allRooms[index], where index is the index of the room in the array.

    The exits are represented as pointers to other Room objects.
    If an exit is not set, that exit will be treated as non-existent.

    The rooms can be locked or unlocked. If a room is locked, any entrances to that room will always lead to that room instead of a random room, 
        and any exits from that room will always lead to the right exit instead of a random exit.
    If the room and its exit are unlocked, then the exits will lead to a random exit which is not locked.
    If for some reason, you need to get a specific exit without following the rules for determining which exit to return, 
        you can use the GetExit(int exitNum) function, where exitNum is 1, 2, or 3.

    To lock a room, use the Lock() function. 
    If for some reason you need to unlock a room, use the Unlock() function.
*/
public class Room
{
    // Static variables
    public static List<Room> allRooms
                        = new List<Room>();     // An list of all the rooms in the game.
    private static List<Room> unlockedRooms 
                        = new List<Room>();     // A list of all the unlocked rooms in the game, 
                                                // used for random access to unlocked rooms.

    // Instance variables
    private string name;    // For debugging purposes
    private Scene scene;    // The scene that represents this room. 
                            // This is where the actual game objects and environment of the room are located.

    private bool lockedInPlace; // Whether the room is locked or not
                                // If true, any entrances to that room will always lead to that room
                                // instead of a random room, and any exits will always lead to 
                                // the set exits
    private bool roomIsLocked;  // Whether the user has to complete some kind of puzzle 
                                // to unlock the door or needs some item, such as a key
                                // Default false
    

    // The exits to other rooms. All three are initially null
    // The rooms can be set using the SetExit1, SetExit2, and SetExit3 functions.
    private Room exit1;  
    private Room exit2;
    private Room exit3;

    public Room(string name, Scene scene)
    {
        this.name = name;
        this.scene = scene;
        this.roomIsLocked = false;

        // By default, the room is unlocked and has no exits
        this.lockedInPlace = false;
        exit1 = null;
        exit2 = null;
        exit3 = null;

        // Update the lists
        allRooms.Add(this);
    }

    
    // ------------------------------------ GETTERS ------------------------------------

    /// <returns>
    /// Returns the scene 
    /// </returns>
    public Scene GetScene()
    {
        return scene;
    }

    /// <returns>
    // Returns the name of the room
    /// </returns>
    public string Name()
    {
        return name;
    }
    
    /// <summary>
    /// Returns whether the door's exits are random
    /// </summary>
    /// <returns>
    /// Returns true if the exits will always lead to the exit,
    /// false if the exit is randomized
    /// </returns>
    public bool IsLockedInPlace()
    {
        return lockedInPlace;
    }

    /// <summary>
    /// Return whether the door is locked
    /// </summary>
    /// <returns>
    /// Returns true if the user cannot exit,
    /// false if they can
    /// </returns>
    public bool DoorIsLocked()
    {
        return roomIsLocked;
    }

    // Find a room by name
    // Returns null if not found
    public static Room GetRoom(string roomName)
    {
        foreach(Room r in allRooms)
        {
            if(r.name.equals(roomName)) return r;
        }

        // Doesn't exist
        return null;
    }

    // ------------------------------------ SETTERS ------------------------------------

    // Locks the room in place; no more randomness
    public void LockInPlace()
    {
        // If the room was unlocked, remove it from the list of unlocked rooms
        if (!lockedInPlace) unlockedRooms.Remove(this);

        lockedInPlace = true;
    }

    // Adds the room to the list of randomly selected rooms to enter
    public void AllowRandomEntry()
    {
        // If the room was locked, add it back to the list of unlocked rooms
        if(lockedInPlace) unlockedRooms.Add(this);
        
        lockedInPlace = false;
    }

    /* ------------------------------------ EXIT SETTERS ------------------------------------ 
        The exits can be set using the SetExit1, SetExit2, and SetExit3 functions.
        If an exit is not set, that exit will be treated as non-existent.
        All exits should be set before the game starts, and they should not be changed during the game.    
       -------------------------------------------------------------------------------------- */

    // Sets the first exit
    public void SetExit1(Room exit)
    {
        exit1 = exit;
    }

    // Sets the second exit
    public void SetExit2(Room exit)
    {
        exit2 = exit;
    }

    // Sets the third exit
    public void SetExit3(Room exit)
    {
        exit3 = exit;
    }

    /* ------------------------------------ EXIT GETTERS ------------------------------------ 
        The exits can be accessed using the GetExit1, GetExit2, and GetExit3 functions.
        
        If the exit is not set, return null
        If the door is locked because the user has not completed a puzzle, return null
        If the room is locked, then always return the right exit
        If the room you are exiting to is locked, then always return the right exit
        Otherwise, return a random exit which is not locked 
       -------------------------------------------------------------------------------------- */
    
    // Pervent the user from taking any exit because the door is locked
    // The user needs to perform some action to unlock it
    public void LockRoom()
    {
        roomIsLocked = true;
    }

    // Unlock the room so that the player can exit
    public void UnlockRoom()
    {
        roomIsLocked = false;
    }

    // For debugging: returns the exit corresponding to the exit number (1, 2, or 3)
    // Do not use this function in the actual game, since it does not follow the rules for determining which exit to return.
    public Room GetExitOverride(int exitNum)
    {
        if (exitNum == 1) return exit1;
        else if (exitNum == 2) return exit2;
        else if (exitNum == 3) return exit3;
        else return null; // Invalid exit number
    }

    // Returns a random exit which is not locked. 
    // If there are no unlocked rooms, return null.
    private Room GetRandomExit()
    {
        if (unlockedRooms.Count == 0){
            Debug.Log("Room " + name + "'s exit 3 cannot be used because it does not exist.");
            return null;
        }
        if (roomIsLocked)
        {
            Debug.Log("Room: Cannot get a random exit because no possible exits exist.");
            return null;
        }
        return unlockedRooms[Random.Range(0, unlockedRooms.Count)];
    }

    // Use the first exit
    // See notes on exit getters for more information on how the exit is determined
    public Room GetExit1()
    {
        // If the exit is not set, return null
        if(exit1 == null) {
            Debug.Log("Room " + name + "'s exit 1 cannot be used because it does not exist.");
            return null;
        }

        // If the door is locked, you can't exit
        if (roomIsLocked)
        {
            Debug.Log("Room "  + name + "'s exit cannot be used because the door is locked.");
            return null;
        }

        // If this room is locked, then always return the right exit
        if (lockedInPlace) {
            return exit1;
        }

        // If the room you are exiting to is locked, then always return the right exit
        if (exit1.lockedInPlace) {
            return exit1;
        }

        // None of the conditions for returning the right exit are met, so return a random exit which is not locked
        return GetRandomExit();
    }


    // Use the second exit
    // See notes on exit getters for more information on how the exit is determined
    public Room GetExit2()
    {
        // If the exit is not set, return null
        if(exit2 == null) {
            Debug.Log("Room " + name + "'s exit 2 cannot be used because it does not exist.");
            return null;
        }

        // If the door is locked, you can't exit
        if (roomIsLocked)
        {
            Debug.Log("Room "  + name + "'s exit cannot be used because the door is locked.");
            return null;
        }

        // If this room is locked, then always return the right exit
        if (lockedInPlace) {
            return exit2;
        }

        // If the room you are exiting to is locked, then always return the right exit
        if (exit2.lockedInPlace) {
            return exit2;
        }

        // None of the conditions for returning the right exit are met, so return a random exit which is not locked
        return GetRandomExit();
    }

    // Use the third exit
    // See notes on exit getters for more information on how the exit is determined
    public Room GetExit3()
    {
        // If the exit is not set, return null
        if(exit3 == null) {
            Debug.Log("Room " + name + "'s exit 3 cannot be used because it does not exist.");
            return null;
        }

        // If the door is locked, you can't exit
        if (roomIsLocked)
        {
            Debug.Log("Room "  + name + "'s exit cannot be used because the door is locked.");
            return null;
        }

        // If this room is locked, then always return the right exit
        if (lockedInPlace) {
            return exit3;
        }

        // If the room you are exiting to is locked, then always return the right exit
        if (exit3.lockedInPlace) {
            return exit3;
        }

        // None of the conditions for returning the right exit are met, so return a random exit which is not locked
        return GetRandomExit();
    }
}