using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public delegate void DialogueEvent();
    public static DialogueEvent DialogueCalled;
    public static DialogueEvent DialogueEnd;

    public GameObject dialogueBox;
    public TextMeshProUGUI speakerNameTxt;
    public TextMeshProUGUI messageTxt;

    Dialogue currentDialogue;
    int messageIndex;


    private void OnEnable()
    {
        InputRelay.Confirm += AdvanceDialogue; // Subscribe to Confirm event
        DialogueTrigger.DialogueTriggered += CallDialogue;
    }

    private void OnDisable()
    {
        InputRelay.Confirm -= AdvanceDialogue; // Unsubscribe from Confirm event
        DialogueTrigger.DialogueTriggered -= CallDialogue;
    }

    void CallDialogue(Dialogue dialogue)
    {
        DialogueCalled?.Invoke();
        messageIndex = 0;
        currentDialogue = dialogue;
        UpdateMessage();
        dialogueBox.SetActive(true);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        messageIndex = 0;
        dialogueBox.SetActive(true);
        UpdateMessage();
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        DialogueEnd?.Invoke();
    }

    void UpdateMessage()
    {
        var name_string = currentDialogue.actors[currentDialogue.messages[messageIndex].actorId].name;
        var message_string = currentDialogue.messages[messageIndex].message;

        speakerNameTxt.text = name_string;
        messageTxt.text = message_string;
    }

    void AdvanceDialogue()
    {

        if (!dialogueBox.activeSelf)
            return;

        messageIndex++;
        CheckDialogueProgress();
    }

    void CheckDialogueProgress()
    {
        if (messageIndex < currentDialogue.messages.Length)
            UpdateMessage();
        else
            EndDialogue();
    }

}
