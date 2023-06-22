using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : ObjectBase
{

    private bool _isGrab;
    private Transform _grapPoint;
    private Vector3 _originPos;
    private Quaternion _originRot;
    private bool _isMine;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        _isInteracting = false;
        _type = Define.CatingType.Tool;
        _originPos = transform.position;
        _originRot = transform.rotation;

        PhotonView pv = null;
        pv = GetComponent<PhotonView>();
        _isMine = (pv.IsMine);
    }

    public override void Interact(VRController interactedHand, Transform target)
    {
        if (!_isMine)
            return;

        if (_isInteracting)
        {
            _interactedHand.Interrupt();
        }

        _interactedHand = interactedHand;
        _grapPoint = target;
        _isGrab = true;
        _isInteracting = true;
    }

    public override void ExitInteract()
    {
        if (!_isMine)
            return;

        _isGrab = false;
        transform.position = _originPos;
        transform.rotation = _originRot;
        _isInteracting = false;
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
