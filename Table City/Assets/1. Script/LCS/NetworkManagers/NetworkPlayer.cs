using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkPlayer : MonoBehaviourPunCallbacks
{

	// 플레이어의 포톤 뷰
	[SerializeField] private PhotonView _PV;
	// 플레이어의 닉네임 텍스트
	[SerializeField] private TMP_Text _NickNameTMP;
	// 플레이어의 머리 오브젝트
	[SerializeField] private GameObject _PlayerHead;

	private void Start()
	{
		SetNick();
		SetHeadLayer();
	}

	// 플레이어의 닉네임 텍스트에 닉네임 넣기
	private void SetNick() => _NickNameTMP.text = _PV.IsMine ? PhotonNetwork.NickName : _PV.Owner.NickName;

	// 플레이어의 머리 오브젝트의 레이어 바꾸기
	private void SetHeadLayer() => _PlayerHead.layer = _PV.IsMine ? LayerMask.NameToLayer("Head_IsMine") : LayerMask.NameToLayer("Head");
}
