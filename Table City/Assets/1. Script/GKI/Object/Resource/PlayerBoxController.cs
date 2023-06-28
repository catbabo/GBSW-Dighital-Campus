using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerBoxController : ObjectBase
{
    private bool _isMine;
    [SerializeField]
    private GameObject _resource;
    [SerializeField]
    private Define.AssetData _resourseType;

    private string _sourceParentPath = "ResourceItems/", _sourcePath;
    private Define.ResourceObject[] _resourceInstant = new Define.ResourceObject[2];

    private string[] _resourceName = { "Wood", "Stone", "Steel", "Cloth", "Coal", "Electricity",
        "Glass", "Rubber", "Uranium", "Semiconductor",  "Mithril","FloatingStone" };


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

        if (_isMine)
        {
            /*bool isPlayerATeam = NetworkManager.Net.IsPlayerTeamA();
            if (isPlayerATeam)
            {
                _sourcePath = _sourceParentPath + _resourceNameA[(int)_resourseType];
            }
            else
            {
                _sourcePath = _sourceParentPath + _resourceNameB[(int)_resourseType];
            }*/
            _sourcePath = _sourceParentPath + _resourceName[(int)_resourseType];
            GameObject _resourcePrefab = Resources.Load<GameObject>(_sourcePath);

            _resourceInstant[0].gameObject = Instantiate(_resourcePrefab, transform.position, Quaternion.identity);
            _resourceInstant[0].Init(transform, _resourseType);
            _resourceInstant[1].gameObject = Instantiate(_resourcePrefab, transform.position, Quaternion.identity);
            _resourceInstant[1].Init(transform, _resourseType);
        }
    }

    public override void Interact(VRController interactedHand, Transform target)
    {
        if (!_isMine)
            return;

        /*if (Managers.system.asset[(int)_resourseType] <= 0)
            return;*/

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
