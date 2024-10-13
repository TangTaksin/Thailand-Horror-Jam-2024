using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    TextMeshProUGUI messageTxt;

    Dialogue currentDialogue;
    int messageIndex;

    void DisplayMessage()
    {
        messageTxt.text = currentDialogue.messages[messageIndex].message;
    }

    void AdvanceDialogue()
    {
        messageIndex++;
        messageTxt.text = currentDialogue.messages[messageIndex].message;
    }
}
