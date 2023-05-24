using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VRController : MonoBehaviour
{
    [SerializeField]
    private bool _isRightController;
    private Vector3 _origin;
    private Vector3 _dir;
    private float _rayLength = 100f;
    private RaycastHit _hit;

    private OVRInput.RawButton _triggerButton;

    private LineRenderer _laser;
    private Color _laserColor;

    private Transform _castedObject;
    private ObjectBase _castedComponent;
    private Define.CatingType _targetType;

    private Transform _toolGrabPoint;

    private Button _button;
    private InputField _inputField;

    private void Start()
    {
        SetLaser();
        SetButton();
        SetToolInfo();

        //_inputField.ActivateInputField();
    }

    private void SetLaser()
    {
        _laser = gameObject.AddComponent<LineRenderer>();

        Material material = new Material(Shader.Find("Standard"));

        _laserColor = Color.cyan;

        SetLaserColor(_laserColor);
        _laser.material = material;
        _laser.positionCount = 2;
        _laser.startWidth = 0.01f;
        _laser.endWidth = 0.01f;
    }

    private void SetLaserColor(Color color)
    {
        _laserColor = color;
        _laserColor.a = 0.5f;
        _laser.material.color = _laserColor;
    }

    private void SetToolInfo()
    {
        _toolGrabPoint = transform.Find("ToolGrabPoint");
    }

    private void SetButton()
    {
        if (_isRightController)
        {
            _triggerButton = OVRInput.RawButton.RHandTrigger;
        }
        else
        {
            _triggerButton = OVRInput.RawButton.LHandTrigger;
        }
    }

    private void Update()
    {
        _dir = transform.forward;
        _origin = transform.position;

        if (IsHitRay())
        {
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

        if (OVRInput.GetDown(_triggerButton))
        { GetDownTrigger(); }

        if (OVRInput.GetUp(_triggerButton))
        { GetUpTrigger(); }

    }

    private bool IsHitRay(int layer = -1)
    {
        if (layer == -1)
        {
            return Physics.Raycast(_origin, _dir, out _hit, _rayLength);
        }
        return Physics.Raycast(_origin, _dir, out _hit, _rayLength, layer);
    }

    private void DrawLaser(Vector3 destnation)
    {
        _laser.SetPosition(0, transform.position);
        _laser.SetPosition(1, destnation);
    }

    /*private void ObjectCasted()
    {
        if (_hit.transform.TryGetComponent(out _button))
        {
            _targetType = Define.CatingType.Button;
            _button.OnPointerEnter(null);
        }
        else if (_hit.transform.TryGetComponent(out _inputField))
        {
            _targetType = Define.CatingType.InputField;
        }
        else
        {
            _targetType = Define.CatingType.Tool;
        }
        _selectedObject = _hit.transform;
    }*/

    private void ObjectCasting()
    {
        if (_hit.transform.TryGetComponent(out _castedComponent))
        {
            _targetType = _castedComponent._type;
        }
        _castedObject = _hit.transform;
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
        SetLaserColor(Color.white);

        if (_castedObject == null)
            return;

        EnterInteract();
    }

    private void EnterInteract()
    {
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
        }
    }

    private void GetUpTrigger()
    {
        ExitInteract();
        SetLaserColor(Color.cyan);
    }

    private void ExitInteract()
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
            _castedComponent.ExitInteract();
        }
        _castedObject = null;
    }

}
