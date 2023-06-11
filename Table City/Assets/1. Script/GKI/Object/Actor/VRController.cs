using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VRController : MonoBehaviour
{
    #region Elements
    private Define.HandInfo LController, RController, _nowController;
    [SerializeField]
    private Transform _lController, _rController;

    private Transform _toolGrabPoint
    {
        get { return _nowController._toolGrabPoint; }
        set { _nowController._toolGrabPoint = value; }
    }

    private bool _isGrab
    {
        get { return _nowController._isGrab; }
        set { _nowController._isGrab = value; }
    }

    private bool _isTouch
    {
        get { return _nowController._isTouch; }
        set { _nowController._isTouch = value; }
    }

    private OVRInput.RawButton _triggerButton
    {
        get { return _nowController._triggerButton; }
        set { _nowController._triggerButton = value; }
    }

    private Transform _castedObject
    {
        get { return _nowController._castedObject; }
        set { _nowController._castedObject = value; }
    }

    private ObjectBase _castedComponent
    {
        get { return _nowController._castedComponent; }
        set { _nowController._castedComponent = value; }
    }

    private Define.CatingType _targetType
    {
        get { return _nowController._targetType; }
        set { _nowController._targetType = value; }
    }

    private Button _button
    {
        get { return _nowController._button; }
        set { _nowController._button = value; }
    }

    private InputField _inputField
    {
        get { return _nowController._inputField; }
        set { _nowController._inputField = value; }
    }
    #endregion

    private Vector3 _origin;
    private Vector3 _dir;
    private float _rayLength = 100f;
    private RaycastHit _hit;
    private Transform _hitTransform;

    private void Start()
    {
        LController._controller = _lController;
        RController._controller = _rController;

        SetLaser();
        SetButton();
    }

    private void SetLaser()
    {
        LController.Init();
        RController.Init();
    }

    private void SetButton()
    {
        RController._triggerButton = OVRInput.RawButton.RIndexTrigger;
        LController._triggerButton = OVRInput.RawButton.LIndexTrigger;
    }

    private void Update()
    {
        SyncWithController(RController);
        ControllerCycle();

        SyncWithController(LController);
        ControllerCycle();
    }

    private void SyncWithController(Define.HandInfo controller)
    {
        _nowController = controller;

        _dir = _nowController._controller.forward;
        _origin = _nowController._controller.position;
    }

    private void ControllerCycle()
    {
        if (OVRInput.GetDown(_triggerButton))
        { GetDownTrigger(); }

        if (OVRInput.GetUp(_triggerButton))
        { GetUpTrigger(); }

        if (_isGrab)
            return;

        if (IsHitRay())
        {
            _hitTransform = _hit.transform;
            _nowController.DrawLaser(_hit.point);
            ObjectCasting();
        }
        else
        {
            _nowController.DrawLaser(transform.forward * _rayLength);

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
        ObjectBase castedComponent;

        if (_hitTransform.TryGetComponent(out castedComponent))
        {
            _castedComponent = castedComponent;
            _targetType = _castedComponent._type;
        }
        _castedObject = _hitTransform;
        _castedComponent = _castedComponent;
    }

    private void ExitCasting()
    {
        if (_targetType == Define.CatingType.Button)
        {
            _button.OnPointerExit(null);
            _button = null;
        }

        if (_targetType == Define.CatingType.InputField)
        {
            _inputField = null;
        }

        if (_targetType == Define.CatingType.Tool)
        {

        }
        _castedObject = null;
    }

    private void GetDownTrigger()
    {
        _nowController.SetLaserColor(Color.white);

        if (_castedObject == null)
            return;

        EnterInteract();
    }

    private void EnterInteract()
    {
        if (_castedComponent == null)
            return;

        if (_targetType == Define.CatingType.Button)
        {
            _button = _castedComponent.Button;
            _button.onClick.Invoke();
        }

        if (_targetType == Define.CatingType.InputField)
        {
            _inputField = _castedComponent.InputField;
            _inputField.ActivateInputField();
        }

        if (_targetType == Define.CatingType.Tool)
        {
            _castedComponent.Interact(_toolGrabPoint);
            _isGrab = true;
            _nowController.LaserEnable(false);
        }
    }

    private void GetUpTrigger()
    {
        ExitInteract();
        _nowController.SetLaserColor(Color.cyan);
    }

    public void ExitInteract()
    {
        if (_castedComponent == null)
            return;

        if (_targetType == Define.CatingType.Button)
        {
            _button = _castedComponent.Button;
            _button.OnPointerExit(null);
            _button = null;
        }

        if (_targetType == Define.CatingType.InputField)
        {
            _inputField = null;
        }

        if (_targetType == Define.CatingType.Tool)
        {
            _castedComponent.ExitInteract();
            _isGrab = false;
            _nowController.LaserEnable(true);
        }
        _castedObject = null;
    }
}
