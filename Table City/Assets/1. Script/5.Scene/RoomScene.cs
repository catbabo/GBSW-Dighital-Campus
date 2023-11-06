using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    private Transform playerPoint;

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
        UpdatePopup(Define.PopupState.Wait);
        _readyButton.Init();
        ResetButton();
    }

    private void SpawnPlayer()
    {
        Managers.player.Destroy();
        Transform spawnPoint = GameObject.Find("SpawnPoint").transform;
        playerPoint = spawnPoint.Find("Spawn_Player");
        string path = "";

        if (Managers.Network.IsSideA())
        { path = "Point_A"; }
        else
        { path = "Point_B"; }
        GameObject go = Managers.Instance.SpawnObject("0. Player/Player_Prefab", playerPoint.Find(path));
        Managers.player = go.GetComponent<PlayerController>();
        //Managers.onLinePlayer.SetNickName(Managers.localPlayer.GetNickName());
    }

    public void OnDataSync()
    {
        //SpawnPlayer();
        Managers.player.Destroy();
    }

    public void OnCreateRoom()
    {
        //SpawnPlayer();
        Managers.player.Destroy();
    }

    public override void LeftScene()
    {
        ResetButton();
        _readyButton.Init();
    }

    private void UpdatePopup(Define.PopupState _state)
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
            case Define.PopupState.SelectJob:
                header = "Choose your tool";
                subject = "You choice : " + (_Selected ? (_selectedJobA ? "Wood" : "Stone") : "?");
                break;
        }
        _Text_Popup_Header.text = header;
        _Text_Popup_Subject.text = subject;
    }

    private void ResetButton()
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
        UpdatePopup(Define.PopupState.SelectJob);
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
            UpdatePopup(Define.PopupState.Ready);
        }
    }

    private void OnInGameStartButton()
    {
        if(Managers.Network.IsCanStartInGame())
        {
            bool isSolo = Managers.Network.IsSolo();
            Managers.Event.ExcuteAllReady(isSolo);
        }
        else
        {
            ReadyMention();
            Managers.Network.ReadyMention();
        }
    }

    public void ReadyMention()
    {
        UpdatePopup(Define.PopupState.ReadyPlease);
    }

    private void LeaveButton()
    {
        Managers.Scene.LoadScene(Define.Scene.Lobby);
    }

    public void OnPlayerLeftRoom()
    {
        ResetButton();
        Managers.UI.ShowPopup("Master Exit", "Now You Are Master");
        _readyButton.UpdateToMasterButon();
        UpdatePopup(Define.PopupState.Ready);
    }

    public void OnPlayerEnteredRoom()
    {
        UpdateReadyPopup();
    }

    public void UpdateReadyPopup()
    {
        if (_popupState == Define.PopupState.Ready)
            UpdatePopup(Define.PopupState.Ready);
    }

    public void ShowJobButton()
    {
        ResetButton();
        _readyButton.gameObject.SetActive(false);
        _leaveButton.gameObject.SetActive(false);
        _Object_PointButton.SetActive(true);
        UpdatePopup(Define.PopupState.SelectJob);
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
        Managers.Sound.BGMStop();
        Managers.Scene.LoadScene(Define.Scene.InGame);
    }    
}
