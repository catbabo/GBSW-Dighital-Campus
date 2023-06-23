using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkPlayer : MonoBehaviourPunCallbacks
{

	/// <summary> 포톤 뷰 </summary>
	[SerializeField] private PhotonView _PV;
	/// <summary> 플레이어 닉네임 텍스트 </summary>
	[SerializeField] private TMP_Text _NickNameTMP;
	/// <summary> 플레이어의 머리 오브젝트 </summary>
	[SerializeField] private GameObject _PlayerHead;

	private void Start()
	{
		SetNick();
		SetHeadLayer();
	}

	/// <summary> 플레이어 닉네임 텍스트에 플레이어 닉네임 적용 </summary>
	private void SetNick() => _NickNameTMP.text = _PV.IsMine ? PhotonNetwork.NickName : _PV.Owner.NickName;

	/// <summary> 플레이어의 머리 오브젝트 레이어 변경 </summary>
	private void SetHeadLayer() => _PlayerHead.layer = _PV.IsMine ? LayerMask.NameToLayer("Head_IsMine") : LayerMask.NameToLayer("Head");
}
