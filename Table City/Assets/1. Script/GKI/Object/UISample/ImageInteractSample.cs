using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageInteractSample : ObjectBase
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        _isInteracting = false;
    }

    public override void Interact(VRController interactedHand)
    {
        if (_isInteracting)
        {
            _interactedHand.Interrupt();
        }

        _interactedHand = interactedHand;
        _isInteracting = true;

        Debug.Log("Enter Image Interact");
    }

    public override void ExitInteract()
    {
        Debug.Log("Exit Image Interact");
        _isInteracting = false;
    }
}
