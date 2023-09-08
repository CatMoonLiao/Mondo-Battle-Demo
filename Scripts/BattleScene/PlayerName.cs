using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    public string playerName;

    public void Awake()
    {
        playerName = LauncherManager.lm.playerName;
    }
}
