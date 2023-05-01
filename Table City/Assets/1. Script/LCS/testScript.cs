using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class testScript : MonoBehaviourPunCallbacks
{
    public TMP_Text StatusText;
    public TMP_InputField roomInput, NickNameInput;

	void Update()
    {
        // ���� � ��������. (���Ƿ� �ö󰡴ٺ���, Enum���� ClientState�� ����Ǿ��ִ�.)
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    #region Public Methods

    public void Connect()
    {
        // ó���� Photon Online Server�� �����ϴ� �� ���� �߿���!!
        // Photon Online Server�� �����ϱ�.
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        // ���� ����.
        // Destroy(GameObject.Find("Player"));

        PhotonNetwork.Disconnect();
    }

    public void JoinLobby()
    {
        // �κ� �����ϱ�.
        PhotonNetwork.JoinLobby();
    }

    // ���� �����Ϸ���, Connect �Ǿ��ְų� Lobby�� �������־�� �Ѵ�.

    public void CreateRoom()
    {
        // �� �����ϰ�, ����.
        // �� �̸�, �ִ� �÷��̾� ��, ����� ���� ���� ����.
        PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 });   
    }

    public void JoinRoom()
    {
        // �� �����ϱ�.
        // �� �̸����� ���� ����.
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    public void JoinOrCreateRoom()
    {
        // �� �����ϴµ�, ���� ������ �����ϰ� ����.
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 }, null);
    }

    public void JoinRandomRoom()
    {
        // �� �������� �����ϱ�.
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        // �� ������.
        PhotonNetwork.LeaveRoom();
    }

    #endregion


    #region Photon Callbacks

    /// <summary>
    /// Photon Online Server�� �����ϸ� �Ҹ��� �ݹ� �Լ�.
    /// PhotonNetwork.ConnectUsingSettings()�� �����ϸ� �Ҹ���.
    /// </summary>
    public override void OnConnectedToMaster()
    {
        print("���� ���� �Ϸ�.");

        // ���� �÷��̾� �г��� ����.
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
    }

    /// <summary>
    /// ������ ����� �Ҹ��� �ݹ� �Լ�.
    /// PhotonNetwork.Disconnect()�� �����ϸ� �Ҹ���.
    /// </summary>
    /// <param name="cause">����� ������ �˷��ش�.</param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("���� ����.");
    }

    /// <summary>
    /// �κ� �����ϸ� �Ҹ��� �ݹ� �Լ�.
    /// PhotonNetwork.JoinLobby()�� �����ϸ� �Ҹ���.
    /// ���� ������ �ƴϸ�, �ϳ��� �κ� ���� ���� ������ Ŀ���� �� ���ϴ�. �ϳ��� �κ� �ʿ��ϸ�, ���� JoinLobby�� �ʿ�� ���� ��.
    /// </summary>
    public override void OnJoinedLobby()
    {
        print("�κ� ���� �Ϸ�.");
    }

    /// <summary>
    /// �� �����ϸ� �Ҹ��� �ݹ� �Լ�.
    /// PhotonNetwork.CreateRoom()�� �����ϸ� �Ҹ���.
    /// </summary>
    public override void OnCreatedRoom()
    {
        print("�� ����� �Ϸ�.");
    }

    /// <summary>
    /// �� �����ϸ� �Ҹ��� �ݹ� �Լ�.
    /// PhotonNetwork.CreateRoom(), PhotonNetwork.JoinedRoom()�� �����ϸ� �Ҹ���.
    /// </summary>
    public override void OnJoinedRoom()
    {
        print("�� ���� �Ϸ�.");
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// �� ���� �����ϸ� �Ҹ��� �ݹ� �Լ�.
    /// PhotonNetwork.CreateRoom()�� ȣ���� ��, �� �̸��� ���� �� ������ ������ �� �ִ�.
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("�� ����� ����.");
    }

    /// <summary>
    /// �� ���� �����ϸ� �Ҹ��� �ݹ� �Լ�.
    /// PhotonNetwork.JoinRoom()�� ȣ���� ��, �� �ο����� ��� á�ų� ���� �������� ������ ������ �� �ִ�.
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("�� ���� ����.");
    }

    /// <summary>
    /// �� ���� ���� �����ϸ� �Ҹ��� �ݹ� �Լ�.
    /// PhotonNetwork.JoinRandomRoom()�� ȣ���� ��, �� �ο����� ��� ���ְų� �������� ������ ������ �� �ִ�.
    /// �ٸ� ����� �� ������ ���ų�, ���� �ݾ��� �� �ִ�.
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("�� ���� ���� ����.");
    }

    #endregion


    [ContextMenu("����")]
    public void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
            print("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else
        {
            print("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
            print("�� ���� : " + PhotonNetwork.CountOfRooms);
            print("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
            print("�κ� �ִ���? : " + PhotonNetwork.InLobby);
            print("����ƴ���? : " + PhotonNetwork.IsConnected);
        }
    }
}