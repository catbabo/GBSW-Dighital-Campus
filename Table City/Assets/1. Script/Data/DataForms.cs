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
}
