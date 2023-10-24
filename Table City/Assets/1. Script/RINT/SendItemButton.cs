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
            string itemName = "";
            switch (_assetData)
            {
                case Define.AssetData.wood:
                    itemName = "����";
                    break;
                case Define.AssetData.rubber:
                    itemName = "��";
                    break;
                case Define.AssetData.coal:
                    itemName = "��ź";
                    break;
                case Define.AssetData.cloth:
                    itemName = "õ";
                    break;
                case Define.AssetData.electricity:
                    itemName = "����";
                    break;
                case Define.AssetData.floatingStone:
                    itemName = "������";
                    break;
                case Define.AssetData.glass:
                    itemName = "����";
                    break;
                case Define.AssetData.mithril:
                    itemName = "�̽���";
                    break;
                case Define.AssetData.semiconductor:
                    itemName = "�ݵ�ü";
                    break;
                case Define.AssetData.steel:
                    itemName = "ö";
                    break;
                case Define.AssetData.uranium:
                    itemName = "���";
                    break;
                case Define.AssetData.stone:
                    itemName = "��";
                    break;

            }

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
        string itemName = "";
        switch (type)
        {
            case Define.AssetData.wood:
                itemName = "����";
                break;
            case Define.AssetData.rubber:
                itemName = "��";
                break;
            case Define.AssetData.coal:
                itemName = "��ź";
                break;
            case Define.AssetData.cloth:
                itemName = "õ";
                break;
            case Define.AssetData.electricity:
                itemName = "����";
                break;
            case Define.AssetData.floatingStone:
                itemName = "������";
                break;
            case Define.AssetData.glass:
                itemName = "����";
                break;
            case Define.AssetData.mithril:
                itemName = "�̽���";
                break;
            case Define.AssetData.semiconductor:
                itemName = "�ݵ�ü";
                break;
            case Define.AssetData.steel:
                itemName = "ö";
                break;
            case Define.AssetData.uranium:
                itemName = "���";
                break;
            case Define.AssetData.stone:
                itemName = "��";
                break;

        }
        nameText.text = itemName + " ����"+" : LV" + Managers.Game.factoryScript[type].data.lv;
    }

    public void SendItemPlay() => inputBoxController.SendItem(type);

    public void SpeedUp()
    {
        if(inputBoxController.speedUpSkill == true)
        {
            inputBoxController.coolTime = 0;
            inputBoxController.speedUpSkill = false;
            inputBoxController.SpeedUp(type);
        }
    }
}
