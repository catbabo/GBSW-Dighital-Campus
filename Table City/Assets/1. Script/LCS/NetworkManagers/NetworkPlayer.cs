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
	[SerializeField] private PhotonView _PV;
	// �÷��̾��� �г��� �ؽ�Ʈ
	[SerializeField] private TMP_Text _NickNameTMP;
	// �÷��̾��� �Ӹ� ������Ʈ
	[SerializeField] private GameObject _PlayerHead;

	private void Start()
	{
		SetNick();
		SetHeadLayer();
	}

	// �÷��̾��� �г��� �ؽ�Ʈ�� �г��� �ֱ�
	private void SetNick() => _NickNameTMP.text = _PV.IsMine ? PhotonNetwork.NickName : _PV.Owner.NickName;

	// �÷��̾��� �Ӹ� ������Ʈ�� ���̾� �ٲٱ�
	private void SetHeadLayer() => _PlayerHead.layer = _PV.IsMine ? LayerMask.NameToLayer("Head_IsMine") : LayerMask.NameToLayer("Head");
}
