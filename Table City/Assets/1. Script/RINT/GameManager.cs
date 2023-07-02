using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //json 데이터 여기에
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

    [Header("자원")]
    public int[] asset;
    [Header("공장 정보 입력 용도")]
    public Factory[] viewFactory;
    [Header("연출")]
    public AnimeBundle[] anime;

    public Dictionary<Define.AssetData, FactoyData> factoryScript = new Dictionary<Define.AssetData, FactoyData>();

    private void Awake()
    {
        SetFactory();

        for(int i = 0; i< anime.Length; i++)
            anime[i].SetData();
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
    
    public void SetData()
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i].condition = new int[data[i].model.Length, 12];
            data[i].Influence = new int[data[i].model.Length];
            /*
            for (int j = 0; j < data.Length; j++)
            {
                data[i].condition = new int[data[i].model.Length, 12];
            }
            */
        }
    }
    
}

[System.Serializable]
public struct Anime
{
    [field: SerializeField]
    public GameObject bundle { get; set; } // 영역 그룹

    [field: SerializeField]
    public GameObject[] model { get; set; } // 소환 모델

    public int[] Influence { get; set; } // 영향력

    public int[,] condition { get; set; }  // 조건
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