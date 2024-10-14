using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public delegate void GeneralEvent();
    public static GeneralEvent GamePause;
    public static GeneralEvent GameResume;

}
