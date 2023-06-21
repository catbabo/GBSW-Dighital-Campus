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
	// Ÿ��Ʋ ȭ�� ( ���� ���� �� ���� ���� ��ư )
	public GameObject _Window_Title;

	// ���� ȭ�� ( �� �̸��� �г��� ���� �� ���� ���� )
	public GameObject _Window_Main;

	// ���̳� �г����� ���� ���� ������ ������ �˾�
	public GameObject _Window_Popup;
	#endregion

	#region InputField
	// �� �ڵ�
	public TMP_InputField _Input_RoomCode;

	// �г���
	public TMP_InputField _Input_NickName;
	#endregion

	#region Text
	// �˾�â ���� �ؽ�Ʈ
	public TMP_Text _Text_Popup_Title;

	// �˾�â ���� �ؽ�Ʈ
	public TMP_Text _Text_Popup_Subject;
	#endregion

	#region Object_UI
	// ĵ�� ��ư
	public GameObject _CancelButton;

	// ����Ʈ ���� ��ư
	public GameObject _PointButton;
	#endregion

	// ���� ��
	private PhotonView _pv;

	// ������ ���� ����Ʈ
	private bool _SelectedA = false, _SelectedB = false;

	private void Start()
	{
		_pv = gameObject.GetComponent<PhotonView>();
		InitWindow();
	}

	// UI â �ʱ�ȭ
	private void InitWindow()
	{
		_Window_Title.SetActive(true);
		_Window_Main.SetActive(false);
	}

	#region Button
	// Ÿ��Ʋ ȭ�鿡�� ���� ȭ������ �̵� �� ������ ���� ����
	// Ÿ��Ʋ ȭ���� START��ư�� ����Ǿ� ����
	public void Button_Start()
	{
		NetworkManager.Net.Connect();
		_Window_Title.SetActive(false);
		_Window_Main.SetActive(true);
	}

	// �� �̸��� �г����� NetworkManager�� �����ϰ� ���� �����ϰų� ����
	// ���� ȭ���� JOIN��ư�� ����Ǿ� ����
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

	// �� ����� ����
	public override void OnJoinedRoom()
	{
		print("�� ���� ����!");
		SetPopup(PopupState.Wait);
		_pv.RPC("JoinPlayer", RpcTarget.All);
	}

	// �÷��̾ ��ٸ��� ���̶�� Disconnect �ƴ϶�� �˾�â ����
	// �˾�â�� Cancel��ư�� ����Ǿ� ����
	public void Button_Cancel()
	{
		if (_PopupState == PopupState.Wait) { NetworkManager.Net.LeaveRoom(); }
		_Window_Popup.SetActive(false);
	}

	// ������ ����Ʈ�� �����ϴ� �Լ�
	// ���� ȭ���� A, B ��ư�� ����Ǿ� ����
	public void Button_Point(bool _A)
	{
		NetworkManager.Net.SetPlayerSpawnPoint(_A);
		_pv.RPC("SelectPoint", RpcTarget.All, _A);
	}
	#endregion

	#region Popup
	// �˾��� ����
	private PopupState _PopupState;
	private enum PopupState
	{
		Wait,
		Warning,
	}

	// �˾� �ؽ�Ʈ ���� ( �˾� ����, �˾� ����, �˾� ���� )
	private void SetPopup(PopupState _state, string _subjectText = null)
	{
		_Window_Popup.SetActive(true);
		_PointButton.SetActive(false);

		_PopupState = _state;
		_Text_Popup_Title.text = _state.ToString();
		
		// �÷��̾ ���� ��� ���� �ִ��� ���
		if (_state == PopupState.Wait) { SetJoinRoomPlayerCount(_Text_Popup_Subject); }
		else { _Text_Popup_Subject.text = _subjectText; }
	}
	#endregion

	// �÷��̾ �濡 ������ ����
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		print(newPlayer.NickName + " ����.");

		SetJoinRoomPlayerCount(_Text_Popup_Subject);

		_pv.RPC("JoinPlayer", RpcTarget.All);

	}

	// �÷��̾ ���� ��� ���� �ִ��� ��� ( ����� �ؽ�Ʈ )
	private void SetJoinRoomPlayerCount(TMP_Text _text) => _text.text = "Player : " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

	// ���� ����Ʈ ���� ��ư�� ���
	private void OnPointButton(bool _on)
	{
		_PointButton.SetActive(_on);
		_CancelButton.SetActive(!_on);
	}

	// �÷��̾ �濡�� ������ ����
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		print(otherPlayer.NickName + " ����.");
		OnPointButton(false);
		SetJoinRoomPlayerCount(_Text_Popup_Subject);
	}

	// �� �ִ� �ο����� �÷��̾ ���Դٸ� ���� ����Ʈ�� �����ϴ� ��ư�� ����.
	[PunRPC]
	private void JoinPlayer()
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
		{
			OnPointButton(true);
		}
	}

	// �÷��̾ ������ ��ư ���� ������ �Ϸ� �Ǿ��ٸ� �÷��� ������ �̵�
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
