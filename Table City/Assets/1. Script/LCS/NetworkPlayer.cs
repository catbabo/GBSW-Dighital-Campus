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
	public PhotonView _PV;
	// 플레이어의 닉네임 텍스트
	public TMP_Text _NickNameTMP;
	// 플레이어의 오디오 리스너
	public AudioListener _AL;

	private void Start()
	{
		SetNick();
	}

	private void SetNick()
	{
		_NickNameTMP.text = _PV.IsMine ? PhotonNetwork.NickName : _PV.Owner.NickName;
	}
}
