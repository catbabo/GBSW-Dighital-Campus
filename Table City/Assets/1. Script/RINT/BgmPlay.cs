using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlay : MonoBehaviour
{

    [SerializeField]
    private int bgmId;


    private Define.SoundClipName[] bgmName = { Define.SoundClipName.bgm1, Define.SoundClipName.bgm6 };

    void Start()
    {
        Managers._sound.BgmPlay(bgmName[bgmId]);
    }
}
