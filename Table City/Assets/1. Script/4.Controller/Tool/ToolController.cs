using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolController : GrabableObject
{
    private void Start()
    {
        _type = Define.CastingType.Tool;
        base.Init();
    }

    public override void Interact(VRController interactedHand, Transform target)
    {
        base.Interact(interactedHand, target);
    }

    public override void ExitInteract()
    {
        base.ExitInteract();
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
