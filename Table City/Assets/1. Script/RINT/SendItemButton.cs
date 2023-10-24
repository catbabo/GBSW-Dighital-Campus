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
                    itemName = "나무";
                    break;
                case Define.AssetData.rubber:
                    itemName = "고무";
                    break;
                case Define.AssetData.coal:
                    itemName = "석탄";
                    break;
                case Define.AssetData.cloth:
                    itemName = "천";
                    break;
                case Define.AssetData.electricity:
                    itemName = "전기";
                    break;
                case Define.AssetData.floatingStone:
                    itemName = "부유석";
                    break;
                case Define.AssetData.glass:
                    itemName = "유리";
                    break;
                case Define.AssetData.mithril:
                    itemName = "미스릴";
                    break;
                case Define.AssetData.semiconductor:
                    itemName = "반도체";
                    break;
                case Define.AssetData.steel:
                    itemName = "철";
                    break;
                case Define.AssetData.uranium:
                    itemName = "우라늄";
                    break;
                case Define.AssetData.stone:
                    itemName = "돌";
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
                itemName = "나무";
                break;
            case Define.AssetData.rubber:
                itemName = "고무";
                break;
            case Define.AssetData.coal:
                itemName = "석탄";
                break;
            case Define.AssetData.cloth:
                itemName = "천";
                break;
            case Define.AssetData.electricity:
                itemName = "전기";
                break;
            case Define.AssetData.floatingStone:
                itemName = "부유석";
                break;
            case Define.AssetData.glass:
                itemName = "유리";
                break;
            case Define.AssetData.mithril:
                itemName = "미스릴";
                break;
            case Define.AssetData.semiconductor:
                itemName = "반도체";
                break;
            case Define.AssetData.steel:
                itemName = "철";
                break;
            case Define.AssetData.uranium:
                itemName = "우라늄";
                break;
            case Define.AssetData.stone:
                itemName = "돌";
                break;

        }
        nameText.text = itemName + " 공장"+" : LV" + Managers.Game.factoryScript[type].data.lv;
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
