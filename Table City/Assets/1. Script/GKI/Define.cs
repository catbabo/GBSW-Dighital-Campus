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

    /// <summary> �˾� ������ enum </summary>
    public enum PopupState
    {
        /// <summary> �÷��̾ ��ٸ��� ���� </summary>
        Wait,
        /// <summary> �÷��̾ ��� ���� ����  </summary>
        MaxPlayer,
        /// <summary> ������ ����ϴ� ���� </summary>
        Warning,
    }

    public enum prefabType
    {
        building,
        effect,
        other
    }

    public enum AssetData
    {
        wood,
        stone,
        steel,
        cloth,
        coal,
        electricity,
        glass,
        rubber,
        uranium,
        semiconductor,
        mithril,
        floatingStone
    }

    public enum Ending
    {
        floatingIsland,
        future,
        space,
        Destruction,
        normal
    }

}
