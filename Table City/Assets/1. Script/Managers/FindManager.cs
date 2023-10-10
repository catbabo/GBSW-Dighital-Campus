using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindManager : ManagerBase
{
    private Transform _root;

    public override void Init()
    {
        _root = null;
        base.Init();
    }

    public GameObject Find(string target)
    {
        if (_root = null)
            Debug.LogError("No Root Object");

        return _root.Find(target).gameObject;
    }

    public GameObject FindChild(string target)
    {
        if (_root = null)
            Debug.LogError("No Root Object");

        Transform go = null;
        for(int i = 0; i < _root.childCount; i++)
        {
            go = _root.GetChild(i).Find(target);
        }

        return go.gameObject;
    }

    public void SetRoot(Transform root)
    {
        _root = root;
    }

    public void SetRoot(GameObject root)
    {
        _root = root.transform;
    }

    public void SetRoot(string root)
    {
        _root = GameObject.Find(root).transform;
    }
}
