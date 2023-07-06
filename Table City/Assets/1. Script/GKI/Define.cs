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
        /// <summary> 플레이어가 모두 들어온 상태  </summary>
        MaxPlayer,
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
        rubber,
        coal,
        glass,
        uranium,
        mithril,
        stone,
        cloth,
        steel,
        electricity,
        semiconductor,
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
    public enum Point
    {
        Left,
        Right
    }

    public struct ResourceObject
    {
        public GameObject gameObject;
        public Transform transform;
        public ResourceController resource;
        public bool _isGrab;

        public void Init(Transform target, AssetData type)
        {            
            transform = gameObject.transform;
            transform.SetParent(target);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            
            resource = gameObject.GetComponent<ResourceController>();
            resource.SetResourceType(type);

            gameObject.SetActive(false);
        }

        public void Grab(VRController hand, Transform target)
        {
            if (!_isGrab)
            {
                hand.ImplusiveGrab<ResourceController>(transform);
            }
        }

        public bool IsGrab()
        {
            return _isGrab;
        }
    }

}
