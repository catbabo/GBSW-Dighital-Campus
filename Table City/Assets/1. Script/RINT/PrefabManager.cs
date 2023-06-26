 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    /*프리펩 데이터*/
    Dictionary<string, GameObject> prefab = new Dictionary<string, GameObject>();

    /*오브젝트 풀링*/
    private Dictionary<string, Queue<GameObject>> poolingObject = new Dictionary<string, Queue<GameObject>>();
    private Transform poolingParent;

    private void Awake()
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
    /*오브젝트 풀링*/
    public GameObject UsePoolingObject(string useObjectname, Vector3 position, Quaternion rotation)
    {
        if (poolingObject.ContainsKey(useObjectname) && poolingObject[useObjectname].Count > 0)
        {
            GameObject returnObject = poolingObject[useObjectname].Dequeue();
            returnObject.transform.position = position;
            returnObject.transform.rotation = rotation;

            returnObject.SetActive(true);

            return returnObject;
        }
        else
        {
            GameObject returnObject = Instantiate(prefab[useObjectname], position, rotation);
            returnObject.name = useObjectname;
            returnObject.transform.parent = poolingParent;
            return returnObject;
        }
        /*오브젝트 풀링*/
    }
    public GameObject UsePoolingObject(GameObject useObject, Vector3 position, Quaternion rotation)
    {
        if (poolingObject.ContainsKey(useObject.name) && poolingObject[useObject.name].Count > 0)
        {
            GameObject returnObject = poolingObject[useObject.name].Dequeue();
            returnObject.transform.position = position;
            returnObject.transform.rotation = rotation;

            returnObject.SetActive(true);

            return returnObject;
        }
        else
        {
            GameObject returnObject = Instantiate(useObject, position, rotation);
            returnObject.name = useObject.name;
            returnObject.transform.parent = poolingParent;
            return returnObject;
        }
    }
    public void AddPooling(GameObject addObject)
    {
        if (!poolingObject.ContainsKey(addObject.name)) poolingObject.Add(addObject.name, new Queue<GameObject>());
        poolingObject[addObject.name].Enqueue(addObject);
        addObject.SetActive(false);
    }
}
