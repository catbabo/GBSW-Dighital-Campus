using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
    private Transform _headPivot;
    private Transform _head;
    private bool _isInitHead;

    private Transform _leftController, _rightController;

    public void InitHead(Transform target)
    {
        _headPivot = target;
        _head = transform.Find("Male Head");
        _isInitHead = true;
    }

    public void InitControllerHelper()
    {
        _leftController = transform.Find("OVRControllerPrefab_Left");
        _rightController = transform.Find("OVRControllerPrefab_Right");
    }

    public void SetControllerParent(Transform left, Transform right)
    {
        _leftController.GetComponent<VRController>().Init(left);
        _rightController.GetComponent<VRController>().Init(right);
    }

    public OVRControllerHelper[] GetControllerHelper()
    {
        OVRControllerHelper[] helpers = {
            _leftController.GetComponent<OVRControllerHelper>(),
            _rightController.GetComponent<OVRControllerHelper>()
        };
        return helpers;        
    }

    private void Update()
    {
        if (!_isInitHead)
            return;

        _head.position = _headPivot.position;
        _head.rotation = _headPivot.rotation;
    }
}
