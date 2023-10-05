using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Photon.Pun;

public class UIManager : ManagerBase
{
    private GameObject[] _uis = new GameObject[3];

    public void ShowUI(Define.UI target)
    {
        foreach (GameObject ui in _uis)
        {
            ui.SetActive(false);
        }
        _uis[(int)target].SetActive(true);
    }

    public void CloseUI(Define.UI target)
    {
        _uis[(int)target].SetActive(false);
    }

    public void SetUI(Define.UI index, string target)
    {
        _uis[(int)index] = Managers._find.Find(target);
    }


    #region Button
    /// <summary>
    /// 타이틀 화면에서 메인 화면으로 이동 및 마스터 서버 연결
    /// '타이틀 화면의 START버튼과 연결되어 있음'
    /// </summary>
    public void Button_Start()
    {
        Managers._network.Connect();
        ShowUI(Define.UI.lobby);
    }

    /// <summary>
    /// 방 이름과 닉네임을 NetworkManager에 전달하고 방을 입장하거나 생성
    /// '메인 화면의 JOIN버튼과 연결되어 있음
    /// </summary>
    public void Button_CreateOrJoin()
    {
        if (_Input_RoomCode.text.Length <= 0 || _Input_NickName.text.Length <= 0)
        {
            SetPopup(Define.PopupState.Warning);
            return;
        }

        Managers._network.SetRoomCode(_Input_RoomCode.text);
        Managers._network.SetNickName(_Input_NickName.text);

        Managers._network.JoinLobby();
    }

    // 방 입장시 실행
    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 성공!");
        SetPopup(Define.PopupState.Wait);
        _pv.RPC("JoinPlayer", RpcTarget.All);
    }

    /// <summary>
    /// 팝업창 종료 다른 플레이어를 기다리는 중이라면 Discnnect 후 종료
    /// '팝업창의 Cancel버튼과 연결되어 있음
    /// </summary>
    public void Button_Cancel()
    {
        if (_PopupState == Define.PopupState.Wait) { Managers._network.LeaveRoom(); }
        CloseUI(Define.LobbyUI.selectTeam);
    }

    /// <summary>
    /// 선택한 위치를 네트워크 매니저에 저장
    /// 메인 화면의 A, B 버튼에 연결되어 있음
    /// </summary>
    /// <param name="_A">true : 포인트 A 선택, false : 포인트 B 선택</param>
    public void Button_Point(bool _A)
    {
        SetPopup(Define.PopupState.MaxPlayer, "Choose your tools", "You choice : " + (_A ? "Wood" : "Stone"));

        Managers._network.SetPlayerSpawnPoint(_A);

        _Selected = true;

        _pv.RPC("SelectPoint", RpcTarget.All, _A);
    }
    #endregion
}

public class TestUIManager : ManagerBase
{
    Dictionary<string, GameObject> ui = new Dictionary<string, GameObject>();
    Stack<GameObject> order = new Stack<GameObject>();

    public override void Init()
    {
        if(ui.Count == 0) ResourceLoad(ui, "2.Prefab/2.UI");

        /*테스트 부분
        ButtonData[] test = new ButtonData[2];
        test[0].text = "Yes";
        test[0].action = () => T();
        test[1].text = "No";
        test[1].action = () => DeletePopup();

        UsePopup("선택해", test,transform.position+new Vector3(0,0,800),Quaternion.identity);
        */

    }
    private void ResourceLoad(Dictionary<string, GameObject> dictionary, string filePath)
    {

        GameObject[] gameObject = Resources.LoadAll<GameObject>(filePath);

        foreach (GameObject oneGameObject in gameObject)
        {
            dictionary.Add(oneGameObject.name, oneGameObject);
        }
    }
    public void UsePopup(String text, ButtonData[] buttonData, Vector3 pos, Quaternion rotation)
    {
        GameObject canvas = Instantiate(ui["Canvas"], pos, rotation);
        GameObject popupGameObject = Instantiate(ui["Popup"], canvas.transform);
        Popup popupScript = popupGameObject.GetComponent<Popup>();
        popupScript.text.text = text;

        foreach (ButtonData OnebuttonData in buttonData)
        {
            GameObject button = Instantiate(ui["Button"], popupScript.buttonAll);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = OnebuttonData.text;
            button.GetComponent<Button>().onClick.AddListener(() => OnebuttonData.action());
        }

        order.Push(canvas);
    }
    public void DeletePopup()
    {
        GameObject set =  order.Pop();
        Destroy(set);
    }
    /*테스트 부분
    public void T()
    {
        ButtonData[] test = new ButtonData[2];
        test[0].text = "Yes";
        test[0].action = () => T();
        test[1].text = "No";
        test[1].action = () => DeletePopup();

        UsePopup("선택해", test, transform.position + new Vector3(0, 0, 800), Quaternion.identity);
    }
    */
}

public struct ButtonData
{
    public string text { get; set; }
    public Action action { get; set; }
}
