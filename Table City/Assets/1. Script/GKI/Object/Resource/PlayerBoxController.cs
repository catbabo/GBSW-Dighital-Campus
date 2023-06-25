using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Todo
 * 
1. GM��  Asset ����ü�� Enum�� Dfine���� �ű���
2. ������ ������ �����غ��� (struct���� int�� �迭��)
3. �÷��̾ ���� ��� �������� ��� �� �� �ֳ�?
4. ������ �ڽ����� �ڿ��� �����ϸ� �����Ѱ�?
5. 
 * 
 * */

public class PlayerBoxController : ObjectBase
{
    private bool _isMine;
    [SerializeField]
    private GameObject _resource;
    [SerializeField]
    private Define.ResourseType _resourseType;
    private string _sourceParentPath = "ResourceItems/", _sourcePath;
    private GameObject _resourcePrefab, _resourceInstant;

    private string[] _resourceNameA = {"Wood", "Iron", "Paper", "Glass", "Uranium", "Missrill" };
    private string[] _resourceNameB= {"Stone", "Coal", "Bettery", "Rubber", "Semiconductor", "FlyStone"};

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
                }
                break;
        }

        bool isPlayerATeam = true;
        if(isPlayerATeam)
        {
            _sourcePath = _sourceParentPath + _resourceNameA[(int)_resourseType];
        }
        else
        {
            _sourcePath = _sourceParentPath + _resourceNameB[(int)_resourseType];
        }
        _resourcePrefab = Resources.Load<GameObject>(_sourcePath);
        _resourceInstant = Instantiate(_resourcePrefab, target);
        _resourceInstant.transform.localPosition = Vector3.zero;
        _resourceInstant.transform.localRotation = Quaternion.identity;
    }

    private void OnGrabResource()
    {

    }

    public override void ExitInteract()
    {

    }
}
