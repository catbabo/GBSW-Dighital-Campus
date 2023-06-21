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

    public Asset asset;
    public AnimeBundle[] anime;


}
[System.Serializable]
public struct Asset
{
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
    [SerializeField]
    private string endingName;

    [SerializeField]
    private GameObject group;

    [SerializeField]
    private Anime[] data;

    [System.Serializable]
    public struct Anime
    {
        [SerializeField]
        private GameObject bundle;

        [SerializeField]
        private GameObject[] model;


        [SerializeField]
        private Asset[] condition;
    }
}

