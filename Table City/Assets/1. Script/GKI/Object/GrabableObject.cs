using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableObject : ObjectBase
{
    protected bool _isGrab;
    protected Transform _grapPoint;
    protected Vector3 _originPos;
    protected Quaternion _originRot;
    protected bool _isMine;

    public override void Init()
    {
        _isInteracting = false;
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
        //if (!_isMine)
        //return;

        _isGrab = false;
        _isInteracting = false;
    }
}
