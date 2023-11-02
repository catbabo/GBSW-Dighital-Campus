using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using PN = Photon.Pun.PhotonNetwork;

[RequireComponent(typeof(PhotonView))]
public class NetworkManager : PunManagerBase
{
	private string _gameVersion = "1";
	public string _roomCode { get; private set; }
	public string _nickName { get; private set; }

	private bool _pointA;

	public bool _forceOut { get; private set; } = false;
	private PhotonView _mainPv;

	[SerializeField]
	private LobbyScene lobby;

	public override void Init()
	{
        _mainPv = GetComponent<PhotonView>();
        // 게임 버전 설정 ( 버전이 같은 사람끼리만 매칭이 가능함 )
        PN.GameVersion = _gameVersion;
        // 초당 패키지를 전송하는 횟수
        PN.SendRate = 60;
        // 초당 OnPhotonSerialize를 실행하는 횟수
        PN.SerializationRate = 30;
        // PhotonNetwork.LoadLevel을 사용하였을 때 모든 참가자를 동일한 레벨로 이동하게 하는지의 여부
        PN.AutomaticallySyncScene = true;

        InitEvent();
		Connect();
    }

    private void InitEvent()
    {
        Managers.Event.AddMatchRoomButton(MatchRoomButton);
        Managers.Event.AddJobButton(JobButton);
    }

    public void Connect()
    {
        PN.ConnectUsingSettings();
    }

    public PhotonView GetPhotonView()
	{
		return _mainPv;
	}

	public void SetNickName(string _name) { _nickName = _name; }

	public void SetRoomCode(string _code) { _roomCode = _code; }

	public override void OnConnectedToMaster()
	{
		Debug.Log("마스터 서버 연결 성공!");
		PN.LocalPlayer.NickName = _nickName;
		JoinLobby();
	}

	public void JoinLobby()
	{
		PN.JoinLobby();
	}

	public override void OnJoinedLobby()
	{
		Debug.Log("로비 접속 완료.");
		//JoinOrCreate();
	}

	public void JoinOrCreate()
	{
        PN.JoinOrCreateRoom(_roomCode, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
	}

	public override void OnCreatedRoom()
	{ Debug.Log("방 만들기 완료."); }

	public override void OnJoinedRoom()
	{
		Debug.Log("방 입장 성공!");
		lobby.OnJoinedRoom();
        _mainPv.RPC("JoinPlayer", RpcTarget.All);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " 참가.");
        _mainPv.RPC("JoinPlayer", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
		Debug.Log(otherPlayer.NickName + " 나감.");
    }

    public override void OnLeftRoom()
	{ Debug.Log("방 퇴장 성공!"); }

    public void LeaveRoom()
	{ PN.LeaveRoom(); }

	public void DisConnect()
	{
		if (PN.IsConnected)
		{ PN.Disconnect(); }
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("서버 연결 해제\n"+cause);
	}

    public void SetPlayerJob(bool _point) { _pointA = _point; }

	public bool IsPlayerTeamA() { return _pointA; }

    public void SetForceOut(bool _force) { _forceOut = _force; }

    public void Info()
	{
		if (PN.InRoom)
		{
			Debug.Log("현재 방 이름 : " + PN.CurrentRoom.Name);
			Debug.Log("현재 방 인원수 : " + PN.CurrentRoom.PlayerCount);
			Debug.Log("현재 방 최대인원수 : " + PN.CurrentRoom.MaxPlayers);

			string playerStr = "방에 있는 플레이어 목록 : ";
			
			for (int i = 0; i < PN.PlayerList.Length; i++)
				playerStr += PN.PlayerList[i].NickName + ", ";

			Debug.Log(playerStr);
		}
		else
			{
				Debug.Log("접속한 인원 수 : " + PN.CountOfPlayers);
			Debug.Log("방 개수 : " + PN.CountOfRooms);
			Debug.Log("모든 방에 있는 인원 수 : " + PN.CountOfPlayersInRooms);
			Debug.Log("로비에 있는지? : " + PN.InLobby);
			Debug.Log("연결됐는지? : " + PN.IsConnected);
		}
	}

	public void EnterRoomSolo()
	{
		SetNickName("admin");
		SetRoomCode("DevelopRoom");
		SetPlayerJob(true);
	}

    public void OutRoom_GoMain()
    {
        PN.LeaveRoom();
        Managers.Scene.LoadScene(Define.Scene.Lobby);
        //DisConnect();
    }

    public void SyncSpawnObejct(Define.prefabType _type, string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle, Define.AssetData _assetType)
    {
        if (Define.prefabType.effect == _type)
        {
            _mainPv.RPC("SpawnEffect", RpcTarget.All, _objName, _spawnPoint, _spawnAngle, _assetType);
        }
    }

    [PunRPC]
    private void SpawnEffect(string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle, Define.AssetData _assetType)
    {
        GameObject _object = Managers.Instance.UsePoolingObject(Define.prefabType.effect + _objName, _spawnPoint, _spawnAngle);
        if (_objName == "truck")
        {
            _object.GetComponent<Throw>().SetTargetPosition(Managers.Asset.GetTargetPosition((int)_assetType));
        }
    }

    private void MatchRoomButton()
    {
		if(IsCanCreateRoom())
			JoinOrCreate();
    }

    private void JobButton(bool _A)
    {
		SetPlayerJob(_A);
		_mainPv.RPC("SelectJob", RpcTarget.All, _A);
    }

	[PunRPC]
	private void SelectJob()
	{
		lobby.SelectJob(IsPlayerTeamA());
	}

    [PunRPC]
    private void JoinPlayer()
	{
		lobby.JoinPlayer();
	}


    public bool IsCanCreateRoom()
	{
		return (_nickName != "" && _roomCode != "");
    }

    public string GetJoinRoomPlayerCount()
	{
		return "Player : " + PN.CurrentRoom.PlayerCount + " / " + PN.CurrentRoom.MaxPlayers;
	}

	public bool IsFullPlayers()
	{
		return (PN.CurrentRoom.PlayerCount == PN.CurrentRoom.MaxPlayers);
	}

	public bool IsMaster()
	{
		return PN.IsMasterClient;
	}
}
