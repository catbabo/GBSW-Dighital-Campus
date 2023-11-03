using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScene : SceneBase
{
    private float _speed;
    [SerializeField]
    private Slider loadBar;

    public override void Init()
    {
        _scene = gameObject;
        _type = Define.Scene.Load;
        _name = "Load";
        InitUI();
    }

    public override void StartLoad()
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
        OnLoad();

        yield return null;
    }

    protected override void OnLoad()
    {
        Managers.Scene.OnLoad();
        Managers.Scene.LoadScene();
    }
}
