using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Ray _ray;
    private float _rayLength = 100f;
    private LineRenderer _laser;
    private RaycastHit _target;
    private GameObject currentObject;
    public LayerMask LayerUI;

    private Transform _leftHand;
    private Transform _rightHand;

    private void Start()
    {
        _laser = gameObject.AddComponent<LineRenderer>();

        Material material = new Material(Shader.Find("Standard"));
        material.color = new Color(0, 195, 255, 0.5f);

        _laser.material = material;
        _laser.positionCount = 2;
        _laser.startWidth = 0.01f;
        _laser.endWidth = 0.01f;
    }

    private void Update()
    {
        _laser.SetPosition(0, transform.position);
        _ray.direction = transform.forward;
        _ray.origin = transform.position;

        Debug.DrawRay(_ray.origin, _ray.direction * _rayLength, Color.green, 0.5f);

        if (Physics.Raycast(_ray.origin, _ray.direction, out _target, _rayLength, LayerUI))
        {
            _laser.SetPosition(1, _target.point);

            if (_target.collider.CompareTag("Button"))
            {
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    _target.collider.GetComponent<Button>().onClick.Invoke();
                }

                else
                {
                    _target.collider.GetComponent<Button>().OnPointerEnter(null);
                    currentObject = _target.collider.gameObject;
                }
            }
        }
        else
        {
            _laser.SetPosition(1, transform.position + (transform.forward * _rayLength));
            if (currentObject != null)
            {
                currentObject.GetComponent<Button>().OnPointerExit(null);
                currentObject = null;
            }

        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            _laser.material.color = Color.white;
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            _laser.material.color = new Color(0, 195, 255, 0.5f);
        }
    }
}
