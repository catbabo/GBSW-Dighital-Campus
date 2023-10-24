using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
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
