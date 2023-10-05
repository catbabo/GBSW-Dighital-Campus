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
	/// <summary> �� �ڵ� �Է� �ʵ� </summary>
	[SerializeField] private TMP_InputField _Input_RoomCode;

	/// <summary> �÷��̾� �г��� �ӷ� �ʵ� </summary>
	[SerializeField] private TMP_InputField _Input_NickName;
	#endregion

	#region Text
	/// <summary> �˾�â ��� �ؽ�Ʈ </summary>
	[SerializeField] private TMP_Text _Text_Popup_Header;

	/// <summary> �˾�â ���� �ؽ�Ʈ </summary>
	[SerializeField] private TMP_Text _Text_Popup_Subject;
	#endregion

	#region Image
	/// <summary> A ����Ʈ ���� �̹��� </summary>
	[SerializeField] private Image _Image_Select_PointA;

	/// <summary> B ����Ʈ ���� �̹��� </summary>
	[SerializeField] private Image _Image_Select_PointB;
	#endregion

	#region Object_UI
	/// <summary> ��� ��ư ������Ʈ </summary>
	[SerializeField] private GameObject _Object_CancelButton;

	/// <summary> ����Ʈ ���� ��ư ������Ʈ </summary>
	[SerializeField] private GameObject _Object_PointButton;


	#endregion

	#region Sprite
	/// <summary> ����Ʈ ���� ��������Ʈ </summary>
	[SerializeField] private Sprite _Sprite_Check;

	/// <summary> ����Ʈ ���� �Ұ� ��������Ʈ </summary>
	[SerializeField] private Sprite _Sprite_X;
	#endregion

	/// <summary> ���� �� </summary>
	private PhotonView _pv;

	/// <summary> �÷��̾ ������ �ߴ����� ���� </summary>
	private bool _Selected = false;
	/// <summary> �÷��̾ ����Ʈ ������ �ߴ����� ���� </summary>
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

        // ������̶�� �÷��̾ ���� ��� ���� �ִ��� ���
        if (state == Define.PopupState.Wait)
        { popupInfos[(int)state].SetText(Managers._network.GetInRoomPlayerCount()); }

        _Text_Popup_Header.text = popupInfos[(int)state]._header;
		_Text_Popup_Subject.text = popupInfos[(int)state]._text;

		// �÷��̾ ��� ������ �������� ���� ��ư ����
		OnPointButton(state == Define.PopupState.MaxPlayer);
		
	}

	/// <summary> ĵ���� ����Ʈ ���� ��ư�� ��ü </summary>
	/// <param name="_on">true : ����Ʈ ���� ��ư���� ��ü, false : ĵ�� ��ư���� ��ü</param>
	private void OnPointButton(bool _on)
	{
		_Object_PointButton.SetActive(_on);
		_Object_CancelButton.SetActive(!_on);
	}

	/// <summary> ������ �������� �ʱ�ȭ </summary>
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

	/// <summary> �濡 �÷��̾ �ִ�� ���Դٸ� ĵ�� ��ư�� ����Ʈ ���� ��ư���� ��ü </summary>
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
	/// <summary> �÷��̾ ����Ʈ�� �����ϸ� ������ ���� ��� ������ �Ϸ� �Ǿ��ٸ� ������ �÷����� ������ �̵� </summary>
	/// <param name="_A">true : ����Ʈ A ����, false : ����Ʈ B ����</param>
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
