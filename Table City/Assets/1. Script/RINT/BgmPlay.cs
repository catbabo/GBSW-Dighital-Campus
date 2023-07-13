using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlay : MonoBehaviour
{
    [SerializeField]
    private string bgmName;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.sound.BgmPlay(bgmName);
    }
}
