using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomNavigation : MonoBehaviour
{
    public Room currentRoom;
    GameController controller;
    Dictionary<string, Room> exitDictionary = new Dictionary<string, Room>();
    public List<Room> roomList = new List<Room>();
    public List<string> enemies;

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

    public bool IsMoveInterrupted()
    {
            var rand = Random.Range(1,100);
            return rand < 18;
    }

    public bool IsSurvivable()
    {
        var rand = Random.Range(1, 100);
        return rand < 68;
    }

    public void PresentCombat(GameController controller,string[] separatedInputWords)
    {
        controller.LogStringWithReturn("<color=red>While moving from " + controller.roomNavigation.currentRoom.name + " to " + separatedInputWords[1] + " you were attacked by " + enemies[Random.Range(0,enemies.Count)] + "</color>");

        if (IsSurvivable())
        {
            controller.LogStringWithReturn("<color=white>You survived, and can continue on to the " + separatedInputWords[1] + "</Color>");
            AttemptToChangeRooms(separatedInputWords[1]);
        }
        else
        {
            Room loseRoom = GetRoomByRoomName("loss");
            loseRoom.description = "You died in combat.\n You may restart the game by typing <color=yellow> reset </color> or you may quit by typing <color=red>quit</color>";
            ChangeToRoomNoExitRequired(loseRoom);
        }
    }


}
