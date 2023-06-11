using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectBase : MonoBehaviour
{
    public Define.CatingType _type;
    protected Transform _interactedHand;

    public Button Button
    {
        get
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
            return _button;
        }
        private set { }
    }
    private Button _button;


    public InputField InputField
    {
        get
        {
            if (_inputField == null)
            {
                _inputField = GetComponent<InputField>();
            }
            return _inputField;
        }
        private set { }
    }
    private InputField _inputField;

    public virtual void Init() { }

    public virtual void Interact(Transform interactedHand) { }
    public virtual void Interact(Transform interactedHand, Transform target) { }

    public virtual void ExitInteract() { }
}
