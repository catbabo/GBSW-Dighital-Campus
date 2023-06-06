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

	public string _roomCode;

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

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			_roomCode = "1234";
			Connect();
		}
	}

	// ���� ����
	public void Connect() 
				=> PhotonNetwork.ConnectUsingSettings();

	// ������ ������ ���� �ϸ� ����
	public override void OnConnectedToMaster()
	{
		SceneManager.LoadScene("PlayRoom");
		JoinOrCreate(_roomCode);
	}

	// �÷��̾� �г��� ����
	public void NickNameSet(string _name) 
				=> PhotonNetwork.LocalPlayer.NickName = _name;

	// �� ���� �Ǵ� ���� ( �� �̸�, �� �ɼ� (����� �ɼ� ����), �κ� Ÿ��)
	public void JoinOrCreate(string _name) 
				=> PhotonNetwork.JoinOrCreateRoom(_name, new RoomOptions { MaxPlayers = 2 }, null);

	// ������Ʈ ��ȯ
	public void SpawnObject(string objectName, Transform MarsterPoint, Transform CommonPoint)
	{
		GameObject _object = PhotonNetwork.Instantiate(objectName,
							 PhotonNetwork.IsMasterClient ? MarsterPoint.position : CommonPoint.position, 
							 PhotonNetwork.IsMasterClient ? MarsterPoint.rotation : CommonPoint.rotation);
	}

	// ������� �濡 ���� ����
	public override void OnJoinedRoom()
	{
		Transform[] _sp = new Transform[2];

		GameObject _spParent = GameObject.Find("SpawnPoint");

		_sp[0] = _spParent.transform.GetChild(0);
		_sp[1] = _spParent.transform.GetChild(1);

		SpawnObject("0. Player/PlayerPrefab", _sp[0], _sp[1]);
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
