using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : PanelBase
{
    private float _speed;
    [SerializeField]
    private Slider loadBar;

    public override void Init()
    {
        _panel = gameObject;
        _type = Define.Panel.Load;
        InitUI();
        StartLoad();
    }

    private void StartLoad()
    {
        _speed = 100f / Managers.Scene.GetLoadTime();
        loadBar.value = 0;
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        while(loadBar.value < 100)
        {
            loadBar.value += _speed * Time.deltaTime;
            yield return null;
        }
        
        loadBar.value = 100;
        OnShow();

        yield return null;
    }

    public override void OnShow()
    {
        Managers.Scene.LoadScene();
    }

    public override void LeftPanel()
    {
        base.LeftPanel();
    }
}
