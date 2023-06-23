using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkPlayer : MonoBehaviourPunCallbacks
{

	/// <summary> ���� �� </summary>
	[SerializeField] private PhotonView _PV;
	/// <summary> �÷��̾� �г��� �ؽ�Ʈ </summary>
	[SerializeField] private TMP_Text _NickNameTMP;
	/// <summary> �÷��̾��� �Ӹ� ������Ʈ </summary>
	[SerializeField] private GameObject _PlayerHead;

	private void Start()
	{
		SetNick();
		SetHeadLayer();
	}

	/// <summary> �÷��̾� �г��� �ؽ�Ʈ�� �÷��̾� �г��� ���� </summary>
	private void SetNick() => _NickNameTMP.text = _PV.IsMine ? PhotonNetwork.NickName : _PV.Owner.NickName;

	/// <summary> �÷��̾��� �Ӹ� ������Ʈ ���̾� ���� </summary>
	private void SetHeadLayer() => _PlayerHead.layer = _PV.IsMine ? LayerMask.NameToLayer("Head_IsMine") : LayerMask.NameToLayer("Head");
}
