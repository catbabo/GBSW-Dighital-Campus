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
        // ���� ���� ���� ( ������ ���� ��������� ��Ī�� ������ )
        PN.GameVersion = _gameVersion;
        // �ʴ� ��Ű���� �����ϴ� Ƚ��
        PN.SendRate = 60;
        // �ʴ� OnPhotonSerialize�� �����ϴ� Ƚ��
        PN.SerializationRate = 30;
        // PhotonNetwork.LoadLevel�� ����Ͽ��� �� ��� �����ڸ� ������ ������ �̵��ϰ� �ϴ����� ����
        PN.AutomaticallySyncScene = true;

        InitEvent();
    }

    private void InitEvent()
    {
        Managers.Event.AddOnGameStart(OnGameStart);
        Managers.Event.AddMatchRoomButton(MatchRoomButton);
    }

	public PhotonView GetPhotonView()
	{
		return _mainPv;
	}

	public void SetNickName(string _name) { _nickName = _name; }

	public void SetRoomCode(string _code) { _roomCode = _code; }

	public void Connect() { PN.ConnectUsingSettings(); }

	public override void OnConnectedToMaster()
	{
		Debug.Log("������ ���� ���� ����!");
		PN.LocalPlayer.NickName = _nickName;
		JoinLobby();
	}

	public void JoinLobby()
	{
		PN.JoinLobby();
	}

	public override void OnJoinedLobby()
	{
		Debug.Log("�κ� ���� �Ϸ�.");
		//JoinOrCreate();
	}

	public void JoinOrCreate()
	{
        PN.JoinOrCreateRoom(_roomCode, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
	}

	public override void OnCreatedRoom()
	{ Debug.Log("�� ����� �Ϸ�."); }

	public override void OnJoinedRoom()
	{
		Debug.Log("�� ���� ����!");
		lobby.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " ����.");
        lobby.OnPlayerEnteredRoom();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
		Debug.Log(otherPlayer.NickName + " ����.");
        lobby.OnPlayerLeftRoom();
    }

    public override void OnLeftRoom()
	{ Debug.Log("�� ���� ����!"); }

    public void LeaveRoom()
	{ PN.LeaveRoom(); }

	public void DisConnect()
	{
		if (PN.IsConnected)
		{ PN.Disconnect(); }
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("���� ���� ����\n"+cause);
	}

    public void SetPlayerSpawnPoint(bool _point) { _pointA = _point; }
	public bool IsPlayerTeamA() { return _pointA; }

    public void SetForceOut(bool _force) { _forceOut = _force; }

    public void Info()
	{
		if (PN.InRoom)
		{
			Debug.Log("���� �� �̸� : " + PN.CurrentRoom.Name);
			Debug.Log("���� �� �ο��� : " + PN.CurrentRoom.PlayerCount);
			Debug.Log("���� �� �ִ��ο��� : " + PN.CurrentRoom.MaxPlayers);

			string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
			
			for (int i = 0; i < PN.PlayerList.Length; i++)
				playerStr += PN.PlayerList[i].NickName + ", ";

			Debug.Log(playerStr);
		}
		else
			{
				Debug.Log("������ �ο� �� : " + PN.CountOfPlayers);
			Debug.Log("�� ���� : " + PN.CountOfRooms);
			Debug.Log("��� �濡 �ִ� �ο� �� : " + PN.CountOfPlayersInRooms);
			Debug.Log("�κ� �ִ���? : " + PN.InLobby);
			Debug.Log("����ƴ���? : " + PN.IsConnected);
		}
	}

	public void EnterRoomSolo()
	{
		SetNickName("admin");
		SetRoomCode("DevelopRoom");
		SetPlayerSpawnPoint(true);
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

    private void OnGameStart()
    {
        Connect();
    }

    private void MatchRoomButton()
    {
		JoinOrCreate();
    }
}
