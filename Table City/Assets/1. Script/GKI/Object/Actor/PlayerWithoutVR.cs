using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWithoutVR : MonoBehaviour
{
    #region Elements

    private VRController _vrController;

    private Transform _castedObject;

    private ObjectBase _castedComponent;

    private Define.CatingType _targetType;

    private Button _button;

    private InputField _inputField;

    private Ray _ray;
    #endregion

    private float _rayLength = 100f;
    private RaycastHit _hit;
    private Transform _hitTransform;

    private void Start()
    {
        _vrController = GetComponent<VRController>();
    }

    private void Update()
    {
        ControllerCycle();
    }

    private void ControllerCycle()
    {
        if (Input.GetMouseButtonDown(0))
        { GetDownTrigger(); }

        if (Input.GetMouseButtonUp(0))
        { GetUpTrigger(); }

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
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (layer == -1)
        {
            return Physics.Raycast(_ray, out _hit, _rayLength);
        }
        return Physics.Raycast(_ray, out _hit, _rayLength, layer);
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
        if (_targetType == Define.CatingType.Button)
        {
            _button = null;
        }

        if (_targetType == Define.CatingType.InputField)
        {
            _inputField = null;
        }

        if (_targetType == Define.CatingType.Tool)
        {
            Debug.Log("Exit");
        }

        if (_targetType == Define.CatingType.Image)
        {
            Debug.Log("Exit");
        }

        if (_targetType == Define.CatingType.Text)
        {
            Debug.Log("Exit");
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
            Debug.Log("Interact");
        }

        if (_targetType == Define.CatingType.Text)
        {
            _castedComponent.Interact(_vrController);
        }

        if (_targetType == Define.CatingType.Image)
        {
            _castedComponent.Interact(_vrController);
        }

        if (_targetType == Define.CatingType.Scrollbar)
        {
            _castedComponent.Interact(_vrController);
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

        if (_targetType == Define.CatingType.Button)
        {
            _button = _castedComponent.Button;
            _button = null;
        }

        if (_targetType == Define.CatingType.InputField)
        {
            _inputField = null;
        }

        if (_targetType == Define.CatingType.Tool)
        {
        }

        if (!isInterrupt)
            _castedComponent.ExitInteract();

        _castedObject = null;
    }

    public void Interrupt()
    {
        ExitInteract(true);
    }
}
