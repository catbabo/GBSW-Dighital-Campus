using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class ButtonController : MonoBehaviourPunCallbacks
{

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

    /// <summary> �÷��̾ ������ �ߴ����� ���� </summary>
    private bool _Selected = false;
    /// <summary> �÷��̾ ����Ʈ ������ �ߴ����� ���� </summary>
    private bool _Selected_PointA = false, _Selected_PointB = false;


    /// <summary> ���� �� </summary>
    private PhotonView _pv;


    /// <summary> ������ �������� �ʱ�ȭ </summary>
    private void InitSelectPoint()
    {
        _Object_PointButton.transform.Find("Button_PointA").GetComponent<Button>().interactable = true;
        _Object_PointButton.transform.Find("Button_PointB").GetComponent<Button>().interactable = true;
        Managers.UI.CloseUI<Image>("PointAImage");
        Managers.UI.CloseUI<Image>("PointBImage");
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
            //SetPopup(Define.PopupState.MaxPlayer);
            Managers.UI.ShowUIOnPopup(Define.PopupState.MaxPlayer);
        }
        else
        {
            Managers.UI.CloseUIOnPopup(Define.PopupState.MaxPlayer);
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
                Managers.UI.ShowUI<Image>("Image_Select_PointA");
                Managers.UI.GetUI<Image>("Image_Select_PointA").sprite = _Sprite_Check;
                Managers.UI.SetImageColor("Image_Select_PointA", Color.green);
            }
            else
            {
                _Selected_PointB = true;
                Managers.UI.ShowUI<Image>("Image_Select_PointB");
                Managers.UI.GetUI<Image>("Image_Select_PointB").sprite = _Sprite_Check;
                Managers.UI.SetImageColor("Image_Select_PointB", Color.green);
            }
            _Selected = false;
        }
        else
        {
            if (_A)
            {
                _Selected_PointA = true;
                _Object_PointButton.transform.Find("Button_PointA").GetComponent<Button>().interactable = false;
                Managers.UI.ShowUI<Image>("Image_Select_PointA");
                Managers.UI.GetUI<Image>("Image_Select_PointA").sprite = _Sprite_X;
                Managers.UI.SetImageColor("Image_Select_PointA", Color.red);
                _Selected_PointA = true;
            }
            else
            {
                _Selected_PointB = true;
                _Object_PointButton.transform.Find("Button_PointB").GetComponent<Button>().interactable = false;
                Managers.UI.ShowUI<Image>("Image_Select_PointB");
                Managers.UI.GetUI<Image>("Image_Select_PointB").sprite = _Sprite_X;
                Managers.UI.SetImageColor("Image_Select_PointB", Color.red);
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


    #region Button
    /// <summary>
    /// Ÿ��Ʋ ȭ�鿡�� ���� ȭ������ �̵� �� ������ ���� ����
    /// 'Ÿ��Ʋ ȭ���� START��ư�� ����Ǿ� ����'
    /// </summary>
    public void Button_Start()
    {
        Managers.Network.Connect();
        //ShowPannel(Define.UI.lobby);
    }

    /// <summary>
    /// �� �̸��� �г����� NetworkManager�� �����ϰ� ���� �����ϰų� ����
    /// '���� ȭ���� JOIN��ư�� ����Ǿ� ����
    /// </summary>
    public void Button_CreateOrJoin()
    {
        if (Managers.UI.GetUI<TMP_InputField>("InputField_RoomCode").text.Length <= 0 ||
            Managers.UI.GetUI<TMP_InputField>("InputField_NickName").text.Length <= 0)
        {
            //SetPopup(Define.PopupState.Warning);
            return;
        }

        Managers.Network.SetRoomCode(Managers.UI.GetUI<TMP_InputField>("InputField_RoomCode").text);
        Managers.Network.SetNickName(Managers.UI.GetUI<TMP_InputField>("InputField_NickName").text);

        Managers.Network.JoinLobby();
    }

    // �� ����� ����
    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� ����!");
        //SetPopup(Define.PopupState.Wait);
        _pv.RPC("JoinPlayer", RpcTarget.All);
    }

    Define.PopupState _PopupState;
    /// <summary>
    /// �˾�â ���� �ٸ� �÷��̾ ��ٸ��� ���̶�� Discnnect �� ����
    /// '�˾�â�� Cancel��ư�� ����Ǿ� ����
    /// </summary>
    public void Button_Cancel()
    {
        if (_PopupState == Define.PopupState.Wait) { Managers.Network.LeaveRoom(); }
        Managers.UI.CloseUI<Pannel>("Match");
    }

    /// <summary>
    /// ������ ��ġ�� ��Ʈ��ũ �Ŵ����� ����
    /// ���� ȭ���� A, B ��ư�� ����Ǿ� ����
    /// </summary>
    /// <param name="_A">true : ����Ʈ A ����, false : ����Ʈ B ����</param>
    public void Button_Point(bool _A)
    {
        //SetPopupInfo(Define.PopupState.MaxPlayer, "Choose your tools", "You choice : " + (_A ? "Wood" : "Stone"));
        //SetPopup(Define.PopupState.MaxPlayer);

        Managers.Network.SetPlayerSpawnPoint(_A);

        _Selected = true;

        _pv.RPC("SelectPoint", RpcTarget.All, _A);
    }
    #endregion
}
