using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    private string _bgmName = "bgm1";

    void Start()
    {
        Managers.Sound.BgmPlay(_bgmName);
    }
}
