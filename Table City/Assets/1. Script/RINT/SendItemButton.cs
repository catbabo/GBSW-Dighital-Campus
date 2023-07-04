using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class SendItemButton : MonoBehaviour
{
    [field:SerializeField]
    public Image icon { get; set; }

    [field: SerializeField]
    public Define.AssetData type {private get; set; }
    [SerializeField]

    private TextMeshProUGUI valueText;


    public InputBoxController inputBoxController { private get; set; }

    public void SendItemPlay()
    {
        RoomManager.room.SyncItemData(inputBoxController, type);
    }
}
