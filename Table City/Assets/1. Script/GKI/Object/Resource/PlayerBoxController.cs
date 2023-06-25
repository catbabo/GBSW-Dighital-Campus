using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Todo
 * 
1. GM의  Asset 구조체와 Enum을 Dfine으로 옮기자
2. 데이터 구조를 변경해보자 (struct말고 int형 배열로)
3. 플레이어가 현재 어느 진영인지 어떻게 알 수 있나?
4. 각각의 박스에서 자원을 관리하면 불편한가?
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
