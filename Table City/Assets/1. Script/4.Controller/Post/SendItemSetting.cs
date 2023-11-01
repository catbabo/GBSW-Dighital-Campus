using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class SendItemSetting : MonoBehaviour
{
    [SerializeField]
    private Transform buttonPos;

    [SerializeField]
    private InputBoxController inputBoxController;

    [SerializeField]
    private TextMeshProUGUI valueText,speedUpText;
    
    void Start()
    {
        if (!inputBoxController._pv_workBench.IsMine) gameObject.SetActive(false);

        GameObject ui = Resources.Load<GameObject>("2.Prefab/2.UI/SendPanel");

        foreach (Define.AssetData i in Enum.GetValues(typeof(Define.AssetData)))
        {
            GameObject set = Instantiate(ui, buttonPos);
            set.name = i.ToString();
            SendItemButton script = set.GetComponent<SendItemButton>();
            script.icon.sprite = Resources.Load<Sprite>($"4.Image/Factory/{i}Factory");
            script.type = i;
            script.inputBoxController = inputBoxController;
        }
    }
    private void Update()
    {
        if (!inputBoxController._pv_workBench.IsMine) return;

        string viewText = "";
        int count = 0;
        foreach (Define.AssetData i in Enum.GetValues(typeof(Define.AssetData)))
        {
            if (inputBoxController.asset[(int)i] != 0)
            {
                if (count == 2)
                {
                    count = 1;
                    viewText += "\n";
                }
                else
                {
                    count++;
                }
                string itemName = "없음";
                itemName = Define.Resources[(int)i];
                viewText += "<color=#0000ff>" + itemName + " : </color>" + inputBoxController.asset[(int)i] + "  ";
            }

        }
        valueText.text = viewText;
        string time = inputBoxController.coolTime > 20 ? "0 / 20 초" : Mathf.CeilToInt(inputBoxController.coolTime)+ " / 20 초";
        speedUpText.text = $"가속\n-------------\n남은 시간: {time}\n사용 여부 : {inputBoxController.speedUpSkill}";
        
    }
}
