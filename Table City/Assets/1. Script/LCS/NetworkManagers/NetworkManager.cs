using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
{

	/// <summary> ���� ���� </summary>
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
	/// <summary> �� ���� �̸� </summary>
	public string _roomCode { get; private set; }
	
	/// <summary> �÷��̾��� �г��� </summary>
	public string _nickName { get; private set; }

	/// <summary>
	/// �÷��̾ A ����Ʈ�� ������ ����
	/// true : �÷��̾� A ����Ʈ���� ��ȯ
	/// false : �÷��̾� B ����Ʈ���� ��ȯ
	/// </summary>
	private bool _pointA;
	#endregion

	/// <summary>
	/// �����ڸ�� ��� ����
	/// true : �����ڸ�� ���
	/// false : �����ڸ�� ����
	/// </summary>
	private bool _onDevelopMode = false;

	/// <summary>��Ʈ��ũ ���� �ʱ�ȭ</summary>
	private void InitNetworkSetting()
	{
		PhotonNetwork.GameVersion = this._gameVersion; // ���� ���� ���� ( ������ ���� ��������� ��Ī�� ������ )
		PhotonNetwork.SendRate = 60; // �ʴ� ��Ű���� �����ϴ� Ƚ��
		PhotonNetwork.SerializationRate = 30; // �ʴ� OnPhotonSerialize�� �����ϴ� Ƚ��
		PhotonNetwork.AutomaticallySyncScene = true; // PhotonNetwork.LoadLevel�� ����Ͽ��� �� ��� �����ڸ� ������ ������ �̵��ϰ� �ϴ����� ����
	}

	private void Update()
	{
		// D, M�� ���ÿ� ������ �����ڸ�� ��� �Ǵ� ����
		if(Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.M)) { _onDevelopMode = !_onDevelopMode; print("������ ��� : " + _onDevelopMode); }
		if (_onDevelopMode) { DevelopMode(); }
	}

	#region SetPlayerServerInfo
	/// <summary> �÷��̾� �г��� ���� ( string ����� �г��� ) </summary>
	/// <param name="_code">����� �г���</param>
	public void SetNickName(string _name) => _nickName = _name;

	/// <summary> ������ �� �ڵ� ���� </summary>
	/// <param name="_code">�� �ڵ�</param>
	public void SetRoomCode(string _code) => _roomCode = _code;
	#endregion

	#region Connect
	// ���� ����
	public void Connect() => PhotonNetwork.ConnectUsingSettings();

	// ������ ������ ���� �ϸ� ����
	public override void OnConnectedToMaster()
	{
		print("������ ���� ���� ����!");

		PhotonNetwork.LocalPlayer.NickName = _nickName;
	}
	#endregion

	#region Lobby
	/// <summary> �κ� ���� </summary>
	public void JoinLobby()
	{
		PhotonNetwork.JoinLobby();
	}
	
	// �κ� ���� �ϸ� ����
	public override void OnJoinedLobby()
	{
		print("�κ� ���� �Ϸ�.");

		JoinOrCreate(_roomCode, 2);
	}
	#endregion

	#region Room
	/// <summary> �� ���� ���� ������ ���� </summary>
	/// <param name="_code">�� �ڵ�</param>
	/// <param name="_maxPlayer">���� ���� �ο� ��</param>
	public void JoinOrCreate(string _code, int _maxPlayer)
				=> PhotonNetwork.JoinOrCreateRoom(_code, new RoomOptions { MaxPlayers = _maxPlayer }, null); // ( �� �̸�, �� �ɼ�, �κ� Ÿ�� )

	// ���� ��������� ����
	public override void OnCreatedRoom()
	{
		Debug.Log("�� ����� �Ϸ�.");
	}

	// ������� �濡 ���� ����
	public override void OnJoinedRoom()
	{
		Debug.Log("�� ���� ����!");
	}

	/// <summary> �� ���� </summary>
	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	// �� ����� ����
	public override void OnLeftRoom()
	{
		print("�� ���� ����!");
	}
	#endregion

	#region Spawn
	/// <summary> ������Ʈ ��ȯ �� ��ȯ�� ������Ʈ ���� </summary>
	/// <param name="_objectName">��ȯ�� ������Ʈ ���� ��ġ (Resources ���� ��ȯ)</param>
	/// <param name="_point">��ȯ�� ��ġ</param>
	public GameObject SpawnObject(string _objectName, Transform _point)
	{
		return PhotonNetwork.Instantiate(_objectName, _point.position, _point.rotation);
	}

	/// <summary> �÷��̾� ��ȯ </summary>
	public void SpawnPlayer()
	{
		GameObject player = SpawnObject("0. Player/Player_Prefab", _pointA ? RoomManager.room._PlayerPointA : RoomManager.room._PlayerPointB);
		player.name += PhotonNetwork.NickName;
		GameObject workbench = SpawnObject("0. Player/Player_Workbench", _pointA ? RoomManager.room._WorkbenchPointA : RoomManager.room._WorkbenchPointB);
		workbench.name += PhotonNetwork.NickName;
	}
	#endregion

	#region Disconnect
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
		Debug.Log("���� ���� ����");
		Debug.Log(cause);
	}
	#endregion

	/// <summary> �÷��̾ ������ ��ġ ���� </summary>
	/// <param name="_point">true : ����Ʈ A ����, false : ����Ʈ B ����</param>
	public void SetPlayerSpawnPoint(bool _point) => _pointA = _point;

	/// <summary>
	/// ���� ���� ���
	/// �濡 �� �ִٸ� �� �̸�, �� �ο���, �� �ִ� �ο���, �濡 �ִ� �÷��̾� ��� ���
	/// �׷��� ������ ������ �ο� ��, �� ����, ��� �濡 �ִ� �ο� ��, �κ� �ִ����� ����, ������ �ƴ����� ����
	/// </summary>
	public void Info()
	{
		if (PhotonNetwork.InRoom)
		{
			Debug.Log("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
			Debug.Log("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
			Debug.Log("���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

			string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
			for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
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

	/// <summary>
	/// ������ ���
	/// ��κ� �ܾ��� ���ڸ��� ������
	/// keycode : 'C'onnect, lo'B'by, 'I'nfo, 'L'eave, 'J'oin, 'D'isconnect
	/// </summary>
	private void DevelopMode()
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

	}

}
