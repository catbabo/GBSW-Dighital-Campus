using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class NetworkManager : MonoBehaviourPunCallbacks
{
	private string _gameVersion = "1";
	public string _roomCode { get; private set; }
	public string _nickName { get; private set; }

	/// <summary>
	/// 플레이어가 A 포인트에 생성될 여부
	/// true : 플레이어 A 포인트에서 소환
	/// false : 플레이어 B 포인트에서 소환
	/// </summary>
	private bool _pointA;

	private bool _onDevelopMode = false;

	/// <summary>
	/// 강제로 방에서 나와졌는지의 여부
	///  true : 강제로 나와졌음
	///  false : 강제로 나오지 않음
	/// </summary>
	public bool _forceOut { get; private set; } = false;
	private PhotonView _pv;

	public void Init()
	{
        _pv = GetComponent<PhotonView>();
        // 게임 버전 설정 ( 버전이 같은 사람끼리만 매칭이 가능함 )
        PhotonNetwork.GameVersion = _gameVersion;
        // 초당 패키지를 전송하는 횟수
        PhotonNetwork.SendRate = 60;
        // 초당 OnPhotonSerialize를 실행하는 횟수
        PhotonNetwork.SerializationRate = 30;
        // PhotonNetwork.LoadLevel을 사용하였을 때 모든 참가자를 동일한 레벨로 이동하게 하는지의 여부
        PhotonNetwork.AutomaticallySyncScene = true;
	}

	private void Update()
	{
		if(Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.M))
		{
			_onDevelopMode = !_onDevelopMode;
			Debug.Log("개발자 모드 : " + _onDevelopMode);
		}

		if (!_onDevelopMode)
			return;

		Cheat();
    }

    private void Cheat()
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

        // S를 누르면 기본 셋팅
        if (Input.GetKeyDown(KeyCode.S)) EnterRoomSolo();

        // T를 누르면 혼자서 입장
        if (Input.GetKeyDown(KeyCode.T)) PhotonNetwork.LoadLevel("PlayRoom");
    }

	public void SetNickName(string _name) { _nickName = _name; }

	public void SetRoomCode(string _code) { _roomCode = _code; }

	public void Connect() { PhotonNetwork.ConnectUsingSettings(); }

	public override void OnConnectedToMaster()
	{
		Debug.Log("마스터 서버 연결 성공!");

		PhotonNetwork.LocalPlayer.NickName = _nickName;
	}

	public void JoinLobby()
	{
		PhotonNetwork.JoinLobby();
	}

	public override void OnJoinedLobby()
	{
		Debug.Log("로비 접속 완료.");

		JoinOrCreate(_roomCode, 2);
	}

	public void JoinOrCreate(string code, int _maxPlayer)
	{
		// ( 방 이름, 방 옵션, 로비 타입 )
		PhotonNetwork.JoinOrCreateRoom(code, new RoomOptions { MaxPlayers = _maxPlayer }, null);
	}

	public override void OnCreatedRoom() { Debug.Log("방 만들기 완료."); }

	public override void OnJoinedRoom() { Debug.Log("방 입장 성공!"); }

    public override void OnLeftRoom() { Debug.Log("방 퇴장 성공!"); }

    public void LeaveRoom() { PhotonNetwork.LeaveRoom(); }

	public void DisConnect()
	{
		if (PhotonNetwork.IsConnected) { PhotonNetwork.Disconnect(); }
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("서버 연결 해제");
		Debug.Log(cause);
	}

    public void SetPlayerSpawnPoint(bool _point) { _pointA = _point; }

	public void SetForceOut(bool _force) { _forceOut = _force; }

    private void Info()
	{
		if (PhotonNetwork.InRoom)
		{
			Debug.Log("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
			Debug.Log("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
			Debug.Log("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

			string playerStr = "방에 있는 플레이어 목록 : ";
			
			for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
				playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";

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

	private void EnterRoomSolo()
	{
		SetNickName("admin");
		SetRoomCode("DevelopRoom");
		SetPlayerSpawnPoint(true);
	}

    public void LoadScene(string _sceneName) { PhotonNetwork.LoadLevel(_sceneName); }

    public void OutRoom_GoMain()
    {
        PhotonNetwork.LeaveRoom();
        DisConnect();
        LoadScene("MainLobby");
    }

	public bool IsPlayerTeamA() { return _pointA; }

    public void SyncSpawnObejct(Define.prefabType _type, string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle, Define.AssetData _assetType)
    {
        if (Define.prefabType.effect == _type)
        {
            _pv.RPC("SpawnEffect", RpcTarget.All, _objName, _spawnPoint, _spawnAngle, _assetType);
        }
    }

    [PunRPC]
    private void SpawnEffect(string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle, Define.AssetData _assetType)
    {
        GameObject _object = Managers.Instance.UsePoolingObject(Define.prefabType.effect + _objName, _spawnPoint, _spawnAngle);
        if (_objName == "truck")
        {
            _object.GetComponent<Throw>().SetTargetPosition(AssetManager._asset.GetTargetPosition((int)_assetType));
        }
    }
}
