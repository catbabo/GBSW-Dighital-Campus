using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.EventSystems;

public class VRController : MonoBehaviour
{
    #region Elements
    [field: SerializeField]
    public bool _isRight { get; set; }
    [SerializeField]
    private bool _isTesting;

    private Transform _toolGrabPoint;
    private bool _isGrab;

    public LineRenderer _laser;
    public Color _laserColor;

    private OVRInput.RawButton _triggerButton;
    public OVRInput.Axis2D _thumbStick;

    private Transform _castedObject;

    private ObjectBase _castedComponent;

    private Define.CastingType _targetType;

    private Transform _followTarget;
    private bool _isFollowInit;
    #endregion

    private Vector3 _origin;
    private Vector3 _dir;
    private float _rayLength = 20f;
    private RaycastHit _hit;
    private Transform _hitTransform;
    private GameObject cursorVisual;
    private void Start()
    {
        if (_isTesting)
            return;

        SetButton();
        _toolGrabPoint = transform.Find("ToolGrabPoint");
    }

    private void SetButton()
    {
        if (_isRight)
        {
            _triggerButton = OVRInput.RawButton.RIndexTrigger;
            _thumbStick = OVRInput.Axis2D.SecondaryThumbstick;
        }
        else
        {
            _triggerButton = OVRInput.RawButton.LIndexTrigger;
            _thumbStick = OVRInput.Axis2D.PrimaryThumbstick;
        }
    }

    private void Update()
    {
        if (_isTesting)
            return;

        ControllerCycle();

        if (_isFollowInit)
        {
            transform.position = _followTarget.position;
            transform.rotation = _followTarget.rotation;
        }
    }

    public void SetFollowObject(Transform target)
    {
        _isFollowInit = true;
        _followTarget = target;
    }

    private void ControllerCycle()
    {
        _dir = transform.forward;
        _origin = transform.position;

        if (OVRInput.GetDown(_triggerButton))
        { GetDownTrigger(); }

        if (OVRInput.GetUp(_triggerButton))
        { GetUpTrigger(); }

        if (_isGrab)
            return;

        if (IsHitRay())
        {
            _hitTransform = _hit.transform;

            ObjectCasting();
        }
        else
        {
            if (_castedObject != null)
            {
                ExitCasting();
            }
        }
    }

    private bool IsHitRay(int layer = -1)
    {
        if (layer == -1)
        {
            return Physics.Raycast(_origin, _dir, out _hit, _rayLength);
        }
        return Physics.Raycast(_origin, _dir, out _hit, _rayLength, layer);
    }

    private void ObjectCasting()
    {
        _castedComponent = null;
        if (_hitTransform.TryGetComponent(out _castedComponent))
        {
            _targetType = _castedComponent._type;
        }
        _castedObject = _hitTransform;
    }

    private void ExitCasting()
    {
        if (_targetType == Define.CastingType.Tool)
        {
        }
        _castedObject = null;
    }

    private void GetDownTrigger()
    {
        if (_castedObject == null)
            return;

        EnterInteract();
    }

    private void EnterInteract()
    {
        if (_castedComponent == null)
            return;


        switch (_targetType)
        {
            case Define.CastingType.Tool:
            case Define.CastingType.PlayerBox:
                {
                    _castedComponent.Interact(this, _toolGrabPoint);
                    _isGrab = true;
                    break;
                }
        }
    }

    private void GetUpTrigger()
    {
        ExitInteract();
    }

    public void ExitInteract(bool isInterrupt = false)
    {
        if (_castedComponent == null)
            return;

        switch (_targetType)
        {
            case Define.CastingType.Tool:
            case Define.CastingType.PlayerBox:
            case Define.CastingType.Resource:
                {
                    _isGrab = false;
                    break;
                }
        }

        if (!isInterrupt)
            _castedComponent.ExitInteract();

        _castedObject = null;
    }

    public void Interrupt()
    {
        ExitInteract(true);
    }

    public void ImplusiveGrab<T>(Transform target) where T : ObjectBase
    {
        ExitInteract();
        _castedObject = target;
        _castedComponent = _castedObject.GetComponent<T>();
        _targetType = _castedComponent._type;
        _isGrab = true;
        _castedComponent.Interact(this, _toolGrabPoint);
    }
}
