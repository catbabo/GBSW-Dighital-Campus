using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const int RESOURCES_COUNT = 12;
    public const int Factory_COUNT = 12;

    public static string[] Resources = { "나무", "돌", "고무", "천", "철", "석탄", "전기", "유리", "반도체", "우라늄", "미스릴", "부유석" };

    public enum Scene
    {
        Title, Lobby, Room, InGame, Load
    }

    public enum PlayerBehaviourState
    {
        StartApplication, Lobby, InRoom, Ready, SelectJob, InGame
    }

    public enum CastingType
    {
        Tool, Resource, InputBox, PlayerBox
    }

    /// <summary> 팝업 상태의 enum </summary>
    public enum PopupState
    {
        Wait,
        Ready,
        ReadyPlease,
        SelectJob,
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
        wood = 0,
        stone,
        rubber,
        cloth,
        steel,
        coal,
        electricity,
        glass,
        semiconductor,
        uranium,
        mithril,
        floatingStone
    }

    public enum Ending
    {
        floatingIsland,
        future,
        space,
        Destruction
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
            resource.Init(this);
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
