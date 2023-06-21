using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{

	#region Window
	// 타이틀 화면 ( 게임 제목 및 게임 시작 버튼 )
	public GameObject _Window_Title;

	// 메인 화면 ( 방 이름과 닉네임 설정 및 서버 입장 )
	public GameObject _Window_Main;

	// 방이나 닉네임이 적혀 있지 않으면 나오는 팝업
	public GameObject _Window_Popup;
	#endregion

	#region InputField
	// 방 코드
	public TMP_InputField _Input_RoomCode;

	// 닉네임
	public TMP_InputField _Input_NickName;
	#endregion

	#region Text
	// 팝업창 제목 텍스트
	public TMP_Text _Text_Popup_Title;

	// 팝업창 내용 텍스트
	public TMP_Text _Text_Popup_Subject;
	#endregion

	#region Object_UI
	// 캔슬 버튼
	public GameObject _CancelButton;

	// 포인트 선택 버튼
	public GameObject _PointButton;
	#endregion

	// 포톤 뷰
	private PhotonView _pv;

	// 선택한 스폰 포인트
	private bool _SelectedA = false, _SelectedB = false;

	private void Start()
	{
		_pv = gameObject.GetComponent<PhotonView>();
		InitWindow();
	}

	// UI 창 초기화
	private void InitWindow()
	{
		_Window_Title.SetActive(true);
		_Window_Main.SetActive(false);
	}

	#region Button
	// 타이틀 화면에서 메인 화면으로 이동 및 마스터 서버 연결
	// 타이틀 화면의 START버튼과 연결되어 있음
	public void Button_Start()
	{
		NetworkManager.Net.Connect();
		_Window_Title.SetActive(false);
		_Window_Main.SetActive(true);
	}

	// 방 이름과 닉네임을 NetworkManager에 전달하고 방을 입장하거나 생성
	// 메인 화면의 JOIN버튼과 연결되어 있음
	public void Button_CreateOrJoin()
	{
		if (_Input_RoomCode.text.Length <= 0 || _Input_NickName.text.Length <= 0)
		{
			SetPopup(PopupState.Warning, "Nothing Enter");
			return;
		}

		NetworkManager.Net.SetRoomCode(_Input_RoomCode.text);
		NetworkManager.Net.SetNickName(_Input_NickName.text);

		NetworkManager.Net.JoinLobby();
	}

	// 방 입장시 실행
	public override void OnJoinedRoom()
	{
		print("방 입장 성공!");
		SetPopup(PopupState.Wait);
		_pv.RPC("JoinPlayer", RpcTarget.All);
	}

	// 플레이어를 기다리는 중이라면 Disconnect 아니라면 팝업창 종료
	// 팝업창의 Cancel버튼과 연결되어 있음
	public void Button_Cancel()
	{
		if (_PopupState == PopupState.Wait) { NetworkManager.Net.LeaveRoom(); }
		_Window_Popup.SetActive(false);
	}

	// 생성할 포인트를 선택하는 함수
	// 메인 화면의 A, B 버튼에 연결되어 있음
	public void Button_Point(bool _A)
	{
		NetworkManager.Net.SetPlayerSpawnPoint(_A);
		_pv.RPC("SelectPoint", RpcTarget.All, _A);
	}
	#endregion

	#region Popup
	// 팝업의 상태
	private PopupState _PopupState;
	private enum PopupState
	{
		Wait,
		Warning,
	}

	// 팝업 텍스트 세팅 ( 팝업 상태, 팝업 제목, 팝업 내용 )
	private void SetPopup(PopupState _state, string _subjectText = null)
	{
		_Window_Popup.SetActive(true);
		_PointButton.SetActive(false);

		_PopupState = _state;
		_Text_Popup_Title.text = _state.ToString();
		
		// 플레이어가 현재 몇명 들어와 있는지 출력
		if (_state == PopupState.Wait) { SetJoinRoomPlayerCount(_Text_Popup_Subject); }
		else { _Text_Popup_Subject.text = _subjectText; }
	}
	#endregion

	// 플레이어가 방에 들어오면 실행
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		print(newPlayer.NickName + " 참가.");

		SetJoinRoomPlayerCount(_Text_Popup_Subject);

		_pv.RPC("JoinPlayer", RpcTarget.All);

	}

	// 플레이어가 현재 몇명 들어와 있는지 출력 ( 출력할 텍스트 )
	private void SetJoinRoomPlayerCount(TMP_Text _text) => _text.text = "Player : " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

	// 스폰 포인트 선택 버튼을 띄움
	private void OnPointButton(bool _on)
	{
		_PointButton.SetActive(_on);
		_CancelButton.SetActive(!_on);
	}

	// 플레이어가 방에서 나가면 실행
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		print(otherPlayer.NickName + " 나감.");
		OnPointButton(false);
		SetJoinRoomPlayerCount(_Text_Popup_Subject);
	}

	// 방 최대 인원까지 플레이어가 들어왔다면 스폰 포인트를 선택하는 버튼을 띄운다.
	[PunRPC]
	private void JoinPlayer()
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
		{
			OnPointButton(true);
		}
	}

	// 플레이어가 선택한 버튼 제거 선택이 완료 되었다면 플레이 룸으로 이동
	[PunRPC]
	private void SelectPoint(bool _A)
	{
		if (_SelectedA && _SelectedB)
		{

			if (PhotonNetwork.IsMasterClient)
			{
				if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
				{
					PhotonNetwork.LoadLevel("PlayRoom");
				}
			}
		}
		else
		{
			if (_A)
			{
				_SelectedA = true;
				_PointButton.transform.Find("Button_PointA").gameObject.SetActive(false);
			}
			else
			{
				_SelectedB = true;
				_PointButton.transform.Find("Button_PointB").gameObject.SetActive(false);
			}
		}
	}
}
