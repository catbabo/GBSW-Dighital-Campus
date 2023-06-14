using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInteractSample : ObjectBase
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

        Debug.Log("Enter Text Interact");
    }

    public override void ExitInteract()
    {
        Debug.Log("Exit Text Interact");
        _isInteracting = false;
    }
}
