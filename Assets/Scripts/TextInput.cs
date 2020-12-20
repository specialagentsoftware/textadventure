using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TextInput : MonoBehaviour
{
    GameController controller;
    public InputField inputField;

    void Awake()
    {
        controller = GetComponent<GameController>();
        inputField.onEndEdit.AddListener(AcceptStringInput);
    }

    void Start()
    {
        inputField.ActivateInputField();
    }

    void AcceptStringInput(string userInput)
    {
        userInput = userInput.ToLower();

        if(userInput == string.Empty)
        {
            InputComplete();
            return;
        }

        bool isInput = true;

        controller.LogStringWithReturn(userInput,isInput);

        char[] delimiterCharacters = { ' ' };
        string[] separatedInputWords = userInput.Split(delimiterCharacters);

        if (controller.IsValidCommand(separatedInputWords[0]))
        {
            for (int i = 0; i < controller.inputActions.Length; i++)
            {
                InputAction inputAction = controller.inputActions[i];
                if (inputAction.keyWord == separatedInputWords[0])
                {
                    inputAction.RespondToInput(controller, separatedInputWords);
                }
            }
        } 
        else
        {
            controller.LogStringWithReturn("<color=#CD5C5C>You have entered an unknown command. You can see a list of commands by typing </color><color=white>help</color>");
        }

        InputComplete();
    }

    void InputComplete()
    {
        controller.DisplayLoggedText();
        inputField.ActivateInputField();
        inputField.text = null;
    }
}
