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
	#region Singleton
	public static NetworkManager Net = null;
	private void Awake()
	{
		if (Net == null)
		{
			Net = this;

			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
		InitNetworkSetting();
	}
	#endregion

	// 게임 버전
	//private string _gameVersion = "1";

	// 방 이름
	public string _roomCode { get; private set; } = "12345";

	// 닉네임
	public string _nickName { get; private set; } = "Admin";

	private void Update()
	{
		DevelopMode();
	}

	// 네트워크 세팅 초기화
	private void InitNetworkSetting()
	{
		//PhotonNetwork.GameVersion = this._gameVersion;
		PhotonNetwork.SendRate = 60;
		PhotonNetwork.SerializationRate = 30;
	}

	// 개발자 모드
	private void DevelopMode()
	{
		// M을 누르면 마스터 서버로 입장
		if (Input.GetKeyDown(KeyCode.M)) Connect();

		// B를 누르면 로비 입장
		if (Input.GetKeyDown(KeyCode.B)) JoinLobby();

		// I를 누르면 서버 정보 호출
		if (Input.GetKeyDown(KeyCode.I)) Info();

		// L을 누르면 방 떠나기
		if (Input.GetKeyDown(KeyCode.L)) LeaveRoom();

		// R을 누르면 방 입장
		if (Input.GetKeyDown(KeyCode.J)) JoinOrCreate(_roomCode);

		// D를 누르면 서버 연결 해제
		if (Input.GetKeyDown(KeyCode.D)) DisConnect();
	}

	// 서버 연결
	public void Connect() => PhotonNetwork.ConnectUsingSettings();

	// 마스터 서버에 접속 하면 실행
	public override void OnConnectedToMaster()
	{
		print("마스터 서버 연결 성공!");
		PhotonNetwork.LocalPlayer.NickName = _nickName;
		PhotonNetwork.LoadLevel("PlayRoom");
	}

	#region Lobby
	// 로비에 접속하기.
	public void JoinLobby()
	{
		PhotonNetwork.JoinLobby();
	}
	
	// 로비에 접속 하면 실행
	public override void OnJoinedLobby()
	{
		print("로비 접속 완료.");
	}
	#endregion

	#region PlayerInfo
	// 플레이어 닉네임 설정
	public void _SetNickName(string _name) => _nickName = _name;

	// 입장할 방 이름 설정
	public void _SetRoomCode(string _code) => _roomCode = _code;
	#endregion

	#region Room
	// 방 입장 또는 생성 ( 방 이름, 방 옵션 (현재는 입장 인원수 2인 최대로 고정), 로비 타입)
	public void JoinOrCreate(string _name) 
				=> PhotonNetwork.JoinOrCreateRoom(_name, new RoomOptions { MaxPlayers = 2 }, null);

	// 방이 만들어지면 실행
	public override void OnCreatedRoom()
	{
		print("방 만들기 완료.");
	}

	// 만들어진 방에 들어가면 실행
	public override void OnJoinedRoom()
	{
		print("방 입장 성공!");
		SpawnPlayer();
	}

	// 방 퇴장
	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	// 방 퇴장시 실행
	public override void OnLeftRoom()
	{
		print("방 퇴장 성공!");
	}
	#endregion

	// 오브젝트 소환 ( 오브젝트 이름 (Resources 에서 소환함), 소환될 위치, 마스터 서버에서 소환될 위치(소환 위치가 상관 없다면 안적어도 됨))
	public void SpawnObject(string objectName, Transform CommonPoint, Transform MarsterPoint = null)
	{
		if(MarsterPoint == null) { MarsterPoint = CommonPoint; }
		GameObject _object = PhotonNetwork.Instantiate(objectName,
							 PhotonNetwork.IsMasterClient ? MarsterPoint.position : CommonPoint.position, 
							 PhotonNetwork.IsMasterClient ? MarsterPoint.rotation : CommonPoint.rotation);
	}

	// 플레이어 소환
	private void SpawnPlayer()
	{
		SpawnObject("0. Player/PlayerPrefab", RoomManager.room.CommonPoint, RoomManager.room.MasterPoint);
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
		print("서버 연결 해제");
	}

	// 서버 정보
	public void Info()
	{
		if (PhotonNetwork.InRoom)
		{
			print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
			print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
			print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

			string playerStr = "방에 있는 플레이어 목록 : ";
			for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
			print(playerStr);
		}
		else
		{
			print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
			print("방 개수 : " + PhotonNetwork.CountOfRooms);
			print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
			print("로비에 있는지? : " + PhotonNetwork.InLobby);
			print("연결됐는지? : " + PhotonNetwork.IsConnected);
		}
	}
}
