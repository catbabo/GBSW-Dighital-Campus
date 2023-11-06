using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InstanceManager : ManagerBase
{
    /*프리펩 데이터*/
    Dictionary<string, GameObject> prefab = new Dictionary<string, GameObject>();

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

    public GameObject SpawnObject(string objectName, Transform point = null)
    {
        Debug.Log("Spawn");
        GameObject go = PhotonNetwork.Instantiate(objectName, Vector3.zero, Quaternion.identity);

        if (point != null)
        {
            go.transform.position = point.position;
            go.transform.rotation = point.rotation;
        }
        return go;
    }

    /*오브젝트 풀링*/
    public GameObject UsePoolingObject(string useObjectname, Vector3 position, Quaternion rotation)
    {
        GameObject returnObject;

        if (poolingObject.ContainsKey(useObjectname) && poolingObject[useObjectname].Count > 0)
        {
            returnObject = poolingObject[useObjectname].Dequeue();
            returnObject.SetActive(true);
        }
        else
        {
            returnObject = Instantiate(prefab[useObjectname]);
            returnObject.name = useObjectname;
            returnObject.transform.parent = poolingParent;
        }
        returnObject.transform.position = position;
        returnObject.transform.rotation = rotation;

        return returnObject;
    }

    public void AddPooling(GameObject addObject)
    {
        if (!poolingObject.ContainsKey(addObject.name)) poolingObject.Add(addObject.name, new Queue<GameObject>());
        poolingObject[addObject.name].Enqueue(addObject);
        addObject.SetActive(false);
    }
}
