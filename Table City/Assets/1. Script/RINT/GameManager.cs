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
    public Asset asset;
    [Header("공장 정보 입력 용도")]
    public Factory[] viewFactory;
    [Header("연출")]
    public AnimeBundle[] anime;

    public Dictionary<AssetData, FactoyData> factoryScript = new Dictionary<AssetData, FactoyData>();

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
                for (int a = 0; a < k.condition.Length; a++)
                {
                    k.model[a].SetActive(false);
                }
            }
        }
    }
    public Dictionary<AssetData, int> factoryLv = new Dictionary<AssetData, int>();

    public void SetFactoryItem(AssetData dataType, Asset count)
    {
        factoryScript[dataType].SetFactoryItem(count);

        foreach (FactoyData i in factoryScript.Values)
        {
            if (factoryLv.ContainsKey(i.data.createAsset))
                factoryLv[i.data.createAsset] = i.data.lv;
            else
                factoryLv.Add(i.data.createAsset, i.data.lv);
        }

        foreach(AnimeBundle j in anime)
        {
            foreach (Anime k in j.data)
            {
                for (int a =0; a < k.condition.Length; a++)
                {
                    if (k.model[a].activeSelf == true||k.bundle.activeSelf == false )continue;
                    Asset condition = k.condition[a];

                    if (!condition.InputWorthMoreThanThis(AssetData.wood, factoryLv[AssetData.wood]))continue;
                    else if (!condition.InputWorthMoreThanThis(AssetData.uranium, factoryLv[AssetData.uranium])) continue;
                    else if (!condition.InputWorthMoreThanThis(AssetData.stone, factoryLv[AssetData.stone])) continue;
                    else if (!condition.InputWorthMoreThanThis(AssetData.steel, factoryLv[AssetData.steel])) continue;
                    else if (!condition.InputWorthMoreThanThis(AssetData.semiconductor, factoryLv[AssetData.semiconductor])) continue;
                    else if (!condition.InputWorthMoreThanThis(AssetData.rubber, factoryLv[AssetData.rubber])) continue;
                    else if (!condition.InputWorthMoreThanThis(AssetData.mithril, factoryLv[AssetData.mithril])) continue;
                    else if (!condition.InputWorthMoreThanThis(AssetData.glass, factoryLv[AssetData.glass])) continue;
                    else if (!condition.InputWorthMoreThanThis(AssetData.floatingStone, factoryLv[AssetData.floatingStone])) continue;
                    else if (!condition.InputWorthMoreThanThis(AssetData.electricity, factoryLv[AssetData.electricity] )) continue;
                    else if (!condition.InputWorthMoreThanThis(AssetData.cloth, factoryLv[AssetData.cloth] )) continue;
                    else if (!condition.InputWorthMoreThanThis(AssetData.coal, factoryLv[AssetData.coal] )) continue;


                    k.model[a].SetActive(true);
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Asset plus = new Asset();
            plus.wood += 1;
            SetFactoryItem(AssetData.wood, plus);
        }
    }
}
[System.Serializable]
public struct Asset
{
    public void ChangeData(AssetData data, int count)
    {
        switch (data)
        {
            case AssetData.wood: 
                wood += count;
                break;
            case AssetData.stone: 
                stone += count;
                break;
            case AssetData.steel: 
                steel += count;
                break;
            case AssetData.cloth: 
                cloth += count;
                break;
            case AssetData.coal: 
                coal += count;
                break;
            case AssetData.electricity: 
                electricity += count;
                break;
            case AssetData.glass: 
                glass += count;
                break;
            case AssetData.rubber: 
                rubber += count;
                break;
            case AssetData.uranium: 
                uranium += count;
                break;
            case AssetData.semiconductor: 
                semiconductor += count;
                break;
            case AssetData.mithril: 
                mithril += count;
                break;
            case AssetData.floatingStone: 
                floatingStone += count;
                break;
        }
        
    }
    public bool ThisWorthMoreThanInput(AssetData data, int count)
    {
        switch (data)
        {
            case AssetData.wood: return wood >= count ? true : false;
            case AssetData.stone: return stone >= count ? true : false;
            case AssetData.steel: return steel >= count ? true : false;
            case AssetData.cloth: return cloth >= count ? true : false;
            case AssetData.coal: return coal >= count ? true : false;
            case AssetData.electricity: return electricity >= count ? true : false;
            case AssetData.glass: return glass >= count ? true : false;
            case AssetData.rubber: return rubber >= count ? true : false;
            case AssetData.uranium: return uranium >= count ? true : false;
            case AssetData.semiconductor: return semiconductor >= count ? true : false;
            case AssetData.mithril: return mithril >= count ? true : false;
            case AssetData.floatingStone: return floatingStone >= count ? true : false;
            default : Debug.LogError("데이터 값이 틀림"); return false;
        }
    }
    public bool InputWorthMoreThanThis(AssetData data, int count)
    {
        switch (data)
        {
            case AssetData.wood: return wood <= count ? true : false;
            case AssetData.stone: return stone <= count ? true : false;
            case AssetData.steel: return steel <= count ? true : false;
            case AssetData.cloth: return cloth <= count ? true : false;
            case AssetData.coal: return coal <= count ? true : false;
            case AssetData.electricity: return electricity <= count ? true : false;
            case AssetData.glass: return glass <= count ? true : false;
            case AssetData.rubber: return rubber <= count ? true : false;
            case AssetData.uranium: return uranium <= count ? true : false;
            case AssetData.semiconductor: return semiconductor <= count ? true : false;
            case AssetData.mithril: return mithril <= count ? true : false;
            case AssetData.floatingStone: return floatingStone <= count ? true : false;
            default: Debug.LogError("데이터 값이 틀림"); return false;
        }
    }

    [field: SerializeField,Header("나무")]
    public int wood { get; set; }
    [field: SerializeField, Header("돌")]
    public int stone { get; set; }
    [field: SerializeField, Header("철")]
    public int steel { get; set; }
    [field: SerializeField, Header("천")]
    public int cloth { get; set; }
    [field: SerializeField, Header("석탄")]
    public int coal { get; set; }
    [field: SerializeField, Header("전기")]
    public int electricity { get; set; }
    [field: SerializeField, Header("유리")]
    public int glass { get; set; }
    [field: SerializeField, Header("고무")]
    public int rubber { get; set; }
    [field: SerializeField, Header("우라늄")]
    public int uranium { get; set; }
    [field: SerializeField, Header("반도체")]
    public int semiconductor { get; set; }

    [field: SerializeField, Header("미스릴")]
    public int mithril { get; set; }

    [field: SerializeField, Header("부유석")]
    public int floatingStone { get; set; }
}

[System.Serializable]
public struct AnimeBundle
{

    [field: SerializeField]
    public string name { get; set; }
    [field: SerializeField]
    public Ending ending { get; set; }

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

    public Asset[] condition;
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
    public Asset[] upGrade;
    [field: SerializeField]
    public AssetData createAsset { get; set; }
}
public enum AssetData
{
   wood, stone,steel ,cloth,coal,electricity, glass , rubber, uranium , semiconductor, mithril, floatingStone
}

[System.Serializable]
public enum Ending
{
    floatingIsland,
    future,
    space,
    Destruction,
    normal
}
