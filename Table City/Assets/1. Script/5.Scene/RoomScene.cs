using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomScene : SceneBase
{
    public GameObject _Object_PlayerA = null;
    public GameObject _Object_PlayerB = null;

    [SerializeField] private TMP_Text _Text_Popup_Header;
    [SerializeField] private TMP_Text _Text_Popup_Subject;

    [SerializeField] private Image[] Image_Job = new Image[2];

    [SerializeField] private GameObject _Object_PointButton;
    private ReadyButton _readyButton;
    private LeaveButton _leaveButton;

    [SerializeField] private Sprite _Sprite_Check;
    [SerializeField] private Sprite _Sprite_X;

    private bool _Selected = false;
    private bool _selectedJobA = false;

    private Define.PopupState _popupState;

    public override void Init()
    {
        _scene = gameObject;
        _type = Define.Scene.Room;
        _name = "Room";

        InitUI();
        InitEvent();
    }

    private void InitEvent()
    {
        Managers.Event.AddReadyButton(ReadyButton);
        Managers.Event.AddLeaveButton(LeaveButton);
    }

    protected override void InitUI()
    {
        _readyButton = transform.Find("Ready").GetComponent<ReadyButton>();
        _leaveButton = transform.Find("Leave").GetComponent<LeaveButton>();
    }

    public override void StartLoad() { OnLoad(); }

    protected override void OnLoad()
    {
        SetPopup(Define.PopupState.Wait);
        _readyButton.Init();
        InitSelectPoint();
        //SpawnPlayer();
    }

    public override void LeftScene()
    {
        InitSelectPoint();
        _readyButton.Init();
    }

    private void SetPopup(Define.PopupState _state)
    {
        _popupState = _state;
        string header = "", subject = "";
        switch (_popupState)
        {
            case Define.PopupState.Wait:
                header = "Wait";
                subject = "Press Ready for play";
                break;

            case Define.PopupState.Ready:
                header = "Ready";
                subject = Managers.Network.GetJoinRoomPlayerCount();
                break;

            case Define.PopupState.ReadyPlease:
                header = "Ready!!";
                subject = "Please Ready All Players";
                break;
            case Define.PopupState.MaxPlayer:
                header = "Choose your tool";
                subject = "You choice : " + (_Selected ? (_selectedJobA ? "Wood" : "Stone") : "?");
                break;
        }
        _Text_Popup_Header.text = header;
        _Text_Popup_Subject.text = subject;
    }

    private void InitSelectPoint()
    {
        _readyButton.gameObject.SetActive(true);
        _leaveButton.gameObject.SetActive(true);

        _Object_PointButton.SetActive(false);

        _Object_PointButton.transform.Find("Button_PointA").GetComponent<Button>().interactable = true;
        _Object_PointButton.transform.Find("Button_PointB").GetComponent<Button>().interactable = true;
        Image_Job[0].gameObject.SetActive(false);
        Image_Job[1].gameObject.SetActive(false);

        _Selected = false;
        _selectedJobA = false;
    }

    private void SpawnPlayer()
    {
        Transform spawnPoint = GameObject.Find("SpawnPoint").transform;
        Transform playerPoint, workbenchPoint;
        string workbenchName;

        if (Managers.Network.IsPlayerTeamA())
        {
            playerPoint = spawnPoint.Find("Spawn_Player").Find("Point_A");
            workbenchName = "0. Player/PlayerA_Workbench";
            workbenchPoint = spawnPoint.Find("Spawn_Workbench").Find("Point_A");
        }
        else
        {
            playerPoint = spawnPoint.Find("Spawn_Player").Find("Point_B");
            workbenchName = "0. Player/PlayerB_Workbench";
            workbenchPoint = spawnPoint.Find("Spawn_Workbench").Find("Point_B");
        }

        Managers.Instance.SpawnObject("0. Player/Player_Prefab", playerPoint);
        Managers.Instance.SpawnObject(workbenchName, workbenchPoint);
    }


    public void SetPlayerObject(GameObject _player, bool _pointA)
    {
        if (_pointA) { _Object_PlayerA = _player; }
        else { _Object_PlayerB = _player; }
    }

    public void JobButton(bool _A)
    {
        Debug.Log("JobButtonStart");
        _Selected = true;
        _selectedJobA = _A;
        SetPopup(Define.PopupState.MaxPlayer);
        Managers.Network.SelectJobSync(_A);
        Debug.Log("JobButtonEnd");
    }

    private void ReadyButton(bool isReady)
    {

        if(isReady)
        {
            OnInGameStartButton();
        }
        else
        {
            _readyButton.SwapButton();
            SetPopup(Define.PopupState.Ready);
        }
    }

    private void OnInGameStartButton()
    {
        if(Managers.Network.IsCanStartInGame())
        {
            _readyButton.SwapButton();
            bool isSolo = Managers.Network.IsSolo();
            Managers.Event.ExcuteAllReady(isSolo);
        }
        else
        {
            Managers.Network.ReadyMention();
        }
    }

    public void PleaseReady()
    {
        SetPopup(Define.PopupState.ReadyPlease);
    }

    private void LeaveButton()
    {
        Managers.Network.LeaveRoom();
        Managers.Scene.LoadScene(Define.Scene.Lobby);
    }

    public void OnPlayerLeftRoom()
    {
        _readyButton.UpdateButon();
        InitSelectPoint();
        SetPopup(Define.PopupState.Ready);
    }

    public void OnPlayerEnteredRoom()
    {
        UpdateReadyPopup();
    }

    public void UpdateReadyPopup()
    {
        if (_popupState == Define.PopupState.Ready)
            SetPopup(Define.PopupState.Ready);
    }

    public void ShowJobButton()
    {
        InitSelectPoint();
        SetPopup(Define.PopupState.MaxPlayer);
        _Object_PointButton.SetActive(true);
        _readyButton.gameObject.SetActive(false);
        _leaveButton.gameObject.SetActive(false);
    }

    public void SelectJob(bool _A)
    {
        Debug.Log("SelectJobStart");
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

        Managers.Network.InGame();
        Debug.Log("SelectJobStart");
    }

    public void InGameStart()
    {
        Debug.Log("Load In Game");
        Managers.Scene.LoadScene(Define.Scene.InGame);
    }    
}
