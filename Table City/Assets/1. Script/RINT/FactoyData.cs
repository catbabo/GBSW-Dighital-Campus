using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoyData : MonoBehaviour
{
    public Factory data;

    [field:Header("����� �ڿ�"),SerializeField]
    public int[] asset { get; set; } = new int[12];


}
