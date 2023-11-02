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

    [SerializeField] private Image[] Image_Job = new Image[2];

    [SerializeField] private GameObject _Object_CancelButton;
    [SerializeField] private GameObject _Object_PointButton;

    [SerializeField] private Sprite _Sprite_Check;
    [SerializeField] private Sprite _Sprite_X;

    private PhotonView _pv;

    private bool _Selected = false;
    private bool _Selected_PointA = false, _Selected_PointB = false;

    private Define.PopupState _PopupState;

    public override void Init()
    {
        _scene = gameObject;
        _type = Define.Scene.Lobby;
        _name = "Lobby";

        _pv = gameObject.GetComponent<PhotonView>();
        InitUI();
        InitEvent();
    }

    protected override void InitUI()
    {
        _Input_RoomCode.GetComponent<SetRoomCode>().Init();
        _Input_NickName.GetComponent<SetNickName>().Init();
    }

    private void InitEvent()
    {
        Managers.Event.AddMatchRoomButton(MatchRoomButton);
        Managers.Event.AddCancleButton(CancleButton);
        Managers.Event.AddJobButton(JobButton);
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
        if (Managers.Network.IsCanCreateRoom())
        {
            SetPopup(Define.PopupState.Warning, "Warning", "Nothing Enter");
        }
    }

    private void CancleButton()
    {
        if (_PopupState == Define.PopupState.Wait)
        { Managers.Network.LeaveRoom(); }

        _Window_Popup.SetActive(false);
    }

    private void JobButton(bool _A)
    {
        SetPopup(Define.PopupState.MaxPlayer, "Choose your tools", "You choice : " + (_A ? "Wood" : "Stone"));
        _Selected = true;
    }

    public void OnJoinedRoom()
    {
        SetPopup(Define.PopupState.Wait, "Wait for Player", "Player : 0 / 2");
    }

    /// <summary> 방에 플레이어가 최대로 들어왔다면 캔슬 버튼을 포인트 선택 버튼으로 교체 </summary>
    public void JoinPlayer()
    {
        if (Managers.Network.IsFullPlayers())
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

    /// <summary> 선택한 시작지점 초기화 </summary>
    private void InitSelectPoint()
    {
        _Object_PointButton.transform.Find("Button_PointA").GetComponent<Button>().interactable = true;
        _Object_PointButton.transform.Find("Button_PointB").GetComponent<Button>().interactable = true;
        Image_Job[0].gameObject.SetActive(false);
        Image_Job[1].gameObject.SetActive(false);
        _Selected = false;
        _Selected_PointA = false;
        _Selected_PointB = false;
    }

    /// <summary> 캔슬과 포인트 선택 버튼을 교체 </summary>
    /// <param name="_on">true : 포인트 선택 버튼으로 교체, false : 캔슬 버튼으로 교체</param>
    private void OnPointButton(bool _on)
    {
        _Object_PointButton.SetActive(_on);
        _Object_CancelButton.SetActive(!_on);
    }


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
        if (_state == Define.PopupState.Wait)
        {
            _Text_Popup_Subject.text = Managers.Network.GetJoinRoomPlayerCount();
        }
        else { _Text_Popup_Subject.text = _subjectText; }

        // 플레이어가 모두 들어오면 시작지점 선택 버튼 등장
        OnPointButton(_state == Define.PopupState.MaxPlayer);
    }

    /// <summary> 플레이어가 포인트를 선택하면 선택지 제거 모두 선택이 완료 되었다면 게임을 플레이할 씬으로 이동 </summary>
    /// <param name="_A">true : 포인트 A 선택, false : 포인트 B 선택</param>
    public void SelectJob(bool _A)
    {
        _Selected_PointA = _A;
        _Selected_PointB = !_A;
        
        int index = 0;
        if (_A)
            index = 0;
        else
            index = 1;

        Image_Job[index].gameObject.SetActive(true);
        if (_Selected)
        {
            _Object_PointButton.transform.Find("Button_PointA").GetComponent<Button>().interactable = false;
            _Object_PointButton.transform.Find("Button_PointB").GetComponent<Button>().interactable = false;
            Image_Job[index].sprite = _Sprite_Check;
            Image_Job[index].color = Color.green;
            _Selected = false;
        }
        else
        {
            Image_Job[index].sprite = _Sprite_X;
            Image_Job[index].color = Color.red;
            if (_A)
            {
                _Object_PointButton.transform.Find("Button_PointA").GetComponent<Button>().interactable = false;
            }
            else
            {
                _Object_PointButton.transform.Find("Button_PointB").GetComponent<Button>().interactable = false;
            }
        }

        if (_Selected_PointA && _Selected_PointB)
        {
            if (Managers.Network.IsMaster())
            {
                if (Managers.Network.IsCanCreateRoom())
                {
                    _pv.RPC("StartGame", RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    private void StartGame()
    {
        Managers.Scene.LoadScene(Define.Scene.Room);
    }
}