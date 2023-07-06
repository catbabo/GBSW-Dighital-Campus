using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoxController : ObjectBase
{
    private bool _isMine, _isInit;
    private GameObject _viewItem, _resourcePrefab;
    public Define.AssetData _resourseType;

    private string
        _sourceParentPath = "ResourceItems/",
        _sourcePath;

    private Define.ResourceObject[] _resourceInstant = new Define.ResourceObject[2];

    private string[] _resourceName = { "Wood", "Rubber", "Coal", "Glass", "Uranium", "Mithril",
        "Stone", "Paper", "Steel", "Electricity","Semiconductor", "FloatingStone" };

    private void Update()
    {
        if (!_isInit)
            return;

        if (Managers.system.asset[(int)_resourseType] < 1)
            _viewItem.SetActive(false);
        else
            _viewItem.SetActive(true);
    }

    public void Init(GameObject viewItem, bool isMine, Define.AssetData type)
    {
        _viewItem = viewItem;
        _isMine = isMine;
        _resourseType = type;
        Init();
    }

    public override void Init()
    {
        _isInteracting = false;
        _type = Define.CastingType.PlayerBox;

        _sourcePath = _sourceParentPath + _resourceName[(int)_resourseType];
        _resourcePrefab = Resources.Load<GameObject>(_sourcePath);

        //_resourceInstant[0].gameObject = Photon.Pun.PhotonNetwork.Instantiate(_sourcePath, transform.position, Quaternion.identity);
        _resourceInstant[0].gameObject = Instantiate(_resourcePrefab, transform.position, Quaternion.identity);
        _resourceInstant[0].Init(transform, _resourseType);
        _resourceInstant[1].gameObject = Instantiate(_resourcePrefab, transform.position, Quaternion.identity);
        //_resourceInstant[1].gameObject = Photon.Pun.PhotonNetwork.Instantiate(_sourcePath, transform.position, Quaternion.identity);
        _resourceInstant[1].Init(transform, _resourseType);

        _isInit = true;
    }

    public override void Interact(VRController interactedHand, Transform target)
    {
        if (!_isMine)
            return;

        if (Managers.system.asset[(int)_resourseType] <= 0)
            return;

        Managers.system.asset[(int)_resourseType]--; //°¨¼Ò

        if (!_resourceInstant[0].IsGrab())
        {
            _resourceInstant[0].Grab(interactedHand, target);
        }
        else if (!_resourceInstant[1].IsGrab())
        {
            _resourceInstant[1].Grab(interactedHand, target);
        }
    }
}
