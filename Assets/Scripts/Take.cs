using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/InputActions/Take")]
public class Take : InputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        Dictionary<string, string> takeDictionary = controller.interactableItems.Take(separatedInputWords);

        if (ValidateInputBeforeAction(controller, separatedInputWords))
        {
            if (takeDictionary != null)
            {
                controller.LogStringWithReturn(controller.TestVerbDictionaryWithNoun(takeDictionary, separatedInputWords[0], separatedInputWords[1]));
            }
        }
        else
        {
            controller.LogStringWithReturn("<color=#CD5C5C>Take command must be followed by an object</color>");
        }

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
