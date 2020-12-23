using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/ActionResponses/WinSequence")]
public class WinSequence : ActionResponse
{
    public Room roomToChangeTo;
    public string requiredDescription;

    public override bool DoActionResponse(GameController controller)
    {
            controller.roomNavigation.currentRoom = roomToChangeTo;
            controller.roomNavigation.currentRoom.description = requiredDescription;
            controller.DisplayRoomText();
            return true;
    }
}
