using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SM = UnityEngine.SceneManagement.SceneManager;

//씬 전환 기능을 담당하고 씬 정보를 갖고 있음
public class SceneManager : ManagerBase
{
    private Define.Scene _nextScene = Define.Scene.Load;
    private float _loadingTime;
    private bool _onLoading;
    private SceneBase _scene;

    public override void Init()
    {
        _onLoading = false;
        _loadingTime = 3;
    }

    public void LoadScene(Define.Scene scene = Define.Scene.Load)
    {
        LoadScene(scene, false);
    }

    public void LoadScene(Define.Scene scene, bool isSkip)
    {
        if(_scene != null)
            _scene.LeftScene();

        if(isSkip)
        {
            _nextScene = scene;
            _onLoading = false;
            SM.LoadScene((int)_nextScene);
            return;
        }

        if (_onLoading)
        {
            _onLoading = false;
            SM.LoadScene((int)_nextScene);
        }
        else
        {
            _nextScene = scene;
            _onLoading = true;
            SM.LoadScene((int)Define.Scene.Load);
        }
    }

    public Define.Scene GetNextScene()
    {
        return _nextScene;
    }

    public float GetLoadTime()
    {
        return _loadingTime;
    }

    public void OnLoad(SceneBase scene)
    {
        _scene = scene;
    }

    public void ShowPanel(Define.Panel scene)
    {
        _scene.ShowPanel(scene);
    }



    private bool[,] isInited = new bool
        [System.Enum.GetValues(typeof(Define.Scene)).Length
        , System.Enum.GetValues(typeof(Define.Panel)).Length];

    public bool IsInitable(Define.Scene scene, Define.Panel panel)
    {
        if (isInited[(int)scene, (int)panel])
            return false;

        isInited[(int)scene, (int)panel] = true;
        return true;
    }
}