using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/InputActions/Go")]
public class Go : InputAction
{

    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        if(ValidateInputBeforeAction(controller,separatedInputWords))
        {
            if (controller.roomNavigation.IsMoveInterrupted())
            {
                controller.roomNavigation.PresentCombat(controller,separatedInputWords);
            }
            else
            {
                controller.roomNavigation.AttemptToChangeRooms(separatedInputWords[1]);
            }
        } 
        else
        {
            controller.LogStringWithReturn("<color=#CD5C5C>Go command must be followed by an object</color>");
        }
    }

    public override bool ValidateInputBeforeAction(GameController controller, string[] separatedInputWords)
    {
        if(separatedInputWords.Length > 1)
        {
            if (separatedInputWords[1] != string.Empty){
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
