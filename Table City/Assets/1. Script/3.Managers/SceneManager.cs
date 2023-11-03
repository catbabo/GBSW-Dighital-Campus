using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//씬 전환 기능을 담당하고 씬 정보를 갖고 있음

public class SceneManager : ManagerBase
{
    [SerializeField]
    private List<SceneBase> scenes = new List<SceneBase>();
    private Define.Scene _nextScene = Define.Scene.Load;
    private float _loadingTime;
    private bool _onLoading;

    public override void Init()
    {
        _onLoading = false;
        _loadingTime = 3;
        for (int i = 0; i < scenes.Count; i++)
        {
            scenes[i].Init();
        }
    }

    public void LoadScene(Define.Scene scene = Define.Scene.Load)
    {
        LoadScene(scene, false);
    }

    public void LoadScene(bool isSkip)
    {
        LoadScene(Define.Scene.Load, isSkip);
    }

    public void LoadScene(Define.Scene scene, bool isSkip)
    {
        if(isSkip)
        {
            scenes[(int)_nextScene].LeftScene();
            _nextScene = scene;
            _onLoading = true;
        }

        if (_onLoading)
        {
            _onLoading = false;
            ShowScene(_nextScene);
        }
        else
        {
            scenes[(int)_nextScene].LeftScene();
            _nextScene = scene;

            ShowScene(Define.Scene.Load);
        }
    }

    private void ShowScene(Define.Scene scene)
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            scenes[i]._scene.SetActive(false);
        }
        scenes[(int)scene]._scene.SetActive(true);

        StartLoad(scenes[(int)scene]);
    }

    private void StartLoad<T>(T scene) where T : SceneBase
    {
        scene.StartLoad();
    }

    public Define.Scene GetNextScene()
    {
        return _nextScene;
    }

    public float GetLoadTime()
    {
        return _loadingTime;
    }

    public void OnLoad()
    {
        _onLoading = true;
    }
}