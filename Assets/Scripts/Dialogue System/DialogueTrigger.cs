using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public delegate void TriggerEvent(Dialogue dialogue);
    public static TriggerEvent DialogueTriggered;

    public bool playOnStart;
    public Dialogue dialogue;

    public Vector3 position { get { return transform.position; } }

    [SerializeField] bool _isInteractable;
    public bool isInteractable { get { return _isInteractable; } set { _isInteractable = value; } }

    [SerializeField] public UnityEvent OnDialogueEnd;

    private void Start()
    {
        if (playOnStart)
            CallDialogue();
    }
    
    public void Interact(object obj)
    {
        print("interact with " + name);
        CallDialogue();
    }

    void CallDialogue()
    {
        DialogueTriggered?.Invoke(dialogue);
    }
}
