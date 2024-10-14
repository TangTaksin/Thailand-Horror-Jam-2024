using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public Vector3 position { get;}
    public bool isInteractable { get; set; }
    public void Interact();
}
