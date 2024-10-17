using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPart : MonoBehaviour, IInteractable
{
    public Vector3 position { get { return transform.position; } }

    [SerializeField] bool _isInteractable;
    public bool isInteractable 
    {
        get { return _isInteractable; }
        set { _isInteractable = value; } 
    }

    bool broken;
    public bool isBroken 
    {
        get { return broken; }
        set 
        {
            broken = value;
        }
    }

    public delegate void GameObjectEvent(GameObject gameObject);
    public static GameObjectEvent OnPartBreak;

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        isBroken = false;
    }

    public void Interact(object interacter)
    {
        isBroken = true;
        gameObject.SetActive(false);
        OnPartBreak?.Invoke(gameObject);
    }
}
