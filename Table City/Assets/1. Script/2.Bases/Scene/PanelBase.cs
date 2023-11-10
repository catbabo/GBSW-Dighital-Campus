using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
    protected Define.Panel _type;
    [HideInInspector]
    public GameObject _panel;

    public virtual void Init() { }

    protected virtual void InitUI() { }

    public virtual void OnShow() { }

    public virtual void LeftPanel()
    {
        gameObject.SetActive(false);
    }

    public Define.Panel GetType()
    {
        return _type;
    }
}
