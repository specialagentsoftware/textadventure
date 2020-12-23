using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItems : MonoBehaviour
{
    [HideInInspector] public List<string> nounsInRoom = new List<string>();
    List<string> nounsInInventory = new List<string>();
    public Dictionary<string, string> examineDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> takeDictionary = new Dictionary<string, string>();
    GameController controller;
    public List<InteractableObject> usableItemList;
    Dictionary<string, ActionResponse> useDictionary = new Dictionary<string, ActionResponse>();

    void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public void RemoveItemsFromInventory()
    {
        nounsInInventory.Clear();
    }

    public string GetObjectsNotInInventory(Room currentRoom, int i)
    {
        InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i];
        if (!nounsInInventory.Contains(interactableInRoom.noun))
        {
            nounsInRoom.Add(interactableInRoom.noun);
            return interactableInRoom.description;
        }

        return null;
    }

    InteractableObject GetInteractableObjectFromUsableList(string noun)
    {
        for (int i = 0; i < usableItemList.Count; i++)
        {
            if (usableItemList[i].noun == noun)
            {
                return usableItemList[i];
            }

        }
        return null;
    }

    public void DisplayInventory()
    {
        if(nounsInInventory.Count > 0)
        {
            controller.LogStringWithReturn("You look inside your backpack, inside you find: ");

            for (int i = 0; i < nounsInInventory.Count; i++)
            {
                controller.LogStringWithReturn("<color=#96b0bc>** " + nounsInInventory[i] + "</color>");
            }
        }
        else
        {
            controller.LogStringWithReturn("There are no items currently in your backpack. Use TAKE to obtain items.");
        }
        
    }

    public void ClearCollections()
    {
        examineDictionary.Clear();
        takeDictionary.Clear();
        nounsInRoom.Clear();
    }

    public void AddActionResponsesToUseDictionary()
    {
        for (int i = 0; i < nounsInInventory.Count; i++)
        {
            string noun = nounsInInventory[i];
            
            InteractableObject interactableObjectInInventory = GetInteractableObjectFromUsableList(noun);
            
            if(interactableObjectInInventory == null)
            {
                continue;
            }
            for (int j = 0; j < interactableObjectInInventory.interactions.Length; j++)
            {
                Interaction interaction = interactableObjectInInventory.interactions[j];
                if(interaction.actionResponse == null)
                {
                    continue;
                }
                if (!useDictionary.ContainsKey(noun))
                {
                    useDictionary.Add(noun, interaction.actionResponse);
                }
            }
        }
    }

    public Dictionary<string,string> Take (string[] separatedInputWords)
    {
        if(separatedInputWords.Length > 1 && separatedInputWords[1] != string.Empty)
        {
            string noun = separatedInputWords[1];
            if (nounsInRoom.Contains(noun))
            {
                nounsInInventory.Add(noun);
                AddActionResponsesToUseDictionary();
                nounsInRoom.Remove(noun);
                return takeDictionary;
            }
            else
            {
                controller.LogStringWithReturn("There is no " + noun + " Here to take.");
                return null;
            }
        }
        return null;
    }

    public void UseItem(string[] separatedInputWords)
    {
        if (separatedInputWords.Length > 1 && separatedInputWords[1] != string.Empty)
        {
            string nounToUse = separatedInputWords[1];
            InteractableObject objectbeingused = GetInteractableObjectFromUsableList(nounToUse);

            if (nounsInInventory.Contains(nounToUse))
            {
                if (useDictionary.ContainsKey(nounToUse))
                {
                    if(objectbeingused.isUsed == false)
                    {
                        bool actionResult = useDictionary[nounToUse].DoActionResponse(controller);
                        if (!actionResult)
                        {
                            controller.LogStringWithReturn("HMM. Nothing Happens.");
                        }
                        else
                        {
                            objectbeingused.isUsed = true;
                        }
                    }
                    else
                    {
                        controller.LogStringWithReturn(nounToUse + " has already been used");
                    } 
                }
                else
                {
                    controller.LogStringWithReturn("You can't use the " + nounToUse);
                }
            }
            else
            {
                controller.LogStringWithReturn("There is no " + nounToUse + " in your inventory");
            }
        }
    }
}
