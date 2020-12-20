using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/InputActions/Inventory")]
public class Inventory : InputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        controller.interactableItems.DisplayInventory();
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
