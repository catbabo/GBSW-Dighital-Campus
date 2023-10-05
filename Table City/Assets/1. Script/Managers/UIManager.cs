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
    /// Ÿ��Ʋ ȭ�鿡�� ���� ȭ������ �̵� �� ������ ���� ����
    /// 'Ÿ��Ʋ ȭ���� START��ư�� ����Ǿ� ����'
    /// </summary>
    public void Button_Start()
    {
        Managers._network.Connect();
        ShowUI(Define.UI.lobby);
    }

    /// <summary>
    /// �� �̸��� �г����� NetworkManager�� �����ϰ� ���� �����ϰų� ����
    /// '���� ȭ���� JOIN��ư�� ����Ǿ� ����
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

    // �� ����� ����
    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� ����!");
        SetPopup(Define.PopupState.Wait);
        _pv.RPC("JoinPlayer", RpcTarget.All);
    }

    /// <summary>
    /// �˾�â ���� �ٸ� �÷��̾ ��ٸ��� ���̶�� Discnnect �� ����
    /// '�˾�â�� Cancel��ư�� ����Ǿ� ����
    /// </summary>
    public void Button_Cancel()
    {
        if (_PopupState == Define.PopupState.Wait) { Managers._network.LeaveRoom(); }
        CloseUI(Define.LobbyUI.selectTeam);
    }

    /// <summary>
    /// ������ ��ġ�� ��Ʈ��ũ �Ŵ����� ����
    /// ���� ȭ���� A, B ��ư�� ����Ǿ� ����
    /// </summary>
    /// <param name="_A">true : ����Ʈ A ����, false : ����Ʈ B ����</param>
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

        /*�׽�Ʈ �κ�
        ButtonData[] test = new ButtonData[2];
        test[0].text = "Yes";
        test[0].action = () => T();
        test[1].text = "No";
        test[1].action = () => DeletePopup();

        UsePopup("������", test,transform.position+new Vector3(0,0,800),Quaternion.identity);
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
    /*�׽�Ʈ �κ�
    public void T()
    {
        ButtonData[] test = new ButtonData[2];
        test[0].text = "Yes";
        test[0].action = () => T();
        test[1].text = "No";
        test[1].action = () => DeletePopup();

        UsePopup("������", test, transform.position + new Vector3(0, 0, 800), Quaternion.identity);
    }
    */
}

public struct ButtonData
{
    public string text { get; set; }
    public Action action { get; set; }
}
