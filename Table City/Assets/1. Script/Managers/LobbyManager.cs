using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : PunManagerBase
{

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

	private struct PopupInfo
	{
		public string _header, _text;

        public void SetInfo(string header, string text)
        {
            _header = header;
			SetText(text);
        }

        public void SetText(string text)
        {
            _text = text;
        }
    }
	private List<PopupInfo> popupInfos = new List<PopupInfo>();

	public override void Init()
	{
		_pv = gameObject.GetComponent<PhotonView>();
		if (Managers._network._forceOut)
		{
			SetPopup(Define.PopupState.Warning);
			Managers._network.SetForceOut(false);
		}
		else
		{
			InitWindow();
			InitPopupInfo();
            Managers._ui.ShowUI(Define.UI.title);
        }
	}

	private void InitWindow()
	{
		Managers._find.SetRoot("LobbyCanvas");
        Managers._ui.SetUI(Define.UI.title, "Title");
        Managers._ui.SetUI(Define.UI.lobby, "Lobby");
        Managers._ui.SetUI(Define.UI.match, "Match");
		GameObject go = Managers._find.Find("Match");

        Managers._find.SetRoot(go.transform);
        Managers._ui.SetUI(Define.UI.matchHeader, "Header");
        Managers._ui.SetUI(Define.UI.matchText, "Subject");
    }

	private void InitPopupInfo()
    {
        popupInfos.Add(new PopupInfo { });
        popupInfos[(int)Define.PopupState.Wait].SetInfo("Wait for Player", "Player : 0 / 2");

        popupInfos.Add(new PopupInfo { });
        popupInfos[(int)Define.PopupState.MaxPlayer].SetInfo("Choose your tools", "You choice : ");

        popupInfos.Add(new PopupInfo { });
        popupInfos[(int)Define.PopupState.Warning].SetInfo("Warning", "Nothing Enter");
        
    }

	private void SetPopup(Define.PopupState state)
	{
        Managers._ui.ShowUI(Define.UI.match);

        // 대기중이라면 플레이어가 현재 몇명 들어와 있는지 출력
        if (state == Define.PopupState.Wait)
        { popupInfos[(int)state].SetText(Managers._network.GetInRoomPlayerCount()); }

        _Text_Popup_Header.text = popupInfos[(int)state]._header;
		_Text_Popup_Subject.text = popupInfos[(int)state]._text;

		// 플레이어가 모두 들어오면 시작지점 선택 버튼 등장
		OnPointButton(state == Define.PopupState.MaxPlayer);
		
	}

	/// <summary> 캔슬과 포인트 선택 버튼을 교체 </summary>
	/// <param name="_on">true : 포인트 선택 버튼으로 교체, false : 캔슬 버튼으로 교체</param>
	private void OnPointButton(bool _on)
	{
		_Object_PointButton.SetActive(_on);
		_Object_CancelButton.SetActive(!_on);
	}

	/// <summary> 선택한 시작지점 초기화 </summary>
	private void InitSelectPoint()
	{
		_Object_PointButton.transform.Find("Button_PointA").GetComponent<Button>().interactable = true;
		_Object_PointButton.transform.Find("Button_PointB").GetComponent<Button>().interactable = true;
		_Image_Select_PointA.gameObject.SetActive(false);
		_Image_Select_PointB.gameObject.SetActive(false);
		_Selected = false;
		_Selected_PointA = false;
		_Selected_PointB = false;
	}

	/// <summary> 방에 플레이어가 최대로 들어왔다면 캔슬 버튼을 포인트 선택 버튼으로 교체 </summary>
	[PunRPC]
	private void JoinPlayer()
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
		{
			InitSelectPoint();
			SetPopup(Define.PopupState.MaxPlayer);
			OnPointButton(true);
		}
		else
		{
			OnPointButton(false);
		}
	}

	[PunRPC]
	/// <summary> 플레이어가 포인트를 선택하면 선택지 제거 모두 선택이 완료 되었다면 게임을 플레이할 씬으로 이동 </summary>
	/// <param name="_A">true : 포인트 A 선택, false : 포인트 B 선택</param>
	private void SelectPoint(bool _A)
	{
		if (_Selected)
		{
			_Object_PointButton.transform.Find("Button_PointA").GetComponent<Button>().interactable = false;
			_Object_PointButton.transform.Find("Button_PointB").GetComponent<Button>().interactable = false;
			if (_A)
			{
				_Selected_PointA = true;
				_Image_Select_PointA.gameObject.SetActive(true);
				_Image_Select_PointA.sprite = _Sprite_Check;
				_Image_Select_PointA.color = Color.green;
			}
			else
			{
				_Selected_PointB = true;
				_Image_Select_PointB.gameObject.SetActive(true);
				_Image_Select_PointB.sprite = _Sprite_Check;
				_Image_Select_PointB.color = Color.green;
			}
			_Selected = false;
		}
		else
		{
			if (_A)
			{
				_Selected_PointA = true;
				_Object_PointButton.transform.Find("Button_PointA").GetComponent<Button>().interactable = false;
				_Image_Select_PointA.gameObject.SetActive(true);
				_Image_Select_PointA.sprite = _Sprite_X;
				_Image_Select_PointA.color = Color.red;
			}
			else
			{
				_Selected_PointB = true;
				_Object_PointButton.transform.Find("Button_PointB").GetComponent<Button>().interactable = false;
				_Image_Select_PointB.gameObject.SetActive(true);
				_Image_Select_PointB.sprite = _Sprite_X;
				_Image_Select_PointB.color = Color.red;
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
