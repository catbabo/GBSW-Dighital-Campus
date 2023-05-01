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
        // 현재 어떤 상태인지. (정의로 올라가다보면, Enum으로 ClientState가 선언되어있다.)
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    #region Public Methods

    public void Connect()
    {
        // 처음에 Photon Online Server에 접속하는 게 가장 중요함!!
        // Photon Online Server에 접속하기.
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        // 연결 끊기.
        // Destroy(GameObject.Find("Player"));

        PhotonNetwork.Disconnect();
    }

    public void JoinLobby()
    {
        // 로비에 접속하기.
        PhotonNetwork.JoinLobby();
    }

    // 방을 참가하려면, Connect 되어있거나 Lobby에 참가해있어야 한다.

    public void CreateRoom()
    {
        // 방 생성하고, 참가.
        // 방 이름, 최대 플레이어 수, 비공개 등을 지정 가능.
        PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 });   
    }

    public void JoinRoom()
    {
        // 방 참가하기.
        // 방 이름으로 입장 가능.
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    public void JoinOrCreateRoom()
    {
        // 방 참가하는데, 방이 없으면 생성하고 참가.
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 }, null);
    }

    public void JoinRandomRoom()
    {
        // 방 랜덤으로 참가하기.
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        // 방 떠나기.
        PhotonNetwork.LeaveRoom();
    }

    #endregion


    #region Photon Callbacks

    /// <summary>
    /// Photon Online Server에 접속하면 불리는 콜백 함수.
    /// PhotonNetwork.ConnectUsingSettings()가 성공하면 불린다.
    /// </summary>
    public override void OnConnectedToMaster()
    {
        print("서버 접속 완료.");

        // 현재 플레이어 닉네임 설정.
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
    }

    /// <summary>
    /// 연결이 끊기면 불리는 콜백 함수.
    /// PhotonNetwork.Disconnect()가 성공하면 불린다.
    /// </summary>
    /// <param name="cause">끊기는 이유를 알려준다.</param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("연결 끊김.");
    }

    /// <summary>
    /// 로비에 접속하면 불리는 콜백 함수.
    /// PhotonNetwork.JoinLobby()가 성공하면 불린다.
    /// 대형 게임이 아니면, 하나의 로비에 여러 개의 방으로 커버가 될 듯하다. 하나의 로비만 필요하면, 굳이 JoinLobby할 필요는 없는 듯.
    /// </summary>
    public override void OnJoinedLobby()
    {
        print("로비 접속 완료.");
    }

    /// <summary>
    /// 방 생성하면 불리는 콜백 함수.
    /// PhotonNetwork.CreateRoom()가 성공하면 불린다.
    /// </summary>
    public override void OnCreatedRoom()
    {
        print("방 만들기 완료.");
    }

    /// <summary>
    /// 방 참가하면 불리는 콜백 함수.
    /// PhotonNetwork.CreateRoom(), PhotonNetwork.JoinedRoom()가 성공하면 불린다.
    /// </summary>
    public override void OnJoinedRoom()
    {
        print("방 참가 완료.");
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// 방 생성 실패하면 불리는 콜백 함수.
    /// PhotonNetwork.CreateRoom()를 호출할 떄, 방 이름이 같은 게 있으면 실패할 수 있다.
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("방 만들기 실패.");
    }

    /// <summary>
    /// 방 참가 실패하면 불리는 콜백 함수.
    /// PhotonNetwork.JoinRoom()를 호출할 때, 방 인원수가 모두 찼거나 방이 존재하지 않으면 실패할 수 있다.
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("방 참가 실패.");
    }

    /// <summary>
    /// 방 랜덤 참가 실패하면 불리는 콜백 함수.
    /// PhotonNetwork.JoinRandomRoom()를 호출할 때, 방 인원수가 모두 차있거나 존재하지 않으면 실패할 수 있다.
    /// 다른 사람이 더 빠르게 들어갔거나, 방을 닫았을 수 있다.
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("방 랜덤 참가 실패.");
    }

    #endregion


    [ContextMenu("정보")]
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