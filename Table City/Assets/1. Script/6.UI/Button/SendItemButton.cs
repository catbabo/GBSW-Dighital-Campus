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
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI[] valueText;

    public InputBoxController inputBoxController;

    private void Update()
    {
        LvViewUpdate();
        ValueTextUpdate();
    }

    private void ValueTextUpdate()
    {
        string viewText = "";
        int count = 0;
        foreach (Define.AssetData _assetData in Enum.GetValues(typeof(Define.AssetData)))
        {
            count++;
            string itemName = Define.Resources[(int)_assetData];

            if(Managers.Game.factoryScript[type].data.lv  != Managers.Game.factoryScript[type].data.maxLv)
            {
                viewText += "<color=#A4A4A4>" + itemName + " : </color>" +
                (
                    Managers.Game.factoryScript[type].data.upgrade
                    [
                        Managers.Game.factoryScript[type].data.lv, (int)_assetData
                    ]
                    - Managers.Game.factoryScript[type].asset[(int)_assetData]

                ) + "\n";
            }

            if (count > 6)
                valueText[1].text = viewText;
            else
                valueText[0].text = viewText;

            if (count == 6)
                viewText = "";
        }
    }

    private void LvViewUpdate()
    {
        string itemName = Define.Resources[(int)type];
        nameText.text = itemName + " °øÀå"+" : LV" + Managers.Game.factoryScript[type].data.lv;
    }

    public void SendItemPlay() { inputBoxController.SendItem(type); }

    RoomScene room
    {
        get
        {
            if (room == null)
            {
                room = GameObject.Find("RoomScene").GetComponent<RoomScene>();
            }
            return room;
        }

        set { room = value; }
    }

    public void SpeedUp()
    {
        if(inputBoxController.speedUpSkill == true)
        {
            inputBoxController.coolTime = 0;
            inputBoxController.speedUpSkill = false;
            room.SpeedUp(type);
        }
    }
}
