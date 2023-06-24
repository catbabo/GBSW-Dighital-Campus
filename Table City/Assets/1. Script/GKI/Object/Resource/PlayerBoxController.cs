using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoxController : ObjectBase
{
    private bool _isMine;
    [SerializeField]
    private GameObject _resource;
    [SerializeField]
    private Define.ResourseType _resourseType;
    private string _sourceParentPath = "ResourceItems/", _sourcePath;
    private GameObject _resourcePrefab, _resourceInstant;


    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        _isInteracting = false;
        _type = Define.CastingType.PlayerBox;

        PhotonView pv = null;
        pv = GetComponent<PhotonView>();
        _isMine = (pv.IsMine);
    }

    public override void Interact(VRController interactedHand, Transform target)
    {
        if (!_isMine)
            return;

        switch (_resourseType)
        {
            case Define.ResourseType.Wood:
                if(Managers.system.asset.wood > 0)
                {
                    _sourcePath = _sourceParentPath + "Wood";
                }
                break;
        }

        _resourcePrefab = Resources.Load<GameObject>(_sourcePath);
        _resourceInstant = Instantiate(_resourcePrefab, target);
        _resourceInstant.transform.localPosition = Vector3.zero;
        _resourceInstant.transform.localRotation = Quaternion.identity;
    }

    public override void ExitInteract()
    {

    }
}
