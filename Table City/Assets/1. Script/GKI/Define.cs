using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Define : MonoBehaviour
{
    public enum CatingType
    {
        Tool,
        Button,
        InputField,
        Text,
        Image,
        Scrollbar
    }



    public struct HandInfo
    {
        public Transform _controller;

        public LineRenderer _laser;
        public Color _laserColor;

        public Transform _toolGrabPoint;
        public bool _isGrab, _isTouch;
        public OVRInput.RawButton _triggerButton;

        public Transform _castedObject;
        public ObjectBase _castedComponent;
        public Define.CatingType _targetType;

        public Button _button;
        public InputField _inputField;

        public void Init()
        {
            _laser = _controller.GetComponent<LineRenderer>();

            Material material = new Material(Shader.Find("Standard"));

            _laserColor = Color.cyan;
            SetLaserColor(_laserColor);

            _laser.material = material;
            _laser.positionCount = 2;
            _laser.startWidth = 0.01f;
            _laser.endWidth = 0.01f;

            _toolGrabPoint = _controller.Find("ToolGrabPoint");
        }

        public void SetController(Transform controller)
        {
            _controller = controller;
        }

        public void SetLaserColor(Color color)
        {
            _laserColor = color;
            _laserColor.a = 0.5f;
            _laser.material.color = _laserColor;
        }

        public void DrawLaser(Vector3 destnation)
        {
            _laser.SetPosition(0, _controller.position);
            _laser.SetPosition(1, destnation);
        }

        public void LaserEnable(bool enable)
        {
            _laser.enabled = enable;
        }
    }
}
