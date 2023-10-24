 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class InstantiateManager : ManagerBase
{
    public struct InstanceData
    {
        public string name;
        public GameObject gameObject;
    }

    /*프리펩 데이터*/
    private Dictionary<string, GameObject> prefab = new Dictionary<string, GameObject>();
    private Dictionary<string, List<GameObject>> _instance = new Dictionary<string, List<GameObject>>();

    /*오브젝트 풀링*/
    private Dictionary<string, Queue<GameObject>> poolingObject = new Dictionary<string, Queue<GameObject>>();
    private Transform poolingParent;

    public override void Init()
    {
        if (prefab.Count == 0)
        {
            ResourceLoad(prefab, Define.prefabType.building, "2.Prefab/0.Build");
            ResourceLoad(prefab, Define.prefabType.effect, "2.Prefab/1.Effect");
            ResourceLoad(prefab, Define.prefabType.other, "2.Prefab/3.Other");
        }

        poolingParent = new GameObject("Pooling").transform;
        poolingParent.position = Vector3.zero;
        poolingObject = new Dictionary<string, Queue<GameObject>>();
    }

    private void ResourceLoad(Dictionary<string, GameObject> dictionary, Define.prefabType type, string filePath)
    {

        GameObject[] gameObject = Resources.LoadAll<GameObject>(filePath);

        foreach (GameObject oneGameObject in gameObject)
        {
            dictionary.Add(type + oneGameObject.name, oneGameObject);
        }
    }

    private void ResourcesLoad(string key, string filePath)
    {
        GameObject[] gameObjects = Resources.LoadAll<GameObject>(filePath);
        if (gameObjects.Length == 0)
        {
            Debug.LogError("No Obejcts");
            return;
        }

        if (!HasKey(key))
        {
            _instance.Add(key, new List<GameObject>());
        }

        foreach (GameObject gameObject in gameObjects)
        {
            _instance[key].Add(gameObject);
        }
    }

    private bool HasKey(string key)
    {
        return _instance.ContainsKey(key);
    }

    public GameObject Instantiate(string key, string objectName, Vector3 position, Quaternion rotation, bool isPooling = false)
    {
        GameObject returnObject = Instantiate(GetObject(key, objectName), position, rotation);
        returnObject.transform.position = position;
        returnObject.transform.rotation = rotation;
        returnObject.name = objectName;
        returnObject.transform.parent = poolingParent;

        if (isPooling)
        {
            returnObject = poolingObject[objectName].Dequeue();

            returnObject.SetActive(true);

            return returnObject;
        }
        return returnObject;
    }

    private GameObject GetObject(string key, string objectName)
    {
        for(int i = 0; i < _instance[key].Count; i++)
        {
            if (_instance[key][i].name.Equals(objectName))
            {
                return _instance[key][i];
            }
        }

        Debug.LogError("No Object");
        return null;
    }

    /*오브젝트 풀링*/
    public GameObject UsePoolingObject(string useObjectname, Vector3 position, Quaternion rotation)
    {
        GameObject returnObject;
        if (poolingObject.ContainsKey(useObjectname) && poolingObject[useObjectname].Count > 0)
        {
            returnObject = poolingObject[useObjectname].Dequeue();
            returnObject.transform.position = position;
            returnObject.transform.rotation = rotation;

            returnObject.SetActive(true);
        }
        else
        {
            returnObject = Instantiate(prefab[useObjectname], position, rotation);
            returnObject.name = useObjectname;
            returnObject.transform.parent = poolingParent;
        }

            return returnObject;
        /*오브젝트 풀링*/
    }

    public void AddPooling(GameObject addObject)
    {
        if (!poolingObject.ContainsKey(addObject.name))
            poolingObject.Add(addObject.name, new Queue<GameObject>());

        poolingObject[addObject.name].Enqueue(addObject);
        addObject.SetActive(false);
    }
}
