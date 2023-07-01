using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
    private bool _isHeadInit, _IsMine;
    private Transform _head, _headPivot;

    private OVRControllerHelper _leftHelper, _rightHelper;

    public void InitHead(Transform target, bool mine)
    {
        _head = transform.Find("Male Head");
        _headPivot = target;
        _isHeadInit = true;
        _IsMine = mine;
    }

	private void Update()
    {
        if (!_isHeadInit && _IsMine)
            return;

        _head.position = _headPivot.position;
        _head.rotation = _headPivot.rotation;
    }

	public void InitControllerHelper()
    {
        _leftHelper = transform.Find("OVRControllerPrefab_Left").GetComponent<OVRControllerHelper>();
        _rightHelper = transform.Find("OVRControllerPrefab_Right").GetComponent<OVRControllerHelper>();
    }

    public void SetControllerParent(Transform left, Transform right)
    {
        //_leftHelper.transform.parent = left;
        //_rightHelper.transform.parent = right;

        //_leftHelper.transform.localRotation = Quaternion.identity;
        //_rightHelper.transform.localRotation = Quaternion.identity;

        _leftHelper.transform.GetComponent<VRController>().SetFollowObject(left);
        _rightHelper.transform.GetComponent<VRController>().SetFollowObject(right);
    }

    public OVRControllerHelper[] GetControllerHelper()
    {
        OVRControllerHelper[] helpers = { _leftHelper, _rightHelper};
        return helpers;        
    }
}
