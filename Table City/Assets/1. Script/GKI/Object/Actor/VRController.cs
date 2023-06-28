using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class VRController : MonoBehaviour
{
    #region Elements
    [SerializeField]
    private bool _isRight, _isTesting;

    private Transform _toolGrabPoint;
    private bool _isGrab;

    public LineRenderer _laser;
    public Color _laserColor;

    private OVRInput.RawButton _triggerButton;

    private Transform _castedObject;

    private ObjectBase _castedComponent;

    private Define.CastingType _targetType;

    private Transform _followTarget;
    private bool _isFollowInit;
    #endregion

    private Vector3 _origin;
    private Vector3 _dir;
    private float _rayLength = 100f;
    private RaycastHit _hit;
    private Transform _hitTransform;

    private void Start()
    {
        if (_isTesting)
            return;

        SetLaser();
        SetButton();
    }

    private void SetLaser()
    {
        _laser = transform.GetComponent<LineRenderer>();

        Material material = new Material(Shader.Find("Standard"));

        _laserColor = Color.cyan;
        SetLaserColor(_laserColor);

        _laser.material = material;
        _laser.positionCount = 2;
        _laser.startWidth = 0.01f;
        _laser.endWidth = 0.01f;

        _toolGrabPoint = transform.Find("ToolGrabPoint");
    }

    public void SetLaserColor(Color color)
    {
        _laserColor = color;
        _laserColor.a = 0.5f;
        _laser.material.color = _laserColor;
    }

    public void DrawLaser(Vector3 destnation)
    {
        _laser.SetPosition(0, transform.position);
        _laser.SetPosition(1, destnation);
    }

    public void LaserEnable(bool enable)
    {
        _laser.enabled = enable;
    }

    private void SetButton()
    {
        if (_isRight)
        {
            _triggerButton = OVRInput.RawButton.RIndexTrigger;
        }
        else
        {
            _triggerButton = OVRInput.RawButton.LIndexTrigger;
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
            DrawLaser(_hit.point);
            ObjectCasting();
        }
        else
        {
            DrawLaser(transform.forward * _rayLength);

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
        SetLaserColor(Color.white);

        if (_castedObject == null)
            return;

        EnterInteract();
    }

    private void EnterInteract()
    {
        if (_castedComponent == null)
            return;


        switch(_targetType)
        {
            case Define.CastingType.Tool:
            case Define.CastingType.PlayerBox:
            {
                _castedComponent.Interact(this, _toolGrabPoint);
                _isGrab = true;
                LaserEnable(false);
                break;
            }
        }
    }

    private void GetUpTrigger()
    {
        ExitInteract();
        SetLaserColor(Color.cyan);
    }

    public void ExitInteract(bool isInterrupt = false)
    {
        if (_castedComponent == null)
            return;

        if (_targetType == Define.CastingType.Tool)
        {
            _isGrab = false;
            LaserEnable(true);
        }

        if(!isInterrupt)
            _castedComponent.ExitInteract();

        _castedObject = null;
    }

    public void Interrupt()
    {
        ExitInteract(true);
    }

    public void ImplusiveGrab(Transform target)
    {
        ExitInteract();
        _castedObject = target;
        _castedComponent = _castedObject.GetComponent<ObjectBase>();
        _targetType = _castedComponent._type;
        _isGrab = true;
        _castedComponent.Interact(this, _toolGrabPoint);
    }
}
