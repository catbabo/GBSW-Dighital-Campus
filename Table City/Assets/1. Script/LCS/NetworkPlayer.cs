using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkPlayer : MonoBehaviourPunCallbacks, IPunObservable
{

	public PhotonView _PV;
	public TMP_Text _NickNameTMP;

	private void Start()
	{
		SetNick();
		SpawnCamrea();
	}

	private void SetNick()
	{
		_NickNameTMP.text = _PV.IsMine ? PhotonNetwork.NickName : _PV.Owner.NickName;
	}

	private void SpawnCamrea()
	{
		if (!_PV.IsMine)
		{
			Destroy(transform.GetChild(0));
		}

		/*
		if (_PV.IsMine)
		{
			Instantiate(Resources.Load<GameObject>("0. Player/PlayerCamera"), transform.position + Vector3.up, transform.rotation, gameObject.transform);
		}
		*/
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

	}
}
