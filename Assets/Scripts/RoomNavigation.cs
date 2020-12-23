using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNavigation : MonoBehaviour
{
    public Room currentRoom;
    GameController controller;
    Dictionary<string, Room> exitDictionary = new Dictionary<string, Room>();
    public List<Room> roomList = new List<Room>();

    void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public void UnpackExitsInRoom()
    {
        for (int i = 0; i < currentRoom.exits.Length; i++)
        {
            exitDictionary.Add(currentRoom.exits[i].keyString, currentRoom.exits[i].valueRoom);
            controller.interactionDescriptionsInRoom.Add(currentRoom.exits[i].exitDescription);
        }
    }

    public void AttemptToChangeRooms(string directionNoun)
    {
        if(currentRoom.roomName.ToString() == "rock")
        {
            controller.AddRoomVisit();
        }

        if (exitDictionary.ContainsKey(directionNoun))
        {
            currentRoom = exitDictionary[directionNoun];
            controller.DisplayRoomText();
            controller.AddRoomVisit();
        } else 
        {
            controller.LogStringWithReturn("There is no path to the " + directionNoun);
        }
    }

    public void ClearExits()
    {
        exitDictionary.Clear();
    }

    public Room GetRoomByRoomName(string Name)
    {
        foreach (Room rm in roomList)
        {
            if(rm.roomName.ToString() == Name)
            {
                return rm;
            }
        }
        return null;
    }

    public void ChangeToRoomNoExitRequired(Room roomToChangeTo)
    {
        currentRoom = roomToChangeTo;
        controller.DisplayRoomText();
    }
}
