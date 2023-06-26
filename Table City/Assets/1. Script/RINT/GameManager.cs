using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //json ������ ���⿡
    public void ActionTimer(float time, int repeatCount, Action action, bool actionFirst) => StartCoroutine(ActionTimerCoroutine(time, repeatCount, action, actionFirst));
    public void ActionTimer(float time, int repeatCount, Action action) => StartCoroutine(ActionTimerCoroutine(time, repeatCount, action, false));
    public void ActionTimer(float time, Action action) => StartCoroutine(ActionTimerCoroutine(time, 1, action, false));

    private IEnumerator ActionTimerCoroutine(float time, int repeatCount, Action action, bool actionFirst)
    {
        for (int i = 0; i < repeatCount; i++)
        {
            if (actionFirst == true) action();
            yield return new WaitForSeconds(time);
            if (actionFirst == false) action();
        }
    }

    [Header("�ڿ�")]
    public int[] asset;
    [Header("���� ���� �Է� �뵵")]
    public Factory[] viewFactory;
    [Header("����")]
    public AnimeBundle[] anime;

    public Dictionary<Define.AssetData, FactoyData> factoryScript = new Dictionary<Define.AssetData, FactoyData>();

    private void Awake()
    {
        SetFactory();
    }
    private void SetFactory()
    {
        foreach(Factory i in viewFactory)
        {
            FactoyData script = i.model.AddComponent<FactoyData>();
            factoryScript.Add(i.createAsset, script);
            script.data = i;

        }
        foreach (AnimeBundle j in anime)
        {
            foreach (Anime k in j.data)
            {
                for (int a = 0; a < k.model.Length; a++)
                {
                    k.model[a].SetActive(false);
                }
            }
        }
    }
    public Dictionary<Define.AssetData, int> factoryLv = new Dictionary<Define.AssetData, int>();

    public void SetFactoryItem(Define.AssetData factoryType, Define.AssetData itemType, int count)
    {
        factoryScript[factoryType].asset[(int)itemType] += count;

        foreach (FactoyData i in factoryScript.Values)
        {
            if (factoryLv.ContainsKey(i.data.createAsset))
                factoryLv[i.data.createAsset] = i.data.lv;
            else
                factoryLv.Add(i.data.createAsset, i.data.lv);
        }

        
    }

}


[System.Serializable]
public struct AnimeBundle
{

    [field: SerializeField]
    public string name { get; set; }
    [field: SerializeField]
    public Define.Ending ending { get; set; }

    [field:SerializeField]
    public GameObject group { get; set; }

    public Anime[] data;
    
}
[System.Serializable]
public struct Anime
{
    [field: SerializeField]
    public GameObject bundle { get; set; }

    [field: SerializeField]
    public GameObject[] model { get; set; }

    [field: SerializeField]
    public int[] Influence { get; set; }

    public int[] condition { get; set; }

    Anime(int param=0)
    {
        this.condition = new int[100];
        this.bundle = null;
        this.model = null;
        this.Influence = null;
    }

}

[System.Serializable]
public struct Factory
{
    [field: SerializeField]
    public string name { get; set; }
    [field: SerializeField]
    public GameObject model { get; set; }
    [field: SerializeField]
    public int lv { get; set; }
    [field: SerializeField]
    public int[] upGrade;
    [field: SerializeField]
    public Define.AssetData createAsset { get; set; }
}