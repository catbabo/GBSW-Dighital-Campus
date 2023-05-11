 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private Dictionary<string, GameObject> ResourceLoad(string filePath)
    {

        GameObject[] gameObject = Resources.LoadAll<GameObject>(filePath);
        Dictionary<string, GameObject> gameObjectData = new Dictionary<string, GameObject>();

        foreach (GameObject oneGameObject in gameObject)
        {
            gameObjectData.Add(oneGameObject.name, oneGameObject);
        }

        return gameObjectData;
    }

    public GameObject InstantiatePrefab(string name)
    {
        return null;
    }
}
