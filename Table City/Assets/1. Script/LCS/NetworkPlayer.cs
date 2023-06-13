using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkPlayer : MonoBehaviourPunCallbacks
{

	// �÷��̾��� ���� ��
	public PhotonView _PV;
	// �÷��̾��� �г��� �ؽ�Ʈ
	public TMP_Text _NickNameTMP;
	// �÷��̾��� ����� ������
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
