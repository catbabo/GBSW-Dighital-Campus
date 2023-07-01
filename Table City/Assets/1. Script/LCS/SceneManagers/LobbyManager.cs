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
	/// <summary> Ÿ��Ʋ ȭ�� â </summary>
	[SerializeField] private GameObject _Window_Title;

	/// <summary> ���� ȭ�� â </summary>
	[SerializeField] private GameObject _Window_Main;

	/// <summary> �˾� â </summary>
	[SerializeField] private GameObject _Window_Popup;
	#endregion

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

	private void Start()
	{
		_pv = gameObject.GetComponent<PhotonView>();
		InitWindow();
	}

	/// <summary> UI â �ʱ�ȭ </summary>
	private void InitWindow()
	{
		_Window_Title.SetActive(true);
		_Window_Main.SetActive(false);
		_Window_Popup.SetActive(false);
	}

	#region Button
	/// <summary>
	/// Ÿ��Ʋ ȭ�鿡�� ���� ȭ������ �̵� �� ������ ���� ����
	/// 'Ÿ��Ʋ ȭ���� START��ư�� ����Ǿ� ����'
	/// </summary>
	public void Button_Start()
	{
		NetworkManager.Net.Connect();
		_Window_Title.SetActive(false);
		_Window_Main.SetActive(true);
	}

	/// <summary>
	/// �� �̸��� �г����� NetworkManager�� �����ϰ� ���� �����ϰų� ����
	/// '���� ȭ���� JOIN��ư�� ����Ǿ� ����
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

	// �� ����� ����
	public override void OnJoinedRoom()
	{
		print("�� ���� ����!");
		SetPopup(Define.PopupState.Wait, "Wait for Player", "Player : 0 / 2");
		_pv.RPC("JoinPlayer", RpcTarget.All);
	}

	/// <summary>
	/// �˾�â ���� �ٸ� �÷��̾ ��ٸ��� ���̶�� Discnnect �� ����
	/// '�˾�â�� Cancel��ư�� ����Ǿ� ����
	/// </summary>
	public void Button_Cancel()
	{
		if (_PopupState == Define.PopupState.Wait) { NetworkManager.Net.LeaveRoom(); }
		_Window_Popup.SetActive(false);
	}

	/// <summary>
	/// ������ ��ġ�� ��Ʈ��ũ �Ŵ����� ����
	/// ���� ȭ���� A, B ��ư�� ����Ǿ� ����
	/// </summary>
	/// <param name="_A">true : ����Ʈ A ����, false : ����Ʈ B ����</param>
	public void Button_Point(bool _A)
	{
		_Selected = true;

		SetPopup(Define.PopupState.MaxPlayer, "Choose your tools", "You choice : " + (_A ? "Wood" : "Stone"));

		NetworkManager.Net.SetPlayerSpawnPoint(_A);

		_pv.RPC("SelectPoint", RpcTarget.All, _A);
	}
	#endregion

	#region Popup
	/// <summary> �˾�â�� ���� </summary>
	private Define.PopupState _PopupState;

	/// <summary> �˾� �ؽ�Ʈ ���� </summary>
	/// <param name="_state">�˾� ����</param>
	/// <param name="_subjectText">�˾� ����</param>
	private void SetPopup(Define.PopupState _state, string _headerText, string _subjectText)
	{
		_Window_Popup.SetActive(true);
		_PopupState = _state;

		_Text_Popup_Header.text = _headerText;

		// ������̶�� �÷��̾ ���� ��� ���� �ִ��� ���
		if (_state == Define.PopupState.Wait) { _Text_Popup_Subject.text = SetJoinRoomPlayerCount(); }
		else { _Text_Popup_Subject.text = _subjectText; }

		// �÷��̾ ��� ������ �������� ���� ��ư ����
		OnPointButton(_state == Define.PopupState.MaxPlayer);
		
	}
	#endregion

	// �÷��̾ �濡 ������ ����
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.Log(newPlayer.NickName + " ����.");

		SetJoinRoomPlayerCount();

		_pv.RPC("JoinPlayer", RpcTarget.All);
	}

	/// <summary> �÷��̾ �濡 ���� ��� ���� �ִ��� ��� </summary>
	private string SetJoinRoomPlayerCount() { return "Player : " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers; }

	/// <summary> ĵ���� ����Ʈ ���� ��ư�� ��ü </summary>
	/// <param name="_on">true : ����Ʈ ���� ��ư���� ��ü, false : ĵ�� ��ư���� ��ü</param>
	private void OnPointButton(bool _on)
	{
		_Object_PointButton.SetActive(_on);
		_Object_CancelButton.SetActive(!_on);
	}

	// �÷��̾ �濡�� ������ ����
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.Log(otherPlayer.NickName + " ����.");
		InitSelectPoint();
		SetJoinRoomPlayerCount();
	}

	/// <summary> ������ �������� �ʱ�ȭ </summary>
	private void InitSelectPoint()
	{
		_Image_Select_PointA.gameObject.SetActive(false);
		_Image_Select_PointB.gameObject.SetActive(false);
		_Selected = false;
		_Selected_PointA = false;
		_Selected_PointB = false;
		SetPopup(Define.PopupState.Wait, "Wait for Player", "Player : 0 / 2");
	}

	/// <summary> �濡 �÷��̾ �ִ�� ���Դٸ� ĵ�� ��ư�� ����Ʈ ���� ��ư���� ��ü </summary>
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
	/// <summary> �÷��̾ ����Ʈ�� �����ϸ� ������ ���� ��� ������ �Ϸ� �Ǿ��ٸ� ������ �÷����� ������ �̵� </summary>
	/// <param name="_A">true : ����Ʈ A ����, false : ����Ʈ B ����</param>
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
