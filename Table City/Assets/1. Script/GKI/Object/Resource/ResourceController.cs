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
        gameObject.SetActive(true);
        base.Interact(interactedHand, target);
    }

    public override void ExitInteract()
    {
        base.ExitInteract();
        if(_isOnInputBox)
        {
            _inputBox.GetComponent<InputBoxController>().OnDropResource(_resourceType, _count);
        }
        gameObject.SetActive(false);
        transform.position = _originPos;
        transform.rotation = _originRot;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("InputBox"))
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

    private void Update()
    {
        if (_isGrab)
        {
            transform.position = _grapPoint.position;
            transform.rotation = _grapPoint.rotation;
        }
    }
}