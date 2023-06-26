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

	#region Object_UI
	/// <summary> ��� ��ư ������Ʈ </summary>
	public GameObject _CancelButton;

	/// <summary> ����Ʈ ���� ��ư ������Ʈ </summary>
	public GameObject _PointButton;
	#endregion

	/// <summary> ���� �� </summary>
	private PhotonView _pv;

	/// <summary> �÷��̾ ����Ʈ ������ �ߴ����� ���� </summary>
	private bool _SelectedA = false, _SelectedB = false;

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
			SetPopup(Define.PopupState.Warning, "Nothing Enter");
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
		SetPopup(Define.PopupState.Wait);
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
	private void SetPopup(Define.PopupState _state, string _subjectText = null)
	{
		_Window_Popup.SetActive(true);
		_PointButton.SetActive(false);

		_PopupState = _state;
		_Text_Popup_Header.text = _state.ToString();

		// �÷��̾ ���� ��� ���� �ִ��� ���
		if (_subjectText != null) { _Text_Popup_Subject.text = _subjectText; }
		else if (_state == Define.PopupState.Wait) { SetJoinRoomPlayerCount(); }
	}
	#endregion

	// �÷��̾ �濡 ������ ����
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		print(newPlayer.NickName + " ����.");

		SetJoinRoomPlayerCount();

		_pv.RPC("JoinPlayer", RpcTarget.All);

	}

	/// <summary> �÷��̾ �濡 ���� ��� ���� �ִ��� ��� </summary>
	private void SetJoinRoomPlayerCount() => SetPopup(Define.PopupState.Wait, "Player : " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers);

	/// <summary> ĵ���� ����Ʈ ���� ��ư�� ��ü </summary>
	/// <param name="_on">true : ����Ʈ ���� ��ư���� ��ü, false : ĵ�� ��ư���� ��ü</param>
	private void OnPointButton(bool _on)
	{
		_PointButton.SetActive(_on);
		_CancelButton.SetActive(!_on);
	}

	// �÷��̾ �濡�� ������ ����
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.Log(otherPlayer.NickName + " ����.");
		OnPointButton(false);
		SetJoinRoomPlayerCount();
	}

	/// <summary> �濡 �÷��̾ �ִ�� ���Դٸ� ĵ�� ��ư�� ����Ʈ ���� ��ư���� ��ü </summary>
	[PunRPC]
	private void JoinPlayer()
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
		{
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
		if(_A)
			{
			_SelectedA = true;
			_PointButton.transform.Find("Button_PointA").gameObject.SetActive(false);
			SetPopup(Define.PopupState.Wait, "You");
			if (_pv.IsMine) _PointButton.transform.Find("Button_Protector").gameObject.SetActive(true);
		}
		else
		{
			_SelectedB = true;
			_PointButton.transform.Find("Button_PointB").gameObject.SetActive(false);
			SetPopup(Define.PopupState.Wait, "You");
			if (_pv.IsMine) _PointButton.transform.Find("Button_Protector").gameObject.SetActive(true);
		}

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
	}
}
