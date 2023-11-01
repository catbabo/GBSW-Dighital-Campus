using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScene : SceneBase
{
    [SerializeField] private GameObject _Window_Main;
    [SerializeField] private GameObject _Window_Popup;

    [SerializeField] private TMP_InputField _Input_RoomCode;
    [SerializeField] private TMP_InputField _Input_NickName;

    [SerializeField] private TMP_Text _Text_Popup_Header;
    [SerializeField] private TMP_Text _Text_Popup_Subject;

    [SerializeField] private Image _Image_Select_PointA;
    [SerializeField] private Image _Image_Select_PointB;

    [SerializeField] private GameObject _Object_CancelButton;
    [SerializeField] private GameObject _Object_PointButton;

    [SerializeField] private Sprite _Sprite_Check;
    [SerializeField] private Sprite _Sprite_X;

    private PhotonView _pv;

    private bool _Selected = false;
    private bool _Selected_PointA = false, _Selected_PointB = false;


    public override void Init()
    {
        _scene = gameObject;
        _type = Define.Scene.Lobby;
        _name = "Lobby";

        _pv = gameObject.GetComponent<PhotonView>();
        InitUI();
        InitEvent();
    }

    private void InitEvent()
    {
        Managers.Event.AddMatchRoomButton(MatchRoomButton);
    }

    public override void StartLoad() { OnLoad(); }

    protected override void OnLoad()
    {
        if (Managers.Network._forceOut)
        {
            SetPopup(Define.PopupState.Warning, "Warning", "Your partner is out.");
            Managers.Network.SetForceOut(false);
        }
        else { InitWindow(); }
    }
    
    private void InitWindow()
    {
        _Window_Main.SetActive(true);
        _Window_Popup.SetActive(false);
    }

    private void MatchRoomButton()
    {
        if (_Input_RoomCode.text.Length <= 0 || _Input_NickName.text.Length <= 0)
        {
            SetPopup(Define.PopupState.Warning, "Warning", "Nothing Enter");
            return;
        }
    }

    // �� ����� ����
    public void OnJoinedRoom()
    {
        SetPopup(Define.PopupState.Wait, "Wait for Player", "Player : 0 / 2");
        _pv.RPC("JoinPlayer", RpcTarget.All);
    }

    /// <summary>
    /// �˾�â ���� �ٸ� �÷��̾ ��ٸ��� ���̶�� Discnnect �� ����
    /// '�˾�â�� Cancel��ư�� ����Ǿ� ����
    /// </summary>
    public void Button_Cancel()
    {
        if (_PopupState == Define.PopupState.Wait)
        { Managers.Network.LeaveRoom(); }

        _Window_Popup.SetActive(false);
    }

    /// <summary>
    /// ������ ��ġ�� ��Ʈ��ũ �Ŵ����� ����
    /// ���� ȭ���� A, B ��ư�� ����Ǿ� ����
    /// </summary>
    /// <param name="_A">true : ����Ʈ A ����, false : ����Ʈ B ����</param>
    public void Button_Point(bool _A)
    {
        SetPopup(Define.PopupState.MaxPlayer, "Choose your tools", "You choice : " + (_A ? "Wood" : "Stone"));

        Managers.Network.SetPlayerSpawnPoint(_A);

        _Selected = true;

        _pv.RPC("SelectPoint", RpcTarget.All, _A);
    }



    /// <summary> �˾�â�� ���� </summary>
    private Define.PopupState _PopupState;

    /// <summary> �˾� �ؽ�Ʈ ���� </summary>
    /// <param name="_state">�˾� ����</param>
    /// <param name="_headerText">�˾� ����</param>
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

    public void OnPlayerEnteredRoom()
    {
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
    public void OnPlayerLeftRoom()
    {
        SetJoinRoomPlayerCount();
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
            SetPopup(Define.PopupState.MaxPlayer, "Choose your tools", "You choice : ");
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