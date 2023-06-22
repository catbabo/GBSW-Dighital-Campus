using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
{

	// 게임 버전
	private string _gameVersion = "1";

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

	#region PlayerServerInfo
	// 들어갈 방 이름
	public string _roomCode { get; private set; } = "12345";

	// 닉네임
	public string _nickName { get; private set; } = "Admin";

	// 플레이어가 A 포인트에 생성될 여부
	// true : 플레이어 A 포인트에서 소환 , false : 플레이어 B 포인트에서 소환
	private bool _pointA;
	#endregion

	/// <summary>개발자모드 사용 여부 (true : 개발자모드 사용 , false : 개발자모드 해제)</summary>
	private bool _onDevelopMode = false;

	/// <summary>네트워크 세팅 초기화</summary>
	private void InitNetworkSetting()
	{
		PhotonNetwork.GameVersion = this._gameVersion; // 게임 버전 설정 ( 버전이 같은 사람끼리만 매칭이 가능함 )
		PhotonNetwork.SendRate = 60; // 초당 패키지를 전송하는 횟수
		PhotonNetwork.SerializationRate = 30; // 초당 OnPhotonSerialize를 실행하는 횟수
		PhotonNetwork.AutomaticallySyncScene = true; // PhotonNetwork.LoadLevel을 사용하였을 때 모든 참가자를 동일한 레벨로 이동하게 하는지의 여부
	}

	private void Update()
	{
		// D, M을 동시에 누르면 개발자모드 사용 또는 해제
		if(Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.M)) { _onDevelopMode = !_onDevelopMode; print("개발자 모드 : " + _onDevelopMode); }
		if (_onDevelopMode) { DevelopMode(); }
	}

	#region SetPlayerServerInfo
	// 플레이어 닉네임 설정
	public void SetNickName(string _name) => _nickName = _name;

	// 입장할 방 이름 설정
	public void SetRoomCode(string _code) => _roomCode = _code;
	#endregion

	#region Connect
	// 서버 연결
	public void Connect() => PhotonNetwork.ConnectUsingSettings();

	// 마스터 서버에 접속 하면 실행
	public override void OnConnectedToMaster()
	{
		print("마스터 서버 연결 성공!");

		PhotonNetwork.LocalPlayer.NickName = _nickName;
	}
	#endregion

	#region Lobby
	// 로비 접속
	public void JoinLobby()
	{
		PhotonNetwork.JoinLobby();
	}
	
	// 로비에 접속 하면 실행
	public override void OnJoinedLobby()
	{
		print("로비 접속 완료.");

		JoinOrCreate(_roomCode, 2);
	}
	#endregion

	#region Room
	// 방 입장 방이 없으면 생성 ( 방 이름, 입장 가능 인원수)
	public void JoinOrCreate(string _name, int _maxPlayer)
				=> PhotonNetwork.JoinOrCreateRoom(_name, new RoomOptions { MaxPlayers = _maxPlayer }, null); // ( 방 이름, 방 옵션, 로비 타입 )

	// 방이 만들어지면 실행
	public override void OnCreatedRoom()
	{
		print("방 만들기 완료.");
	}

	// 만들어진 방에 들어가면 실행
	public override void OnJoinedRoom()
	{
		print("방 입장 성공!");
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

	#region Spawn
	// 오브젝트 소환 ( 오브젝트 파일 위치 (Resources 에서 소환함), 소환될 위치, 마스터 서버에서 소환될 위치(소환 위치가 상관 없다면 안적어도 됨))
	public void SpawnObject(string _objectName, Transform _point)
	{
		GameObject _object = PhotonNetwork.Instantiate(_objectName, _point.position, _point.rotation);
	}

	// 플레이어 소환
	public void SpawnPlayer()
	{
		SpawnObject("0. Player/Player_Prefab", _pointA ? RoomManager.room._PlayerPointA : RoomManager.room._PlayerPointB);
		SpawnObject("0. Player/Player_Workbench", _pointA ? RoomManager.room._WorkbenchPointA : RoomManager.room._WorkbenchPointB);
	}
	#endregion

	#region Disconnect
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
	#endregion

	// 플레이어가 선택한 위치 저장
	public void SetPlayerSpawnPoint(bool _point) => _pointA = _point;

	// 방에 들어온 플레이어의 수를 리턴
	// public void SetJoinRoomPlayerCount(TMP_Text _text) => _text.text = "Player : " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

	// 서버 정보 출력
	// 방에 들어가 있다면 방 이름, 방 인원수, 방 최대 인원수, 방에 있는 플레이어 목록 출력
	// 그렇지 않으면 접속한 인원 수, 방 개수, 모든 방에 있는 인원 수, 로비에 있는지의 여부, 연결이 됐는지의 여부
	public void Info()
	{
		if (PhotonNetwork.InRoom)
		{
			Debug.Log("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
			Debug.Log("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
			Debug.Log("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

			string playerStr = "방에 있는 플레이어 목록 : ";
			for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
			Debug.Log(playerStr);
		}
		else
		{
			Debug.Log("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
			Debug.Log("방 개수 : " + PhotonNetwork.CountOfRooms);
			Debug.Log("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
			Debug.Log("로비에 있는지? : " + PhotonNetwork.InLobby);
			Debug.Log("연결됐는지? : " + PhotonNetwork.IsConnected);
		}
	}

	// 개발자 모드
	// 대부분 앞자리를 따와서 제작함
	// 키코드 : 'C'onnect, lo'B'by, 'I'nfo, 'L'eave , 'J'oin, 'D'isconnect
	private void DevelopMode()
	{

		// B를 누르면 로비 입장
		if (Input.GetKeyDown(KeyCode.B)) JoinLobby();

		// C를 누르면 마스터 서버로 입장
		if (Input.GetKeyDown(KeyCode.C)) Connect();

		// D를 누르면 서버 연결 해제
		if (Input.GetKeyDown(KeyCode.D)) DisConnect();

		// I를 누르면 서버 정보 호출
		if (Input.GetKeyDown(KeyCode.I)) Info();

		// J을 누르면 방 입장
		if (Input.GetKeyDown(KeyCode.J)) JoinOrCreate(_roomCode, 2);

		// L을 누르면 방 떠나기
		if (Input.GetKeyDown(KeyCode.L)) LeaveRoom();

	}

}
