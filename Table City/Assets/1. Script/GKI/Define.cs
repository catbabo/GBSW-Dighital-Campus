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

    /// <summary> �˾� ������ enum </summary>
    public enum PopupState
    {
        /// <summary> �÷��̾ ��ٸ��� ���� </summary>
        Wait,
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

    public struct ResourceObject
    {
        public GameObject gameObject;
        public Transform transform;
        public bool _isGrab;

        public void Grab(VRController hand, Transform target)
        {
            if(!_isGrab)
            {
                hand.ImplusiveGrab(transform);
            }
        }

        public bool IsGrab()
        {
            return _isGrab;
        }
    }

}
