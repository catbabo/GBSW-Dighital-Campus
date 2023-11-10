using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase
{
    [SerializeField]
    private Popup popup;

    public override void Init() { }

    public void ShowPopup(string header, string subject)
    {
        popup.Show(header, subject);
    }

    public void SetPopup(GameObject obj)
    {
        popup = obj.GetComponent<Popup>();

        popup.Init();
    }
}