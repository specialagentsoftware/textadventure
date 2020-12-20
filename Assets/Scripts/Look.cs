using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/InputActions/Look")]
public class Look : InputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        controller.DisplayRoomText();
    }

    public override bool ValidateInputBeforeAction(GameController controller, string[] separatedInputWords)
    {
        if (separatedInputWords.Length > 1)
        {
            if (separatedInputWords[1] != string.Empty)
            {
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
