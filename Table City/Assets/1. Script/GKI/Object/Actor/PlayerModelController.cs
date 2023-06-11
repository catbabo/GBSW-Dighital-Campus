using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
    private Transform _headPivot;
    private Transform _head;
    private bool _isInitHead;

    private OVRControllerHelper _leftHelper, _rightHelper;

    public void InitHead(Transform target)
    {
        _headPivot = target;
        _head = transform.Find("Male Head");
        _head.parent = _headPivot;
        _isInitHead = true;
    }

    public void InitControllerHelper()
    {
        _leftHelper = transform.Find("OVRControllerPrefab_Left").GetComponent<OVRControllerHelper>();
        _rightHelper = transform.Find("OVRControllerPrefab_Right").GetComponent<OVRControllerHelper>();
    }

    public void SetControllerParent(Transform left, Transform right)
    {
        _leftHelper.transform.parent = left;
        _rightHelper.transform.parent = right;
    }

    public OVRControllerHelper[] GetControllerHelper()
    {
        OVRControllerHelper[] helpers = { _leftHelper, _rightHelper};
        return helpers;        
    }
}
