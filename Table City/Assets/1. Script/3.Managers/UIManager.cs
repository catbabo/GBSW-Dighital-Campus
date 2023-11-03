using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIManager : ManagerBase
{
    Dictionary<string, GameObject> ui = new Dictionary<string, GameObject>();

    [SerializeField]
    private Popup popup;

    public override void Init()
    {
        if(ui.Count == 0)
            ResourceLoad(ui, "2.Prefab/2.UI");

        popup.Init();
    }
    private void ResourceLoad(Dictionary<string, GameObject> dictionary, string filePath)
    {

        GameObject[] gameObject = Resources.LoadAll<GameObject>(filePath);

        foreach (GameObject oneGameObject in gameObject)
        {
            dictionary.Add(oneGameObject.name, oneGameObject);
        }
    }

    public void ShowPopup(string header, string subject)
    {
        popup.Show(header, subject);
    }
}