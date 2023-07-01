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
	/// <summary> 타이틀 화면 창 </summary>
	[SerializeField] private GameObject _Window_Title;

	/// <summary> 메인 화면 창 </summary>
	[SerializeField] private GameObject _Window_Main;

	/// <summary> 팝업 창 </summary>
	[SerializeField] private GameObject _Window_Popup;
	#endregion

	#region InputField
	/// <summary> 방 코드 입력 필드 </summary>
	[SerializeField] private TMP_InputField _Input_RoomCode;

	/// <summary> 플레이어 닉네임 임력 필드 </summary>
	[SerializeField] private TMP_InputField _Input_NickName;
	#endregion

	#region Text
	/// <summary> 팝업창 헤더 텍스트 </summary>
	[SerializeField] private TMP_Text _Text_Popup_Header;

	/// <summary> 팝업창 내용 텍스트 </summary>
	[SerializeField] private TMP_Text _Text_Popup_Subject;
	#endregion

	#region Image
	/// <summary> A 포인트 선택 이미지 </summary>
	[SerializeField] private Image _Image_Select_PointA;

	/// <summary> B 포인트 선택 이미지 </summary>
	[SerializeField] private Image _Image_Select_PointB;
	#endregion

	#region Object_UI
	/// <summary> 취소 버튼 오브젝트 </summary>
	[SerializeField] private GameObject _Object_CancelButton;

	/// <summary> 포인트 선택 버튼 오브젝트 </summary>
	[SerializeField] private GameObject _Object_PointButton;


	#endregion

	#region Sprite
	/// <summary> 포인트 선택 스프라이트 </summary>
	[SerializeField] private Sprite _Sprite_Check;

	/// <summary> 포인트 선택 불가 스프라이트 </summary>
	[SerializeField] private Sprite _Sprite_X;
	#endregion

	/// <summary> 포톤 뷰 </summary>
	private PhotonView _pv;

	/// <summary> 플레이어가 선택을 했는지의 여부 </summary>
	private bool _Selected = false;
	/// <summary> 플레이어가 포인트 선택을 했는지의 여부 </summary>
	private bool _Selected_PointA = false, _Selected_PointB = false;

	private void Start()
	{
		_pv = gameObject.GetComponent<PhotonView>();
		InitWindow();
	}

	/// <summary> UI 창 초기화 </summary>
	private void InitWindow()
	{
		_Window_Title.SetActive(true);
		_Window_Main.SetActive(false);
		_Window_Popup.SetActive(false);
	}

	#region Button
	/// <summary>
	/// 타이틀 화면에서 메인 화면으로 이동 및 마스터 서버 연결
	/// '타이틀 화면의 START버튼과 연결되어 있음'
	/// </summary>
	public void Button_Start()
	{
		NetworkManager.Net.Connect();
		_Window_Title.SetActive(false);
		_Window_Main.SetActive(true);
	}

	/// <summary>
	/// 방 이름과 닉네임을 NetworkManager에 전달하고 방을 입장하거나 생성
	/// '메인 화면의 JOIN버튼과 연결되어 있음
	/// </summary>
	public void Button_CreateOrJoin()
	{
		if (_Input_RoomCode.text.Length <= 0 || _Input_NickName.text.Length <= 0)
		{
			SetPopup(Define.PopupState.Warning, "Warning", "Nothing Enter");
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
		SetPopup(Define.PopupState.Wait, "Wait for Player", "Player : 0 / 2");
		_pv.RPC("JoinPlayer", RpcTarget.All);
	}

	/// <summary>
	/// 팝업창 종료 다른 플레이어를 기다리는 중이라면 Discnnect 후 종료
	/// '팝업창의 Cancel버튼과 연결되어 있음
	/// </summary>
	public void Button_Cancel()
	{
		if (_PopupState == Define.PopupState.Wait) { NetworkManager.Net.LeaveRoom(); }
		_Window_Popup.SetActive(false);
	}

	/// <summary>
	/// 선택한 위치를 네트워크 매니저에 저장
	/// 메인 화면의 A, B 버튼에 연결되어 있음
	/// </summary>
	/// <param name="_A">true : 포인트 A 선택, false : 포인트 B 선택</param>
	public void Button_Point(bool _A)
	{
		_Selected = true;

		SetPopup(Define.PopupState.MaxPlayer, "Choose your tools", "You choice : " + (_A ? "Wood" : "Stone"));

		NetworkManager.Net.SetPlayerSpawnPoint(_A);

		_pv.RPC("SelectPoint", RpcTarget.All, _A);
	}
	#endregion

	#region Popup
	/// <summary> 팝업창의 상태 </summary>
	private Define.PopupState _PopupState;

	/// <summary> 팝업 텍스트 변경 </summary>
	/// <param name="_state">팝업 상태</param>
	/// <param name="_subjectText">팝업 내용</param>
	private void SetPopup(Define.PopupState _state, string _headerText, string _subjectText)
	{
		_Window_Popup.SetActive(true);
		_PopupState = _state;

		_Text_Popup_Header.text = _headerText;

		// 대기중이라면 플레이어가 현재 몇명 들어와 있는지 출력
		if (_state == Define.PopupState.Wait) { _Text_Popup_Subject.text = SetJoinRoomPlayerCount(); }
		else { _Text_Popup_Subject.text = _subjectText; }

		// 플레이어가 모두 들어오면 시작지점 선택 버튼 등장
		OnPointButton(_state == Define.PopupState.MaxPlayer);
		
	}
	#endregion

	// 플레이어가 방에 들어오면 실행
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.Log(newPlayer.NickName + " 참가.");

		SetJoinRoomPlayerCount();

		_pv.RPC("JoinPlayer", RpcTarget.All);
	}

	/// <summary> 플레이어가 방에 현재 몇명 들어와 있는지 출력 </summary>
	private string SetJoinRoomPlayerCount() { return "Player : " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers; }

	/// <summary> 캔슬과 포인트 선택 버튼을 교체 </summary>
	/// <param name="_on">true : 포인트 선택 버튼으로 교체, false : 캔슬 버튼으로 교체</param>
	private void OnPointButton(bool _on)
	{
		_Object_PointButton.SetActive(_on);
		_Object_CancelButton.SetActive(!_on);
	}

	// 플레이어가 방에서 나가면 실행
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.Log(otherPlayer.NickName + " 나감.");
		InitSelectPoint();
		SetJoinRoomPlayerCount();
	}

	/// <summary> 선택한 시작지점 초기화 </summary>
	private void InitSelectPoint()
	{
		_Image_Select_PointA.gameObject.SetActive(false);
		_Image_Select_PointB.gameObject.SetActive(false);
		_Selected = false;
		_Selected_PointA = false;
		_Selected_PointB = false;
		SetPopup(Define.PopupState.Wait, "Wait for Player", "Player : 0 / 2");
	}

	/// <summary> 방에 플레이어가 최대로 들어왔다면 캔슬 버튼을 포인트 선택 버튼으로 교체 </summary>
	[PunRPC]
	private void JoinPlayer()
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
		{
			SetPopup(Define.PopupState.MaxPlayer, "Choose your tools", "You choice : ");
			OnPointButton(true);
		}
		else
		{
			InitSelectPoint();
			OnPointButton(false);
		}
	}

	[PunRPC]
	/// <summary> 플레이어가 포인트를 선택하면 선택지 제거 모두 선택이 완료 되었다면 게임을 플레이할 씬으로 이동 </summary>
	/// <param name="_A">true : 포인트 A 선택, false : 포인트 B 선택</param>
	private void SelectPoint(bool _A)
	{
		if(_A)
		{
			_Selected_PointA = true;
			_Object_PointButton.transform.Find("Button_PointA").GetComponent<Button>().interactable = false;
			_Image_Select_PointA.gameObject.SetActive(true);
			if (_pv.IsMine)
			{
				if (_Selected)
				{
					_Image_Select_PointA.sprite = _Sprite_Check;
					_Image_Select_PointA.color = Color.green;
				}
				else
				{
					_Image_Select_PointA.sprite = _Sprite_X;
					_Image_Select_PointA.color = Color.red;
				}
			}
		}
		else
		{
			_Selected_PointB = true;
			_Object_PointButton.transform.Find("Button_PointB").GetComponent<Button>().interactable = false;
			_Image_Select_PointB.gameObject.SetActive(true);
			if (_pv.IsMine)
			{
				if (_Selected)
				{
					_Image_Select_PointB.sprite = _Sprite_Check;
					_Image_Select_PointB.color = Color.green;
				}
				else
				{
					_Image_Select_PointB.sprite = _Sprite_X;
					_Image_Select_PointB.color = Color.red;
				}
			}
		}

		if (_Selected_PointA && _Selected_PointB)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
				{
					PhotonNetwork.LoadLevel("PlayRoom");
				}
			}
		}
	}
}
