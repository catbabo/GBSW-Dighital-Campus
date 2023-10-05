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

    public void SetRoot(Transform root)
    {
        _root = root;
    }

    public void SetRoot(string root)
    {
        _root = GameObject.Find(root).transform;
    }
}
