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
	/// �÷��̾ A ����Ʈ�� ������ ����
	/// true : �÷��̾� A ����Ʈ���� ��ȯ
	/// false : �÷��̾� B ����Ʈ���� ��ȯ
	/// </summary>
	private bool _pointA;

	private bool _onDevelopMode = false;

	/// <summary>
	/// ������ �濡�� ������������ ����
	///  true : ������ ��������
	///  false : ������ ������ ����
	/// </summary>
	public bool _forceOut { get; private set; } = false;
	private PhotonView _pv;

	public void Init()
	{
        _pv = GetComponent<PhotonView>();
        // ���� ���� ���� ( ������ ���� ��������� ��Ī�� ������ )
        PhotonNetwork.GameVersion = _gameVersion;
        // �ʴ� ��Ű���� �����ϴ� Ƚ��
        PhotonNetwork.SendRate = 60;
        // �ʴ� OnPhotonSerialize�� �����ϴ� Ƚ��
        PhotonNetwork.SerializationRate = 30;
        // PhotonNetwork.LoadLevel�� ����Ͽ��� �� ��� �����ڸ� ������ ������ �̵��ϰ� �ϴ����� ����
        PhotonNetwork.AutomaticallySyncScene = true;
	}

	private void Update()
	{
		if(Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.M))
		{
			_onDevelopMode = !_onDevelopMode;
			Debug.Log("������ ��� : " + _onDevelopMode);
		}

		if (!_onDevelopMode)
			return;

		Cheat();
    }

    private void Cheat()
    {
        // B�� ������ �κ� ����
        if (Input.GetKeyDown(KeyCode.B)) JoinLobby();

        // C�� ������ ������ ������ ����
        if (Input.GetKeyDown(KeyCode.C)) Connect();

        // D�� ������ ���� ���� ����
        if (Input.GetKeyDown(KeyCode.D)) DisConnect();

        // I�� ������ ���� ���� ȣ��
        if (Input.GetKeyDown(KeyCode.I)) Info();

        // J�� ������ �� ����
        if (Input.GetKeyDown(KeyCode.J)) JoinOrCreate(_roomCode, 2);

        // L�� ������ �� ������
        if (Input.GetKeyDown(KeyCode.L)) LeaveRoom();

        // S�� ������ �⺻ ����
        if (Input.GetKeyDown(KeyCode.S)) EnterRoomSolo();

        // T�� ������ ȥ�ڼ� ����
        if (Input.GetKeyDown(KeyCode.T)) PhotonNetwork.LoadLevel("PlayRoom");
    }

	public void SetNickName(string _name) { _nickName = _name; }

	public void SetRoomCode(string _code) { _roomCode = _code; }

	public void Connect() { PhotonNetwork.ConnectUsingSettings(); }

	public override void OnConnectedToMaster()
	{
		Debug.Log("������ ���� ���� ����!");

		PhotonNetwork.LocalPlayer.NickName = _nickName;
	}

	public void JoinLobby()
	{
		PhotonNetwork.JoinLobby();
	}

	public override void OnJoinedLobby()
	{
		Debug.Log("�κ� ���� �Ϸ�.");

		JoinOrCreate(_roomCode, 2);
	}

	public void JoinOrCreate(string code, int _maxPlayer)
	{
		// ( �� �̸�, �� �ɼ�, �κ� Ÿ�� )
		PhotonNetwork.JoinOrCreateRoom(code, new RoomOptions { MaxPlayers = _maxPlayer }, null);
	}

	public override void OnCreatedRoom() { Debug.Log("�� ����� �Ϸ�."); }

	public override void OnJoinedRoom() { Debug.Log("�� ���� ����!"); }

    public override void OnLeftRoom() { Debug.Log("�� ���� ����!"); }

    public void LeaveRoom() { PhotonNetwork.LeaveRoom(); }

	public void DisConnect()
	{
		if (PhotonNetwork.IsConnected) { PhotonNetwork.Disconnect(); }
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("���� ���� ����");
		Debug.Log(cause);
	}

    public void SetPlayerSpawnPoint(bool _point) { _pointA = _point; }

	public void SetForceOut(bool _force) { _forceOut = _force; }

    private void Info()
	{
		if (PhotonNetwork.InRoom)
		{
			Debug.Log("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
			Debug.Log("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
			Debug.Log("���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

			string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
			
			for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
				playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";

			Debug.Log(playerStr);
		}
		else
		{
			Debug.Log("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
			Debug.Log("�� ���� : " + PhotonNetwork.CountOfRooms);
			Debug.Log("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
			Debug.Log("�κ� �ִ���? : " + PhotonNetwork.InLobby);
			Debug.Log("����ƴ���? : " + PhotonNetwork.IsConnected);
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
