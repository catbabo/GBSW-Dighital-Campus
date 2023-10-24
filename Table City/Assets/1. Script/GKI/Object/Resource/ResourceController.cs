using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : GrabableObject
{
    [SerializeField]
    private Define.AssetData _resourceType;
    private bool _isOnInputBox;
    private Transform _inputBox;
    private int _count;
    private Define.Point handPos;
    Define.ResourceObject _resourceObject;

    public void Init(Define.ResourceObject resourceObject)
    {
        _resourceObject = resourceObject;
    }

    public void SetResourceType(Define.AssetData type)
    {
        _resourceType = type;
    }

    private void Start()
    {
        _type = Define.CastingType.Resource;
        base.Init();
    }

    public override void Interact(VRController interactedHand, Transform target)
    {
        if (_resourceObject._isGrab)
        {
            return;
        }

        gameObject.SetActive(true);

        if (interactedHand._isRight)
            handPos = Define.Point.Right;
        else
            handPos = Define.Point.Left;


        base.Interact(interactedHand, target);
        
        _count = 1;
        Managers.Game.asset[(int)_resourceType]--; //감소
        Managers.Game.countText[(int)handPos] = _count.ToString();
        _resourceObject._isGrab = true;
    }

    public override void ExitInteract()
    {
        base.ExitInteract();

        Debug.Log("복귀");
        if (_isOnInputBox)
        {
            //아이템 이동
            _inputBox.GetComponent<InputBoxController>().OnDropResource(_resourceType, _count);
        }
        else
        {
            //데이터 원상 복귀
            Managers.Game.asset[(int)_resourceType] += _count;
        }

        //초기화
        _count = 0;

        //메시지 안 보이게 하기
        Managers.Game.countText[(int)handPos] = "";

        _resourceObject._isGrab = false;

        gameObject.SetActive(false);
        transform.position = _originPos;
        transform.rotation = _originRot;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InputBox"))
        {
            _isOnInputBox = true;
            _inputBox = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InputBox"))
        {
            _isOnInputBox = false;
        }
    }

    float deleyTime = 1;
    private void Update()
    {
        if (_isGrab)
        {
            transform.position = _grapPoint.position;
            transform.rotation = _grapPoint.rotation;
        }

        deleyTime += Time.deltaTime * (1+(float)_count/20);
        if (deleyTime > 0.1f)
        {
            float inputStick = 0;
            inputStick = OVRInput.Get(_interactedHand._thumbStick).y;

            if (inputStick > 0.5f)
            {
                if (Managers.Game.asset[(int)_resourceType] > 0)
                {
                    Managers.Game.asset[(int)_resourceType]--;
                    Managers.Sound.SfxPlay("countUp");
                    _count++;
                    //갯수 증가 단 조건
                    deleyTime = 0;
                    Managers.Game.countText[(int)handPos] = _count.ToString();
                }
            }
            else if (inputStick < -0.5f)
            {
                if (_count > 1)
                {
                    Managers.Game.asset[(int)_resourceType]++;
                    Managers.Sound.SfxPlay("countDown");
                    _count--;
                    //갯수 감소 단 조건
                    deleyTime = 0;
                    Managers.Game.countText[(int)handPos] = _count.ToString();
                }
            }
        }

    }
}