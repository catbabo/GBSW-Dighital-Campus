using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DataForms
{


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
}
