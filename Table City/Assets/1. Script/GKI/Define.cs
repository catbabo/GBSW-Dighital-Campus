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

    /// <summary> 팝업 상태의 enum </summary>
    public enum PopupState
    {
        /// <summary> 플레이어를 기다리는 상태 </summary>
        Wait,
        /// <summary> 위험을 경고하는 상태 </summary>
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
