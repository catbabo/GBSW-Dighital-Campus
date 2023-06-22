using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : GrabableObject
{
    [SerializeField]
    private Define.ResourseType _resourceType;

    private void Start()
    {
<<<<<<< HEAD
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
=======
        _type = Define.CastingType.Resource;
        gameObject.SetActive(true);
        base.Init();
>>>>>>> Resource_Grab_BeforeFix
    }

    public override void Interact(VRController interactedHand, Transform target)
    {
        gameObject.SetActive(true);
        base.Interact(interactedHand, target);
    }

    public override void ExitInteract()
    {
        base.ExitInteract();
        gameObject.SetActive(false);
        transform.position = _originPos;
        transform.rotation = _originRot;
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnCollisionExit(Collision collision)
    {

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