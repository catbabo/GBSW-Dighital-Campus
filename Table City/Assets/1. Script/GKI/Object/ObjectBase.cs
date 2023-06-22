using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectBase : MonoBehaviour
{
    protected bool _isInteracting;
    public Define.CatingType _type;
    protected VRController _interactedHand;

    public virtual void Init() { }

    public virtual void Interact(VRController interactedHand) { }
    public virtual void Interact(VRController interactedHand, Transform target) { }

    public virtual void ExitInteract() { }
}
