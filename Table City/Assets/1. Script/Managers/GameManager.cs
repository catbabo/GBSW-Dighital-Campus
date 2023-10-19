using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class GameManager : ManagerBase
{
    [field: SerializeField, Header("엔딩 에니메이션")]
    public Animator endingAnime { set; get; }
    [SerializeField, Header("엔딩 텍스트")]
    private GameObject endingAnimeText;
    public int endingType { set; get; }


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

    [field: SerializeField]
    public Vector3[] _workbenchPointsA { get; private set; } = new Vector3[6];
    [field: SerializeField]
    public Vector3[] _workbenchPointsB { get; private set; } = new Vector3[6];

    #region 타이머
    public void ActionTimer(float time, Action action) => StartCoroutine(ActionTimerCoroutine(time, action));
    private IEnumerator ActionTimerCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
    #endregion


    public override void Init()
    {
        SetFactory();
        SetAnimation();
    }
    // 연출 초기 설정 #엑셀 파싱
    private void SetAnimation()
    {
        List<Dictionary<string, object>> csvData = CSVReader.Read("3.Csv/Animation");
        int backGroupNum = 99999;

        for (int i = 0; i < anime.Length; i++)
        {
            foreach (var csvLineSet in csvData)
            {
                if (anime[i].name == csvLineSet["AreaName"].ToString())
                {
                    int groupNum = int.Parse(csvLineSet["GroupNum"].ToString())-1;
                    int animeNum = int.Parse(csvLineSet["AnimeNum"].ToString()) -1;

                    if (backGroupNum != groupNum)
                    {
                        anime[i].data[groupNum].Influence = new int[anime[i].data[groupNum].model.Length];
                        anime[i].data[groupNum].condition = new int[anime[i].data[groupNum].model.Length, 12];
                        backGroupNum = groupNum;
                    }

                    anime[i].data[groupNum].Influence[animeNum] = int.Parse(csvLineSet["영향력"].ToString().Replace("%", ""));



                    anime[i].data[groupNum].condition[animeNum,(int)Define.AssetData.wood]
                       = int.Parse(csvLineSet["나무"].ToString());

                    anime[i].data[groupNum].condition[animeNum, (int)Define.AssetData.stone]
                        = int.Parse(csvLineSet["돌"].ToString());

                    anime[i].data[groupNum].condition[animeNum, (int)Define.AssetData.steel]
                        = int.Parse(csvLineSet["철"].ToString());

                    anime[i].data[groupNum].condition[animeNum, (int)Define.AssetData.cloth]
                        = int.Parse(csvLineSet["천"].ToString());

                    anime[i].data[groupNum].condition[animeNum, (int)Define.AssetData.coal]
                        = int.Parse(csvLineSet["석탄"].ToString());

                    anime[i].data[groupNum].condition[animeNum, (int)Define.AssetData.electricity]
                        = int.Parse(csvLineSet["전기"].ToString());

                    anime[i].data[groupNum].condition[animeNum, (int)Define.AssetData.glass]
                        = int.Parse(csvLineSet["유리"].ToString());

                    anime[i].data[groupNum].condition[animeNum, (int)Define.AssetData.rubber]
                        = int.Parse(csvLineSet["고무"].ToString());

                    anime[i].data[groupNum].condition[animeNum, (int)Define.AssetData.uranium]
                        = int.Parse(csvLineSet["우라늄"].ToString());

                    anime[i].data[groupNum].condition[animeNum, (int)Define.AssetData.semiconductor]
                        = int.Parse(csvLineSet["반도체"].ToString());

                    anime[i].data[groupNum].condition[animeNum, (int)Define.AssetData.mithril]
                        = int.Parse(csvLineSet["미스릴"].ToString());

                    anime[i].data[groupNum].condition[animeNum, (int)Define.AssetData.floatingStone]
                        = int.Parse(csvLineSet["부유석"].ToString());

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
        int allEnding0 = 0;
        for (int i = 0; i < endingValues.Length; i++) allEnding0 += endingValues[i];
        if (allEnding0 >= 100) return;

        for (int j = 0; j < anime.Length; j++)
        {
            for (int k = 0; k < anime[j].data.Length; k++)
            {
                if (anime[j].data[k].bundle.activeSelf == false) continue;// 예전에 사용됨 (다음 걸로 )

                if (k - 1 != -1)
                    if (anime[j].data[k - 1].bundle.activeSelf == true) continue; // 전에 트리가 아직 작동하지 않는다면?

                for (int a = 0; a < anime[j].data[k].model.Length; a++)
                {
                    if (anime[j].data[k].model[a].activeSelf == true) continue; // 지금 등장한 연출 (스킵)

                    for (int n = 0; n < 12; n++)
                    {
                        if (anime[j].data[k].condition[a, n] > factoryLvCheck[n]) break; // 조건이 아직 부족함

                        if (n == 11)
                        {
                            for (int ni = 0; ni < 12; ni++)
                            {
                                Debug.Log(anime[j].name + anime[j].data[k].condition[a, ni] + " " + factoryLvCheck[ni]);
                            }
                            //조건 충족
                            anime[j].data[k].model[a].SetActive(true);
                            Managers.Sound.SfxPlay(Define.SoundClipName.eventTrigger);
                            endingValues[(int)anime[j].ending] += anime[j].data[k].Influence[a];

                            int allEnding1 = 0;
                            int _ending = 0;
                            int big = 0;
                            bool normal = true;
                            for (int i = 0; i < endingValues.Length; i++)
                            {
                                allEnding1 += endingValues[i];
                            }

                            if (allEnding1 > 100)
                            {
                                endingValues[(int)anime[j].ending] += 100 - allEnding1;
                            }

                            for (int i = 0; i < endingValues.Length; i++)
                            {
                                if (big < endingValues[i])
                                {
                                    big = endingValues[i];
                                    _ending = i;
                                    //가장 큰 엔딩
                                }
                                if (endingValues[i] < 23)
                                {
                                    normal = false;
                                    //노멀 엔딩 아님
                                }
                            }

                            if (allEnding1 >= 100)
                            {
                                ActionTimer(4,()=> 
                                {
                                    endingAnime.SetBool("Ending", true);
                                    endingAnimeText.SetActive(true);

                                    if (normal == false)
                                    {
                                        endingAnime.SetInteger("Num", _ending);
                                        endingType = _ending;
                                    }
                                    else
                                    {
                                        endingAnime.SetInteger("Num", 4);
                                        endingType = 4;
                                    }
                                });
                            }


                            if (k - 1 != -1)
                                if (anime[j].data[k - 1].bundle.activeSelf == true)
                                {
                                    anime[j].data[k - 1].bundle.GetComponent<Animator>().SetBool("Start", true);
                                    ActionTimer(4, () => anime[j].data[k - 1].bundle.SetActive(false));
                                }
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

        for (int i = 0; i < viewFactory.Length; i++)
        {
            viewFactory[i].upgrade = new int[viewFactory[i].maxLv, 12];
            foreach (var csvLineSet in csvData)
            {

                if (viewFactory[i].name == csvLineSet["업그레이드 요소"].ToString())
                {

                    viewFactory[i].upgrade[int.Parse(csvLineSet["LV"].ToString()) - 1, (int)Define.AssetData.wood]
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

    public void PlayInputFactoryItem(Define.AssetData _factoryType, int[] _Assets)
    {
        for (int i = 0; i < _Assets.Length; i++)
        {
            InputFactoryItem(_factoryType, (Define.AssetData)i, _Assets[i]);
        }
    }

    //공장에 아이템 넣기
    private void InputFactoryItem(Define.AssetData factoryType, Define.AssetData itemType, int count)
    {
        //아이템 증가
        factoryScript[factoryType].asset[(int)itemType] += count;


        //업그레이드 정보 확인
        for (int i = factoryScript[factoryType].data.lv; i < factoryScript[factoryType].data.maxLv; i++)
        {
            bool checkLvUp = true;
            for (int j = 0; j < 12; j++)
            {
                if (factoryScript[factoryType].data.upgrade[i, j] > factoryScript[factoryType].asset[j])
                {
                    checkLvUp = false;
                    break;
                }
            }

            if (checkLvUp == true)
            {
                factoryScript[factoryType].data.lv += 1;
                //레벨이 오른만큼 아이템 삭제
                for (int j = 0; j < 12; j++)
                {
                    factoryScript[factoryType].asset[j] -= factoryScript[factoryType].data.upgrade[i, j];
                }
            }
        }
        //연출 조건 확인 및 실행
        if (factoryLvCheck[(int)factoryType] != factoryScript[factoryType].data.lv)
        {
            factoryLvCheck[(int)factoryType] = factoryScript[factoryType].data.lv;
            PlayAnimation();
        }


    }

    public void SetWorkbechPoint(Vector3[] pos, bool _pointA)
    {
        if (_pointA) { _workbenchPointsA = pos; }
        else { _workbenchPointsB = pos; }
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

    [field: SerializeField]
    public GameObject group { get; set; }

    public Anime[] data;


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