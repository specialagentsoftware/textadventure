using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [HideInInspector] public RoomNavigation roomNavigation;
    [HideInInspector] public List<string> interactionDescriptionsInRoom = new List<string>();
    public InputAction[] inputActions;
    [HideInInspector] public InteractableItems interactableItems;
    [HideInInspector] public List<string> roomsVisited;
    public Dictionary<string, Room> roomDictionary = new Dictionary<string, Room>();

    List<string> actionLog = new List<string>();
    [TextArea]
    public Text displayText;
    public Text LocationValue;

    void Awake()
    {
        interactableItems = GetComponent<InteractableItems>();
        roomNavigation = GetComponent<RoomNavigation>();
    }

    void Start()
    {
        DisplayRoomText();
        DisplayLoggedText();
    }

    public void DisplayLoggedText()
    {
        LocationValue.text = roomNavigation.currentRoom.roomName.ToString();
        string logAsText = string.Join("\n", actionLog.ToArray());
        displayText.text = logAsText;
    }

    public void LogStringWithReturn(string stringToAdd,bool isInput = false)
    {
        if (isInput)
        {
            actionLog.Add("<color=white>--  " + stringToAdd + "  --</color>\n");
        }
        else
        {
            actionLog.Add(stringToAdd + "\n");
        }
        
    }

    void UnpackRoom()
    {
        roomNavigation.UnpackExitsInRoom();
        PrepareObjectsToTakeOrExamine(roomNavigation.currentRoom);
    }

    void ClearCollectionsforNewRoom()
    {
        interactableItems.ClearCollections();
        interactionDescriptionsInRoom.Clear();
        roomNavigation.ClearExits();
    }

    void PrepareObjectsToTakeOrExamine(Room currentRoom)
    {
        for (int i = 0; i < currentRoom.interactableObjectsInRoom.Length; i++)
        {
            string descriptionNotInInventory = interactableItems.GetObjectsNotInInventory(currentRoom, i);
            if(descriptionNotInInventory != null)
            {
                interactionDescriptionsInRoom.Add(descriptionNotInInventory);
            }

            InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i];
            for (int j = 0; j < interactableInRoom.interactions.Length; j++)
            {
                Interaction interaction = interactableInRoom.interactions[j];
                if(interaction.inputAction.keyWord == "examine")
                {
                    interactableItems.examineDictionary.Add(interactableInRoom.noun, interaction.textResponse);
                }
                
                if (interaction.inputAction.keyWord == "take")
                {
                    interactableItems.takeDictionary.Add(interactableInRoom.noun, interaction.textResponse);
                }
            }
        }
    }

    public string TestVerbDictionaryWithNoun(Dictionary<string,string> verbDictionary, string verb, string noun)
    {
        if (verbDictionary.ContainsKey(noun))
        {
            return verbDictionary[noun];
        }

        return "You can't " + verb + " " + noun;
    }

    public void DisplayRoomText()
    {
        ClearCollectionsforNewRoom();
        UnpackRoom();
        string joinedInteractionDescriptions = string.Join("\n\n", interactionDescriptionsInRoom.ToArray());
        string specialFirstVisitText = roomNavigation.currentRoom.firstVisitText;
        string combinedText;

        if (!roomsVisited.Contains(roomNavigation.currentRoom.roomName.ToString())) 
        {
            combinedText = specialFirstVisitText +"\n\n" + roomNavigation.currentRoom.description + "\n\n" + joinedInteractionDescriptions;
        }
        else
        {
            combinedText = roomNavigation.currentRoom.description + "\n\n" + joinedInteractionDescriptions;
        }
        
        LogStringWithReturn(combinedText);
    }

    public bool IsValidCommand(string keyWord)
    {
        List <string> commands = new List<string>();
        
        foreach (var A in inputActions)
        {
            commands.Add(A.keyWord.ToString());
        }

        if (commands.Contains(keyWord))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DisplayHelp()
    {
        LogStringWithReturn("Commands you can use are listed below");

        Dictionary<string,string> commands = new Dictionary<string, string>();
        
        foreach (var A in inputActions)
        {
            commands.Add(A.keyWord.ToString(), A.description.ToString());
        }
        
        foreach (var cmd in commands)
        {
            LogStringWithReturn("<color=#96b0bc>" + cmd.Value + "</color>");
        }
    }

    public void PromptForExit()
    {
        LogStringWithReturn("<color=#CD5C5C>Exiting in 5 seconds...</color>");
        StartCoroutine(WaitToQuit());

    }

    IEnumerator WaitToQuit()
    {
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }

    public void AddRoomVisit()
    {
        if (!roomsVisited.Contains(roomNavigation.currentRoom.roomName.ToString()))
        {
            roomsVisited.Add(roomNavigation.currentRoom.roomName.ToString());
        }
    }

    public void Reset()
    {
        Room roomToChangeTo = roomNavigation.GetRoomByRoomName("rock");
        if(roomToChangeTo != null)
        {
            roomsVisited.Clear();
            actionLog.Clear();
            MarkUsableItemsUnused();
            LogStringWithReturn("<color=#CD5C5C>Restarting</color>");
            interactableItems.RemoveItemsFromInventory();
            roomNavigation.ChangeToRoomNoExitRequired(roomToChangeTo);
        }
    }

    public void MarkUsableItemsUnused()
    {
        foreach (InteractableObject obj in interactableItems.usableItemList)
        {
            obj.isUsed = false;
        }
    }
}
