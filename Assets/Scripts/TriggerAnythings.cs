using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerAnythings : MonoBehaviour
{
    public UnityEvent OnEnter, OnStay, OnExit;
    public LayerMask mask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var colmask = collision.gameObject.layer;

        print(string.Format("trigger!, detected mask {0}, targeted mask {1}", colmask, mask.value));

        if (((1 << collision.gameObject.layer) & mask) != 0)
        {
            print("trigger mask");
            OnEnter?.Invoke();
        } 
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & mask) != 0)
        {
            print("trigger mask");
            OnStay?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & mask) != 0)
        {
            print("trigger mask");
            OnExit?.Invoke();
        }
    }
}
