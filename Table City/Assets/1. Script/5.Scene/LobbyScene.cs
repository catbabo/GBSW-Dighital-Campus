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

    // 방 입장시 실행
    public void OnJoinedRoom()
    {
        SetPopup(Define.PopupState.Wait, "Wait for Player", "Player : 0 / 2");
        _pv.RPC("JoinPlayer", RpcTarget.All);
    }

    /// <summary>
    /// 팝업창 종료 다른 플레이어를 기다리는 중이라면 Discnnect 후 종료
    /// '팝업창의 Cancel버튼과 연결되어 있음
    /// </summary>
    public void Button_Cancel()
    {
        if (_PopupState == Define.PopupState.Wait)
        { Managers.Network.LeaveRoom(); }

        _Window_Popup.SetActive(false);
    }

    /// <summary>
    /// 선택한 위치를 네트워크 매니저에 저장
    /// 메인 화면의 A, B 버튼에 연결되어 있음
    /// </summary>
    /// <param name="_A">true : 포인트 A 선택, false : 포인트 B 선택</param>
    public void Button_Point(bool _A)
    {
        SetPopup(Define.PopupState.MaxPlayer, "Choose your tools", "You choice : " + (_A ? "Wood" : "Stone"));

        Managers.Network.SetPlayerSpawnPoint(_A);

        _Selected = true;

        _pv.RPC("SelectPoint", RpcTarget.All, _A);
    }



    /// <summary> 팝업창의 상태 </summary>
    private Define.PopupState _PopupState;

    /// <summary> 팝업 텍스트 변경 </summary>
    /// <param name="_state">팝업 상태</param>
    /// <param name="_headerText">팝업 제목</param>
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

    public void OnPlayerEnteredRoom()
    {
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
    public void OnPlayerLeftRoom()
    {
        SetJoinRoomPlayerCount();
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
            SetPopup(Define.PopupState.MaxPlayer, "Choose your tools", "You choice : ");
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