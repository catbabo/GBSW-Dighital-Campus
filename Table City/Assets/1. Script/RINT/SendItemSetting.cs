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
    private TextMeshProUGUI valueText;

    // Start is called before the first frame update
    void Start()
    {
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
        string viewText = "";
        int count = 0;
        foreach (Define.AssetData i in Enum.GetValues(typeof(Define.AssetData)))
        {
            if (count == 2)
            {
                count = 0;
                if (inputBoxController.asset[(int)i] != 0)
                    viewText += "\n" + i.ToString() + " " + inputBoxController.asset[(int)i] + " / ";
            }
            else
            {
                count++;
                if (inputBoxController.asset[(int)i] != 0)
                    viewText += i.ToString() + " " + inputBoxController.asset[(int)i] + " / ";
            }
        }
        valueText.text = viewText;
    }
}
