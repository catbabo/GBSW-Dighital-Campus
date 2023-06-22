using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Define : MonoBehaviour
{
    public enum CastingType
    {
        Tool, Resource, InputBox, PlayerBox
    }

    public enum ResourseType
    {
        Wood, Stone,
        Iron, Coal,
        Paper, Bettery,
        Glass, Rubber,
        Uranium, Semiconductor,
        Missrill, FlyStone
    }
}
