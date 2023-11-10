using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class GameManager : ManagerBase
{
    [field: SerializeField, Header("���� ���ϸ��̼�")]
    public Animator endingAnime { set; get; }
    [SerializeField, Header("���� �ؽ�Ʈ")]
    private GameObject endingAnimeText;
    public int endingType { set; get; }


    [field: SerializeField, Header("���� ���൵")]
    public int[] endingValues = new int[4];
    public int allEndingValue = 0;

    [field: SerializeField, Header("�ڿ�")]
    public int[] asset { get; set; } = new int[Define.RESOURCES_COUNT];

    public string[] countText { get; set; } = new string[2];

    [Header("���� ���� �Է� �뵵")]
    public Factory[] viewFactory;
    [Header("����")]
    public AnimeBundle[] anime;

    public Dictionary<Define.AssetData, FactoyData> factoryScript = new Dictionary<Define.AssetData, FactoyData>(); //���� ��ũ��Ʈ�� ��� ����
    [field: SerializeField, Header("���� ���� ��Ȳ")]
    private int[] factoryLvCheck = new int[Define.RESOURCES_COUNT]; // ���� ���� ��Ȳ Ȯ�� �뵵

    public Vector3[] _workbenchPointsA { get; private set; } = new Vector3[6];
    public Vector3[] _workbenchPointsB { get; private set; } = new Vector3[6];

    public void ActionTimer(float time, Action action)
    { StartCoroutine(ActionTimerCoroutine(time, action)); }

    private IEnumerator ActionTimerCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }


    public override void Init()
    {
        //SetFactory();
        //SetAnimation();
    }

    // ���� �ʱ� ���� #���� �Ľ�
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
                        anime[i].data[groupNum].condition = new int[anime[i].data[groupNum].model.Length, Define.RESOURCES_COUNT];
                        backGroupNum = groupNum;
                    }

                    anime[i].data[groupNum].Influence[animeNum] = int.Parse(csvLineSet["�����"].ToString().Replace("%", ""));

                    for(int j = 0; j < Define.RESOURCES_COUNT; j++)
                    {
                        string resource = Define.Resources[j];
                        anime[i].data[groupNum].condition[animeNum, j] = int.Parse(csvLineSet[resource].ToString());
                    }
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

    // ���� Ȯ�� ���� Ȯ�� �� ����
    private void PlayAnimation()
    {
        int allEnding0 = 0;

        for (int i = 0; i < endingValues.Length; i++)
            allEnding0 += endingValues[i];

        if (allEnding0 >= 100)
            return;

        for (int j = 0; j < anime.Length; j++)
        {
            for (int k = 0; k < anime[j].data.Length; k++)
            {
                if (anime[j].data[k].bundle.activeSelf == false) continue;// ������ ���� (���� �ɷ� )

                if (k - 1 != -1)
                    if (anime[j].data[k - 1].bundle.activeSelf == true) continue; // ���� Ʈ���� ���� �۵����� �ʴ´ٸ�?

                for (int a = 0; a < anime[j].data[k].model.Length; a++)
                {
                    if (anime[j].data[k].model[a].activeSelf == true) continue; // ���� ������ ���� (��ŵ)

                    for (int n = 0; n < Define.RESOURCES_COUNT; n++)
                    {
                        if (anime[j].data[k].condition[a, n] > factoryLvCheck[n]) break; // ������ ���� ������

                        if (n == 11)
                        {
                            for (int ni = 0; ni < Define.RESOURCES_COUNT; ni++)
                            {
                                Debug.Log(anime[j].name + anime[j].data[k].condition[a, ni] + " " + factoryLvCheck[ni]);
                            }
                            //���� ����
                            anime[j].data[k].model[a].SetActive(true);
                            Managers.Sound.SfxPlay("event");
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
                                    //���� ū ����
                                }
                                if (endingValues[i] < 23)
                                {
                                    normal = false;
                                    //��� ���� �ƴ�
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

    // ���� �ʱ� ����
    private void SetFactory()
    {
        List<Dictionary<string, object>> csvData = CSVReader.Read("3.Csv/Upgrade");
        //viewFactory = new Factory[Define.Factory_COUNT];
        for (int i = 0; i < viewFactory.Length; i++)
        {
            foreach (var csvLineSet in csvData)
            {
                if (Define.Resources[i]+" ����" == csvLineSet["���׷��̵� ���"].ToString())
                {
                    int lv = int.Parse(csvLineSet["LV"].ToString());

                    viewFactory[i].upgrade = new int[lv, Define.RESOURCES_COUNT];
                    for (int j = 0; j < Define.RESOURCES_COUNT; j++)
                    {
                        int value = int.Parse(csvLineSet[Define.Resources[j]].ToString());
                        viewFactory[i].upgrade[lv - 1, j] = value;
                    }
                }
            }
            //��ũ��Ʈ �ֱ�
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

    //���忡 ������ �ֱ�
    private void InputFactoryItem(Define.AssetData factoryType, Define.AssetData itemType, int count)
    {
        //������ ����
        factoryScript[factoryType].asset[(int)itemType] += count;


        //���׷��̵� ���� Ȯ��
        for (int i = factoryScript[factoryType].data.lv; i < factoryScript[factoryType].data.maxLv; i++)
        {
            bool checkLvUp = true;
            for (int j = 0; j < Define.RESOURCES_COUNT; j++)
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
                //������ ������ŭ ������ ����
                for (int j = 0; j < Define.RESOURCES_COUNT; j++)
                {
                    factoryScript[factoryType].asset[j] -= factoryScript[factoryType].data.upgrade[i, j];
                }
            }
        }
        //���� ���� Ȯ�� �� ����
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
    public GameObject bundle { get; set; } // ���� �׷�

    [field: SerializeField]
    public GameObject[] model { get; set; } // ��ȯ ��

    [field: SerializeField]
    public int[] Influence { get; set; } // �����

    public int[,] condition { get; set; }  // ����
}

[System.Serializable]
public struct Factory
{
    [field: SerializeField]
    public string name { get; set; }//���� �̸�
    [field: SerializeField]
    public GameObject model { get; set; }//���� ��
    [field: SerializeField]
    public int lv { get; set; }//���� LV
    [field: SerializeField]
    public int maxLv { get; set; }//�߽� Lv
    public int[,] upgrade { get; set; } // Lv �� ���׷��̵� ����
    [field: SerializeField]
    public Define.AssetData createAsset { get; set; } // ���� Type
}