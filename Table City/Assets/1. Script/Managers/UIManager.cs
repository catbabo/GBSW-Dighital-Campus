using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Photon.Pun;

public class UIManager : PunManagerBase
{
    private struct UIData
    {
        public string name;
        public UnityEngine.Object ui;
        public GameObject obj;
    }

    private PhotonView _pv;

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

    [SerializeField]
    //private List<UnityEngine.Object> _uis = new List<UnityEngine.Object>();
    private Dictionary<Type, List<UIData>> _uis = new Dictionary<Type, List<UIData>>();
    public override void Init()
    {
        _uis.Add(typeof(TMP_Text), new List<UIData>());
    }

    public void ShowPannel(string name)
    {
        foreach (UIData ui in _uis[typeof(Pannel)])
        {
            ui.obj.SetActive(false);
        }
        GetUI<GameObject>(name).SetActive(true);
    }

    public void ShowUIOnPopup(Define.PopupState target)
    {
        switch (target)
        {

            /// <summary> ĵ���� ����Ʈ ���� ��ư�� ��ü </summary>
            /// <param name="_on">true : ����Ʈ ���� ��ư���� ��ü, false : ĵ�� ��ư���� ��ü</param>
            case Define.PopupState.MaxPlayer:
                //_Object_PointButton.SetActive(true);
                //_Object_CancelButton.SetActive(false);
                break;
        }
    }

    public void CloseUIOnPopup(Define.PopupState target)
    {
        switch (target)
        {

            /// <summary> ĵ���� ����Ʈ ���� ��ư�� ��ü </summary>
            /// <param name="_on">true : ����Ʈ ���� ��ư���� ��ü, false : ĵ�� ��ư���� ��ü</param>
            case Define.PopupState.MaxPlayer:
                //_Object_PointButton.SetActive(false);
                //_Object_CancelButton.SetActive(true);
                break;
        }
    }

    public void CloseUI(string name)
    {
        GetUI<GameObject>(name).SetActive(false);
    }

    public void AddUI<T>(string target) where T : UnityEngine.Object
    {
        if(!_uis.ContainsKey(typeof(T)))
        {
            _uis.Add(typeof(T), new List<UIData>());
        }

        if(IsContainUI<T>(target))
        {
            Debug.LogError("It's aleady In List");
            return;
        }
            
        SetUI<T>(target);
    }

    public void SetUI<T>(string target) where T : UnityEngine.Object
    {
        UIData ui;

        ui.obj = Utill.Find(target);
        if (ui.obj == null)
            Debug.LogError("No Object");

        ui.ui = ui.obj.GetComponent<T>();
        if (ui.ui)
            Debug.LogError("No Component");

        ui.name = target;

        _uis[typeof(T)].Add(ui);
    }

    private bool IsContainUI<T>(string target) where T : UnityEngine.Object
    {
        if (!HasKey<T>())
        {
            Debug.LogError("No UI");
            return false;
        }

        if (GetUIData<T>(target).obj == null)
        {
            Debug.LogError("No UI");
            return false;
        }

        return true;
    }

    private bool HasKey<T>() where T : UnityEngine.Object
    {
        if (!_uis.ContainsKey(typeof(T)))
        {
            Debug.LogError("No Key");
            return false;
        }

        return true;
    }

    private UIData GetUIData<T>(string target) where T : UnityEngine.Object
    {
        for (int i = 0; i < _uis.Count; i++)
        {
            if(_uis[typeof(T)][i].name.Equals(target))
                return _uis[typeof(T)][i];
        }

        Debug.LogError("No UI");
        return new UIData();
    }

    public T GetUI<T>(string name) where T : UnityEngine.Object
    {
        return GetUIData<T>(name).ui as T;
    }


    public GameObject GetUIObject<T>(string name) where T : UnityEngine.Object
    {
        return GetUIData<T>(name).obj;
    }

    public void UIActive<T>(string name, bool active) where T : UnityEngine.Object
    {
        GetUIObject<T>(name).SetActive(active);
    }

    public void SetText(string name, string text)
    {
        GetUI<TMP_Text>(name).text = text;
    }

    public void SetImageColor(string name, Color color)
    {
        GetUI<Image>(name).color = color;
    }


    #region Button
    /// <summary>
    /// Ÿ��Ʋ ȭ�鿡�� ���� ȭ������ �̵� �� ������ ���� ����
    /// 'Ÿ��Ʋ ȭ���� START��ư�� ����Ǿ� ����'
    /// </summary>
    public void Button_Start()
    {
        Managers._network.Connect();
        ShowPannel(Define.UI.lobby);
    }

    /// <summary>
    /// �� �̸��� �г����� NetworkManager�� �����ϰ� ���� �����ϰų� ����
    /// '���� ȭ���� JOIN��ư�� ����Ǿ� ����
    /// </summary>
    public void Button_CreateOrJoin()
    {
        if (GetUI<TMP_InputField>(Define.UI.roomCodeField).text.Length <= 0 ||
            GetUI<TMP_InputField>(Define.UI.nickNameField).text.Length <= 0)
        {
            Managers._lobby.SetPopup(Define.PopupState.Warning);
            return;
        }

        Managers._network.SetRoomCode(GetUI<TMP_InputField>(Define.UI.roomCodeField).text);
        Managers._network.SetNickName(GetUI<TMP_InputField>(Define.UI.nickNameField).text);

        Managers._network.JoinLobby();
    }

    // �� ����� ����
    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� ����!");
        Managers._lobby.SetPopup(Define.PopupState.Wait);
        _pv.RPC("JoinPlayer", RpcTarget.All);
    }

    Define.PopupState _PopupState;
    /// <summary>
    /// �˾�â ���� �ٸ� �÷��̾ ��ٸ��� ���̶�� Discnnect �� ����
    /// '�˾�â�� Cancel��ư�� ����Ǿ� ����
    /// </summary>
    public void Button_Cancel()
    {
        if (_PopupState == Define.PopupState.Wait) { Managers._network.LeaveRoom(); }
        CloseUI(Define.UI.match);
    }

    private bool _Selected = false;
    /// <summary>
    /// ������ ��ġ�� ��Ʈ��ũ �Ŵ����� ����
    /// ���� ȭ���� A, B ��ư�� ����Ǿ� ����
    /// </summary>
    /// <param name="_A">true : ����Ʈ A ����, false : ����Ʈ B ����</param>
    public void Button_Point(bool _A)
    {
        Managers._lobby.SetPopupInfo(Define.PopupState.MaxPlayer, "Choose your tools", "You choice : " + (_A ? "Wood" : "Stone"));
        Managers._lobby.SetPopup(Define.PopupState.MaxPlayer);

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
