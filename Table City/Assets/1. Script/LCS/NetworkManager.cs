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

	// 서버 연결
	public void Connect() 
				=> PhotonNetwork.ConnectUsingSettings();

	// 마스터 서버에 접속 하면 실행
	public override void OnConnectedToMaster()
	{
		SceneManager.LoadScene("PlayRoom");
		JoinOrCreate(_roomCode);
	}

	// 플레이어 닉네임 설정
	public void NickNameSet(string _name) 
				=> PhotonNetwork.LocalPlayer.NickName = _name;

	// 방 입장 또는 생성 ( 방 이름, 방 옵션 (현재는 옵션 고정), 로비 타입)
	public void JoinOrCreate(string _name) 
				=> PhotonNetwork.JoinOrCreateRoom(_name, new RoomOptions { MaxPlayers = 2 }, null);

	// 오브젝트 소환
	public void SpawnObject(string objectName, Transform MarsterPoint, Transform CommonPoint)
	{
		GameObject _object = PhotonNetwork.Instantiate(objectName,
							 PhotonNetwork.IsMasterClient ? MarsterPoint.position : CommonPoint.position, 
							 PhotonNetwork.IsMasterClient ? MarsterPoint.rotation : CommonPoint.rotation);
	}

	// 만들어진 방에 들어가면 실행
	public override void OnJoinedRoom()
	{
		Transform[] _sp = new Transform[2];

		GameObject _spParent = GameObject.Find("SpawnPoint");

		_sp[0] = _spParent.transform.GetChild(0);
		_sp[1] = _spParent.transform.GetChild(1);

		SpawnObject("0. Player/PlayerPrefab", _sp[0], _sp[1]);
	}

	// 서버 연결 해제
	public void DisConnect()
	{
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
		}
	}

	// 서버 연결 해제시 실행
	public override void OnDisconnected(DisconnectCause cause)
	{
		
	}

}
