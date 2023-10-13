using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class LobbyController : MonoBehaviourPunCallbacks
{

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

	private void Start()
	{
		_pv = gameObject.GetComponent<PhotonView>();
		if (Managers._network._forceOut)
		{
			SetPopup(Define.PopupState.Warning);
			Managers._network.SetForceOut(false);
		}
    }

    private void OnEnterTitle()
    {
        InitWindow();
        InitPopupInfo();
        Managers._ui.ShowPannel(Define.UI.title);
    }

    private void OnEnterLobby()
    {

    }

    private void InitWindow()
	{
		Utill.SetRoot("LobbyCanvas");
        Managers._ui.AddUI<Pannel>("Title");
        Managers._ui.AddUI<Pannel>("Lobby");
        Managers._ui.AddUI<Pannel>("Match");

        Utill.SetRoot("Lobby", true);
        Managers._ui.AddUI<TMP_InputField>("InputField_RoomCode");
        Managers._ui.AddUI<TMP_InputField>("InputField_NickName");

		go = Managers._ui.GetUIObject<Pannel>("Match");
        Managers._find.SetRoot(go);
        Managers._find.SetRoot("Popup01");
        Managers._ui.AddUI<TMP_Text>(Define.UI.matchHeader, "Header");
        Managers._ui.AddUI<TMP_Text>(Define.UI.matchText, "Subject");

		go = Managers._find.Find("Select Point Button");
        Managers._find.SetRoot("Button_PointA");
        Managers._ui.AddUI<Image>(Define.UI.pointAImage, "Image_Select_PointA");

        Managers._find.SetRoot(go);
        Managers._find.SetRoot("Button_PointB");
        Managers._ui.AddUI<Image>(Define.UI.pointBImage, "Image_Select_PointB");
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

    public void SetPopupInfo(Define.PopupState popup, string header, string text)
    {
        popupInfos[(int)popup].SetInfo(header, text);
    }

    public void SetPopup(Define.PopupState state)
	{
        Managers._ui.ShowPannel(Define.UI.match);

        // ������̶�� �÷��̾ ���� ��� ���� �ִ��� ���
        if (state == Define.PopupState.Wait)
        { popupInfos[(int)state].SetText(Managers._network.GetInRoomPlayerCount()); }

        Managers._ui.SetText(Define.UI.matchHeader, popupInfos[(int)state]._header);
        Managers._ui.SetText(Define.UI.matchText, popupInfos[(int)state]._text);

		// �÷��̾ ��� ������ �������� ���� ��ư ����
		Managers._ui.ShowUIOnPopup(Define.PopupState.MaxPlayer);
	}

	/// <summary> ������ �������� �ʱ�ȭ </summary>
	private void InitSelectPoint()
	{
		_Object_PointButton.transform.Find("Button_PointA").GetComponent<Button>().interactable = true;
		_Object_PointButton.transform.Find("Button_PointB").GetComponent<Button>().interactable = true;
        Managers._ui.UIActive(Define.UI.pointAImage, false);
        Managers._ui.UIActive(Define.UI.pointBImage, false);
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
            Managers._ui.ShowUIOnPopup(Define.PopupState.MaxPlayer);
        }
		else
        {
            Managers._ui.CloseUIOnPopup(Define.PopupState.MaxPlayer);
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
                Managers._ui.UIActive(Define.UI.pointAImage, true);
                Managers._ui.GetUI<Image>(Define.UI.pointAImage).sprite = _Sprite_Check;
                Managers._ui.SetImageColor(Define.UI.pointAImage, Color.green);
            }
			else
			{
				_Selected_PointB = true;
                Managers._ui.UIActive(Define.UI.pointBImage, true);
                Managers._ui.GetUI<Image>(Define.UI.pointBImage).sprite = _Sprite_Check;
                Managers._ui.SetImageColor(Define.UI.pointBImage, Color.green);
            }
			_Selected = false;
		}
		else
		{
			if (_A)
			{
				_Selected_PointA = true;
				_Object_PointButton.transform.Find("Button_PointA").GetComponent<Button>().interactable = false;
                Managers._ui.UIActive(Define.UI.pointAImage, true);
                Managers._ui.GetUI<Image>(Define.UI.pointAImage).sprite = _Sprite_X;
                Managers._ui.SetImageColor(Define.UI.pointAImage, Color.red);
                _Selected_PointA = true;
            }
			else
			{
				_Selected_PointB = true;
				_Object_PointButton.transform.Find("Button_PointB").GetComponent<Button>().interactable = false;
                Managers._ui.UIActive(Define.UI.pointBImage, true);
                Managers._ui.GetUI<Image>(Define.UI.pointBImage).sprite = _Sprite_X;
                Managers._ui.SetImageColor(Define.UI.pointBImage, Color.red);
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
