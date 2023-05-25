using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class NetworkManager : MonoBehaviourPunCallbacks
{

	public static NetworkManager Net = null;

	private void Awake()
	{
		if(Net == null)
		{
			Net = this;

			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}

		PhotonNetwork.SendRate = 60;
		PhotonNetwork.SerializationRate = 30;
	}

	// ���� ����
	public void Connect() 
				=> PhotonNetwork.ConnectUsingSettings();

	// ������ ������ ���� �ϸ� ����
	public override void OnConnectedToMaster()
	{
		
	}

	// �÷��̾� �г��� ����
	public void NickNameSet(string _name) 
				=> PhotonNetwork.LocalPlayer.NickName = _name;

	// �� ���� �Ǵ� ���� ( �� �̸�, �� �ɼ�, �κ� Ÿ��)
	public void JoinOrCreate(string _name, RoomOptions _roomOption = null, TypedLobby _typedLobby = null) 
				=> PhotonNetwork.JoinOrCreateRoom(_name, new RoomOptions { MaxPlayers = 2 }, _typedLobby);

	// ������Ʈ ��ȯ
	public void SpawnObject(string objectName, Transform MarsterPoint, Transform CommonPoint)
	{
		GameObject _object = PhotonNetwork.Instantiate(objectName,
							 PhotonNetwork.IsMasterClient ? MarsterPoint.position : CommonPoint.position, 
							 PhotonNetwork.IsMasterClient ? MarsterPoint.rotation : CommonPoint.rotation);
		PhotonView _pv = _object.GetComponent<PhotonView>();
	}

	// ������� �濡 ���� ����
	public override void OnJoinedRoom()
	{
		SceneManager.LoadScene("PlayRoom");
	}

	// ���� ���� ����
	public void DisConnect()
	{
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
		}
	}

	// ���� ���� ������ ����
	public override void OnDisconnected(DisconnectCause cause)
	{
		
	}

}
