using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class NetworkPlayer : MonoBehaviourPunCallbacks
{
	private PhotonView _PV;
	/// <summary> 플레이어 닉네임 텍스트 </summary>
	[SerializeField] private TMP_Text _NickNameTMP;
	/// <summary> 플레이어의 머리 오브젝트 </summary>
	[SerializeField] private Transform _PlayerHead;

	private void Start()
	{
		_PV = GetComponent<PhotonView>();
		SetNick();
		SetHeadLayer();
	}

	private void SetNick()
	{
		_NickNameTMP.text = _PV.IsMine ? PhotonNetwork.NickName : _PV.Owner.NickName;
	}

	private void SetHeadLayer()
    {
        string layer = "";
        if (_PV.IsMine)
        {
            layer = "Head_IsMine";
        }
        else
        {
            layer = "Head";
		}

        Util.ChangeLayer(_PlayerHead.transform, layer);
	}
}