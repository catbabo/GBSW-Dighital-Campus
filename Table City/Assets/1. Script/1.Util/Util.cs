using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static void ChangeLayer(Transform _trans, string _name)
    {
        _trans.gameObject.layer = LayerMask.NameToLayer(_name);
    }

    public static PhotonView GetMainPhotonView()
    {
        return Managers.Network.GetPhotonView();
    }

    public static T AddOrGetComponent<T>(GameObject target) where T : UnityEngine.Component
    {
        T obj = target.GetComponent<T>();
        if (obj == null)
        {
            obj = target.AddComponent<T>();
        }
        return obj;
    }

    public static GameObject NewEmptyObject(string name, Transform parnet)
    {
        GameObject empty = new GameObject(name);
        empty.transform.parent = parnet;

        return empty;
    }

    public static void ResourceLoad<T>(Dictionary<string, T> dictionary, string filePath) where T : UnityEngine.Object
    {
        T[] gameObject = Resources.LoadAll<T>(filePath);

        foreach (T oneGameObject in gameObject)
        {
            dictionary.Add(oneGameObject.name, oneGameObject);
        }
    }
}
