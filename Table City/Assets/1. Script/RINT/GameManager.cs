using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [field: SerializeField, Header("엔딩 진행도")]
    public int[] endingValues { get; set; } = new int[4];

    [field: SerializeField, Header("자원")]
    public int[] asset { get; set; } = new int[12];

    public string[] countText { get; set; } = new string[2];

    [Header("공장 정보 입력 용도")]
    public Factory[] viewFactory;
    [Header("연출")]
    public AnimeBundle[] anime;

    public Dictionary<Define.AssetData, FactoyData> factoryScript = new Dictionary<Define.AssetData, FactoyData>(); //공장 스크립트들 모두 연결
    [field: SerializeField, Header("레벨 변경 상황")]
    private int[] factoryLvCheck = new int[12]; // 레벨 변경 상황 확인 용도

    public Vector3[] _workbenchPointsA { get; private set; } =  new Vector3[6];
    public Vector3[] _workbenchPointsB { get; private set; } =  new Vector3[6];

    #region 타이머
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
    #endregion


    private void Awake()
    {
        SetFactory();
        SetAnimation();
    }

    // 연출 초기 설정 #엑셀 파싱
    private void SetAnimation()
    {
        List<Dictionary<string, object>> csvData = CSVReader.Read("3.Csv/Animation");


        for (int i = 0; i < anime.Length; i++)
        {
            foreach (var csvLineSet in csvData)
            {
                if (anime[i].name == csvLineSet["AreaName"].ToString())
                {
                    int[] csvLine = new int[15];


                    csvLine[0] = int.Parse(csvLineSet["GroupNum"].ToString());
                    csvLine[1] = int.Parse(csvLineSet["AnimeNum"].ToString());
                    csvLine[2] = int.Parse(csvLineSet["영향력"].ToString().Replace("%", ""));

                    csvLine[(int)Define.AssetData.wood + 3]
                       = int.Parse(csvLineSet["나무"].ToString());

                    csvLine[(int)Define.AssetData.stone + 3]
                        = int.Parse(csvLineSet["돌"].ToString());

                    csvLine[(int)Define.AssetData.steel + 3]
                        = int.Parse(csvLineSet["철"].ToString());

                    csvLine[(int)Define.AssetData.cloth + 3]
                        = int.Parse(csvLineSet["천"].ToString());

                    csvLine[(int)Define.AssetData.coal + 3]
                        = int.Parse(csvLineSet["석탄"].ToString());

                    csvLine[(int)Define.AssetData.electricity + 3]
                        = int.Parse(csvLineSet["전기"].ToString());

                    csvLine[(int)Define.AssetData.glass + 3]
                        = int.Parse(csvLineSet["유리"].ToString());

                    csvLine[(int)Define.AssetData.rubber + 3]
                        = int.Parse(csvLineSet["고무"].ToString());

                    csvLine[(int)Define.AssetData.uranium + 3]
                        = int.Parse(csvLineSet["우라늄"].ToString());

                    csvLine[(int)Define.AssetData.semiconductor + 3]
                        = int.Parse(csvLineSet["반도체"].ToString());

                    csvLine[(int)Define.AssetData.mithril + 3]
                        = int.Parse(csvLineSet["미스릴"].ToString());

                    csvLine[(int)Define.AssetData.floatingStone + 3]
                        = int.Parse(csvLineSet["부유석"].ToString());

                    anime[i].SetData(csvLine);
                }
            }

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

    // 연출 확인 조건 확인 후 실행
    private void PlayAnimation()
    {
        foreach (AnimeBundle j in anime)
        {
            for (int k =0; k< j.data.Length;k++)
            {
                if (j.data[k].bundle.activeSelf == false)
                    continue;
                for (int a = 0; a < j.data[k].model.Length; a++)
                {
                    bool on = true;
                    for (int b = 0; b < 12; b++)
                    {
                        if (j.data[k].condition[a,b] > factoryLvCheck[b])
                        {
                            on = false;
                            break;
                        }
                    }
                    if(on == true)
                    {
                        if (k-1 != -1 && j.data[k].bundle.activeSelf == true)
                        {
                            j.data[k - 1].bundle.GetComponent<Animator>().SetBool("Start", true);
                            ActionTimer(4, () => j.data[k - 1].bundle.SetActive(false));
                        }

                        if(j.data[k].model[a].activeSelf == false)
                        {
                            endingValues[(int)j.ending] = j.data[k].Influence[a];
                            j.data[k].model[a].SetActive(true);
                        }
                    }
                }
            }
        }
    }

    // 공장 초기 설정
    private void SetFactory()
    {
        List<Dictionary<string, object>> csvData = CSVReader.Read("3.Csv/Upgrade");

        for(int i = 0; i < viewFactory.Length; i++)
        {
            foreach(var csvLineSet in csvData)
            {
                viewFactory[i].upgrade = new int[viewFactory[i].maxLv, 12];

                if (viewFactory[i].name == csvLineSet["업그레이드 요소"].ToString())
                {
                    
                    viewFactory[i].upgrade[int.Parse(csvLineSet["LV"].ToString()) - 1,(int)Define.AssetData.wood] 
                        = int.Parse(csvLineSet["나무"].ToString());

                    viewFactory[i].upgrade[int.Parse(csvLineSet["LV"].ToString()) - 1, (int)Define.AssetData.stone]
                        = int.Parse(csvLineSet["돌"].ToString());

                    viewFactory[i].upgrade[int.Parse(csvLineSet["LV"].ToString()) - 1, (int)Define.AssetData.steel]
                        = int.Parse(csvLineSet["철"].ToString());

                    viewFactory[i].upgrade[int.Parse(csvLineSet["LV"].ToString()) - 1, (int)Define.AssetData.cloth]
                        = int.Parse(csvLineSet["천"].ToString());

                    viewFactory[i].upgrade[int.Parse(csvLineSet["LV"].ToString()) - 1, (int)Define.AssetData.coal]
                        = int.Parse(csvLineSet["석탄"].ToString());

                    viewFactory[i].upgrade[int.Parse(csvLineSet["LV"].ToString()) - 1, (int)Define.AssetData.electricity]
                        = int.Parse(csvLineSet["전기"].ToString());

                    viewFactory[i].upgrade[int.Parse(csvLineSet["LV"].ToString()) - 1, (int)Define.AssetData.glass]
                        = int.Parse(csvLineSet["유리"].ToString());

                    viewFactory[i].upgrade[int.Parse(csvLineSet["LV"].ToString()) - 1, (int)Define.AssetData.rubber]
                        = int.Parse(csvLineSet["고무"].ToString());

                    viewFactory[i].upgrade[int.Parse(csvLineSet["LV"].ToString()) - 1, (int)Define.AssetData.uranium]
                        = int.Parse(csvLineSet["우라늄"].ToString());

                    viewFactory[i].upgrade[int.Parse(csvLineSet["LV"].ToString()) - 1, (int)Define.AssetData.semiconductor]
                        = int.Parse(csvLineSet["반도체"].ToString());

                    viewFactory[i].upgrade[int.Parse(csvLineSet["LV"].ToString()) - 1, (int)Define.AssetData.mithril]
                        = int.Parse(csvLineSet["미스릴"].ToString());

                    viewFactory[i].upgrade[int.Parse(csvLineSet["LV"].ToString()) - 1, (int)Define.AssetData.floatingStone]
                        = int.Parse(csvLineSet["부유석"].ToString());

                }
            }
            //스크립트 넣기
            FactoyData script = viewFactory[i].model.AddComponent<FactoyData>();
            factoryScript.Add(viewFactory[i].createAsset, script);
            script.data = viewFactory[i];

        }

    }

    //공장에 아이템 넣기
    public void InputFactoryItem(Define.AssetData factoryType, Define.AssetData itemType, int count)
    {
        //아이템 증가
        factoryScript[factoryType].asset[(int)itemType] += count;


        //업그레이드 정보 확인
        for (int i = factoryScript[factoryType].data.lv; i < factoryScript[factoryType].data.maxLv; i++)
        {
            bool checkLvUp = true;
            for (int j = 0; j < 12; j++)
            {
                if (factoryScript[factoryType].data.upgrade[i,j] > factoryScript[factoryType].asset[j])
                {
                    checkLvUp = false;
                    break;
                }
            }

            if(checkLvUp == true) factoryScript[factoryType].data.lv += 1;
        }
        //연출 조건 확인 및 실행
        if (factoryLvCheck[(int)factoryType] != factoryScript[factoryType].data.lv)
        {
            factoryLvCheck[(int)factoryType] = factoryScript[factoryType].data.lv;
            PlayAnimation();
        }


    }

    public void SetWorkbechPoint(int index, Vector3 pos, bool _pointA)
    {
		if (_pointA) { _workbenchPointsA[index] = pos; }
        else { _workbenchPointsB[index] = pos; }
        
    }
    public Vector3 GetWorkbechPoint(Define.AssetData type, bool _pointA)
    {
        int index = (int)type;
        if (index >= 6)
        {
            index -= 6;
        }

        if (_pointA)
        {
            return _workbenchPointsA[index];
        }
		else
		{
            return _workbenchPointsB[index];
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

    public void SetData(int[] csvLine)
    {
        if(data[csvLine[0] - 1].Influence.Length == 0)
        {
            data[csvLine[0]-1].Influence = new int[data[csvLine[0] - 1].model.Length];
            data[csvLine[0]-1].condition = new int[data[csvLine[0] - 1].model.Length, 12];
        }

        data[csvLine[0] - 1].Influence[csvLine[1] - 1] = csvLine[2];

        for(int i = 3; i < csvLine.Length; i++)
            data[csvLine[0] - 1].condition[csvLine[1] - 1,i-3] = csvLine[i];

    }
    
}

[System.Serializable]
public struct Anime
{
    [field: SerializeField]
    public GameObject bundle { get; set; } // 영역 그룹

    [field: SerializeField]
    public GameObject[] model { get; set; } // 소환 모델

    [field: SerializeField]
    public int[] Influence { get; set; } // 영향력

    public int[,] condition { get; set; }  // 조건
}

[System.Serializable]
public struct Factory
{
    [field: SerializeField]
    public string name { get; set; }//공장 이름
    [field: SerializeField]
    public GameObject model { get; set; }//공장 모델
    [field: SerializeField]
    public int lv { get; set; }//현재 LV
    [field: SerializeField]
    public int maxLv { get; set; }//멕스 Lv
    public int[,] upgrade { get; set; } // Lv 별 업그레이드 정보
    [field: SerializeField]
    public Define.AssetData createAsset { get; set; } // 공장 Type
}