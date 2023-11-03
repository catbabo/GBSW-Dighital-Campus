using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using PN = Photon.Pun.PhotonNetwork;
using Photon.Voice.PUN.UtilityScripts;

[RequireComponent(typeof(PhotonView))]
public class NetworkManager : PunManagerBase
{
	private string _gameVersion = "1";
	
	public string _roomCode;
	public string _nickName;

	private bool _pointA;

	public bool _forceOut { get; private set; } = false;
	private PhotonView _mainPv;

	[SerializeField]
	private RoomScene room;

	[SerializeField]
	private bool _masterReady, _clientReady;
	[SerializeField]
	private int _readyPlayerCount;
	[SerializeField]
	private bool _isMaster;
	[SerializeField]
	private bool _masterJobSelect, _clientJobSelect;


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
        Managers.Event.AddReadyButton(ReadyButton);
        Managers.Event.AddAllReady(AllReady);
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
	}

	public void JoinOrCreate()
	{
        PN.JoinOrCreateRoom(_roomCode, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
	}

	public override void OnCreatedRoom()
	{
		Debug.Log("방 만들기 완료.");
		_isMaster = PN.IsMasterClient;

    }

	public override void OnJoinedRoom()
	{
		Debug.Log("방 입장 성공!");
        _isMaster = PN.IsMasterClient;
        //_mainPv.RPC("JoinPlayer", RpcTarget.All);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " 참가.");
		_mainPv.RPC("SyncReadyData", RpcTarget.Others, _masterReady, _readyPlayerCount);
		room.OnPlayerEnteredRoom();
    }

	[PunRPC]
	private void SyncReadyData(bool master, int count)
	{
		_masterReady = master;
		_readyPlayerCount = count;
	}

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
		Debug.Log(otherPlayer.NickName + " 나감.");

		if (_isMaster)
        {
			if(_clientReady)
                _readyPlayerCount--;

            _clientReady = false;
        }
		else
        {
            if (_masterReady)
                _readyPlayerCount--;

            _masterReady = _clientReady;
			_clientReady = false;
			Managers.UI.ShowPopup("Master Exit", "Now You Are Master");
			_isMaster = IsMaster();
        }
		room.OnPlayerLeftRoom();
    }

    public void LeaveRoom()
	{
		PN.LeaveRoom();
	}

    public override void OnLeftRoom()
	{
		Debug.Log("방 퇴장 성공!");
		_masterReady = false;
		_clientReady = false;
		_readyPlayerCount = 0;
        _isMaster = PN.IsMasterClient;
    }

	public void DisConnect()
	{
		if (PN.IsConnected)
		{ PN.Disconnect(); }
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("서버 연결 해제\n"+cause);
	}

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

    private void ReadyButton(bool isReady)
    {
		if (!isReady)
        {
            if (IsMaster())
            {
                _masterReady = true;
            }
            else
            {
                _clientReady = true;
            }
			_readyPlayerCount++;
            _mainPv.RPC("ReadyOtherPlayer", RpcTarget.Others);
		}

		_mainPv.RPC("UpdateReadyPopup", RpcTarget.All);
    }

	[PunRPC]
    private void UpdateReadyPopup()
	{
		room.UpdateReadyPopup();
    }


    [PunRPC]
	private void ReadyOtherPlayer()
	{
		if(IsMaster())
		{
			_clientReady = true;
		}
		else
		{
			_masterReady = true;
		}
		_readyPlayerCount++;
    }

    public bool IsCanStartInGame()
    {
        return (_masterReady && _clientReady) || (_readyPlayerCount == PN.CurrentRoom.PlayerCount);
    }

    public bool IsSolo()
    {
        return (1 == PN.CurrentRoom.PlayerCount);
    }

	public void ReadyMention()
	{
		_mainPv.RPC("PleaseReady", RpcTarget.All);
	}

	[PunRPC]
	private void PleaseReady()
	{
		room.PleaseReady();
	}

    private void AllReady(bool isSolo)
    {
        LockRoom();
        if (isSolo)
        {
            room.InGameStart();
        }
		else
        {
            _mainPv.RPC("ShowJobButton", RpcTarget.All);
        }
    }

	private void LockRoom()
    {
        PN.CurrentRoom.IsOpen = false;
        PN.CurrentRoom.IsVisible = false;
    }

    [PunRPC]
    private void ShowJobButton()
    {
        room.ShowJobButton();
    }

    private void JobButton(bool _A)
    {
        SetPlayerJob(_A);
		_mainPv.RPC("SetSelectSync", RpcTarget.All, IsMaster());
		room.JobButton(_A);
    }

	public void InGame()
    {
        bool isCanInGame = (IsOnSelectJob() && IsMaster());
        Debug.Log("IsOnSelectJob() && IsMaster()  : " + $"{isCanInGame}");
        if (isCanInGame)
        {
            Debug.Log("InGame");
            _mainPv.RPC("InGameStart", RpcTarget.All);
        }
        else
        {
            Debug.Log("Foor");
        }
    }

	public void SelectJobSync(bool _A)
	{
		_mainPv.RPC("SelectJob", RpcTarget.All, _A);
	}

    [PunRPC]
    private void SelectJob(bool a)
    {
        room.SelectJob(a);
    }

    [PunRPC]
    private void SetSelectSync(bool isMaster)
    {
		if(isMaster)
		{
			Debug.Log("MasterSelect");
			_masterJobSelect = true;
		}
		else
        {
            Debug.Log("ClientSelect");
            _clientJobSelect = true;
		}

    }

    public void SetPlayerJob(bool _point) { _pointA = _point; }

    public bool IsPlayerTeamA() { return _pointA; }


    [PunRPC]
    private void InGameStart()
    {
		room.InGameStart();
    }


    public bool IsCanCreateRoom()
	{
		return (_nickName != "" && _roomCode != "");
    }

    public string GetJoinRoomPlayerCount()
	{
		return "Player : " + _readyPlayerCount + " / " + PN.CurrentRoom.PlayerCount;
	}

	public bool IsFullPlayers()
	{
		return (PN.CurrentRoom.PlayerCount == PN.CurrentRoom.MaxPlayers);
	}

	public bool IsMaster()
    {
        Debug.Log("IsMasterClient");
        return PN.IsMasterClient;
	}

	public bool IsOnSelectJob()
    {
        Debug.Log("IsOnSelectJob");
        return (_masterJobSelect && _clientJobSelect);
	}
}
