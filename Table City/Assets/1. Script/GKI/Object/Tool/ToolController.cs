using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolController : ObjectBase
{
    private bool _isGrab;
    private Transform _grapPoint;
    private Vector3 _originPos;
    private Quaternion _originRot;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        _type = Define.CatingType.Tool;
        _originPos = transform.position;
        _originRot = transform.rotation;
    }

    public override void Interact(Transform interactedHand, Transform target)
    {
        _interactedHand = interactedHand;
        _grapPoint = target;
        _isGrab = true;
    }

    public override void ExitInteract()
    {
        _isGrab = false;
        transform.position = _originPos;
        transform.rotation = _originRot;
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
