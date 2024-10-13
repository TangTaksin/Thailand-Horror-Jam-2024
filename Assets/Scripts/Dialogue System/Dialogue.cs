using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    public Message[] messages;
}

[System.Serializable]
public class Message
{
    public int actorId;
    public string message;
}

[System.Serializable]
public class Actor
{
    public string name;
}
