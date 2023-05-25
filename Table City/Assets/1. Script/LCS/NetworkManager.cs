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

	// 서버 연결
	public void Connect() 
				=> PhotonNetwork.ConnectUsingSettings();

	// 마스터 서버에 접속 하면 실행
	public override void OnConnectedToMaster()
	{
		
	}

	// 플레이어 닉네임 설정
	public void NickNameSet(string _name) 
				=> PhotonNetwork.LocalPlayer.NickName = _name;

	// 방 입장 또는 생성 ( 방 이름, 방 옵션, 로비 타입)
	public void JoinOrCreate(string _name, RoomOptions _roomOption = null, TypedLobby _typedLobby = null) 
				=> PhotonNetwork.JoinOrCreateRoom(_name, new RoomOptions { MaxPlayers = 2 }, _typedLobby);

	// 오브젝트 소환
	public void SpawnObject(string objectName, Transform MarsterPoint, Transform CommonPoint)
	{
		GameObject _object = PhotonNetwork.Instantiate(objectName,
							 PhotonNetwork.IsMasterClient ? MarsterPoint.position : CommonPoint.position, 
							 PhotonNetwork.IsMasterClient ? MarsterPoint.rotation : CommonPoint.rotation);
		PhotonView _pv = _object.GetComponent<PhotonView>();
	}

	// 만들어진 방에 들어가면 실행
	public override void OnJoinedRoom()
	{
		SceneManager.LoadScene("PlayRoom");
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
