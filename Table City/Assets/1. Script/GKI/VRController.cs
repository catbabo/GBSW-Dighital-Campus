using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class VRController : MonoBehaviour
{
    private Vector3 _origin;
    private Vector3 _dir;
    private float _rayLength = 100f;
    private RaycastHit _hit;
    public LayerMask LayerUI;

    private LineRenderer _laser;
    private Color _laserColor;

    private Transform _selectedObject;
    private Define.CatingType _targetType;

    private Button _button;
    private InputField _inputField;

    private void Start()
    {
        SetLaser();

        _dir = transform.forward;
        _origin = transform.position;

        _inputField.ActivateInputField();
    }

    private void SetLaser()
    {
        _laser = gameObject.AddComponent<LineRenderer>();

        Material material = new Material(Shader.Find("Standard"));

        _laserColor = Color.cyan;

        _laser.material = material;
        _laser.positionCount = 2;
        _laser.startWidth = 0.01f;
        _laser.endWidth = 0.01f;
    }

    private void Update()
    {
        //Debug.DrawRay(_origin, _dir * _rayLength, Color.green, 0.5f);

        if (IsHitRay())
        {
            DrawLaser(_hit.point);
            ObjectCasted();
        }
        else
        {
            DrawLaser(transform.position + (transform.forward * _rayLength));

            if (_selectedObject != null)
            {
                ExitInteract();
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        { GetDownTrigger(); }

        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        { GetUpTrigger(); }

    }

    private void GetDownTrigger()
    {
        SetLaserColor(Color.white);

        if (_selectedObject == null)
            return;

        EnterInteract();
    }

    private void GetUpTrigger()
    {
        SetLaserColor(Color.cyan);
    }

    private void ObjectCasted()
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
            _targetType = Define.CatingType.Object;
        }
        _selectedObject = _hit.transform;
    }

    private void EnterInteract()
    {
        if (_targetType == Define.CatingType.Button)
        {
            _button.onClick.Invoke();
        }

        if (_targetType == Define.CatingType.InputField)
        {
            _inputField.ActivateInputField();
        }

        if (_targetType == Define.CatingType.Object)
        {

        }
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

        if (_targetType == Define.CatingType.Object)
        {

        }
        _selectedObject = null;
    }

    private bool IsHitRay(int layer = -1)
    {
        if(layer == -1)
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

    private void SetLaserColor(Color color)
    {
        _laserColor = color;
        _laserColor.a = 0.5f;
        _laser.material.color = _laserColor;
    }    
}
