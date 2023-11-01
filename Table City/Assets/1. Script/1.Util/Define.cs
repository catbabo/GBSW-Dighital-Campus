using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const int RESOURCES_COUNT = 12;
    public const int Factory_COUNT = 12;

    public static string[] Resources = { "����", "��", "��", "õ", "ö", "��ź", "����", "����", "�ݵ�ü", "���", "�̽���", "������" };

    public enum Scene
    {
        Title, Lobby, Room, Load
    }

    public enum PlayerBehaviourState
    {
        StartApplication, Title, CreateLobby, CreateRoom, 
    }

    public enum CastingType
    {
        Tool, Resource, InputBox, PlayerBox
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
