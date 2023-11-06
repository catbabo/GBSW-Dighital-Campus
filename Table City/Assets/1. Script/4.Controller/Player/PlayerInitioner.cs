using Oculus.Interaction.Input.Visuals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitioner : MonoBehaviour
{
    private Transform _headPivot, _models;
    private PlayerModelController _modelController;

    private GameObject _ovrRoot;
    private Transform _controllerRoot;
    private OVRControllerVisual _rightControllerVisual, _leftControllerVisual;

    public void Init(bool isLocal)
    {
        GetModel();
        GetControllerHelper();

        InitOVRSystem(isLocal);
        GetControllerVisual();

        InitControllerHelper();
        InitHead();
    }

    private void GetModel()
    {
        _models = transform.Find("Models");
        _modelController = _models.GetComponent<PlayerModelController>();
    }

    private void GetControllerHelper()
    {
        _modelController.InitControllerHelper();
    }

    private void InitOVRSystem(bool isLocal)
    {
        if (isLocal)
        {
            GameObject _ovrSource = Resources.Load<GameObject>("0. Player/OVR_Systems");
            _ovrRoot = Instantiate(_ovrSource, transform);
        }
        else
        {
            _ovrRoot = Managers.Instance.SpawnObject("0. Player/OVR_Systems");
        }
        _ovrRoot.transform.SetParent(transform);
        _ovrRoot.name = "OVR_Systems";
        _ovrRoot.transform.localPosition = Vector3.zero;
        _ovrRoot.transform.localRotation = Quaternion.identity;
        _controllerRoot = _ovrRoot.transform.Find("OVRInteraction").Find("OVRControllers");
    }

    private void GetControllerVisual()
    {
        _rightControllerVisual = _controllerRoot.Find("RightController").GetComponentInChildren<OVRControllerVisual>();
        _leftControllerVisual = _controllerRoot.Find("LeftController").GetComponentInChildren<OVRControllerVisual>();
    }

    private void InitControllerHelper()
    {
        OVRControllerHelper[] helpers = _modelController.GetControllerHelper();
        _leftControllerVisual.InjectAllOVRControllerHelper(helpers[0]);
        _rightControllerVisual.InjectAllOVRControllerHelper(helpers[1]);

        _modelController.SetControllerParent(_leftControllerVisual.transform, _rightControllerVisual.transform);
    }

    private void InitHead()
    {
        _headPivot = _ovrRoot.transform.Find("TrackingSpace").Find("CenterEyeAnchor");
        //_modelController.InitHead(_headPivot);
    }
}
