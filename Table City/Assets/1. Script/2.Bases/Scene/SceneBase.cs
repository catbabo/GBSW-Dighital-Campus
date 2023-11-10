using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBase : MonoBehaviour
{
    [HideInInspector]
    public GameObject _scene;
    protected Define.Scene _type;
    protected string _name;

    [SerializeField]
    protected List<PanelBase> panels = new List<PanelBase>();

    protected virtual void OnLoad() { }

    public virtual void Init() { }

    protected virtual void InitUI() { }

    protected virtual void InitPanel() { }

    public virtual void LeftScene() { }

    public void ShowPanel(Define.Panel scene)
    {
        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i]._panel.activeSelf)
                panels[i].LeftPanel();
        }
        panels[(int)scene]._panel.SetActive(true);
        panels[(int)scene].OnShow();
    }

    public void RegistrationPanel(PanelBase panel)
    {
        panel.Init();
        panels[(int)panel.GetType()] = panel;
    }
}
