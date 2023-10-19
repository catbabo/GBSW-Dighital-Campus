using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class ButtonController : MonoBehaviourPunCallbacks
{

    #region Object_UI
    /// <summary> 취소 버튼 오브젝트 </summary>
    [SerializeField] private GameObject _Object_CancelButton;

    /// <summary> 포인트 선택 버튼 오브젝트 </summary>
    [SerializeField] private GameObject _Object_PointButton;


    #endregion

    #region Sprite
    /// <summary> 포인트 선택 스프라이트 </summary>
    [SerializeField] private Sprite _Sprite_Check;

    /// <summary> 포인트 선택 불가 스프라이트 </summary>
    [SerializeField] private Sprite _Sprite_X;
    #endregion

    /// <summary> 플레이어가 선택을 했는지의 여부 </summary>
    private bool _Selected = false;
    /// <summary> 플레이어가 포인트 선택을 했는지의 여부 </summary>
    private bool _Selected_PointA = false, _Selected_PointB = false;


    /// <summary> 포톤 뷰 </summary>
    private PhotonView _pv;


    /// <summary> 선택한 시작지점 초기화 </summary>
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

    /// <summary> 방에 플레이어가 최대로 들어왔다면 캔슬 버튼을 포인트 선택 버튼으로 교체 </summary>
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
    /// 타이틀 화면에서 메인 화면으로 이동 및 마스터 서버 연결
    /// '타이틀 화면의 START버튼과 연결되어 있음'
    /// </summary>
    public void Button_Start()
    {
        Managers.Network.Connect();
        //ShowPannel(Define.UI.lobby);
    }

    /// <summary>
    /// 방 이름과 닉네임을 NetworkManager에 전달하고 방을 입장하거나 생성
    /// '메인 화면의 JOIN버튼과 연결되어 있음
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

    // 방 입장시 실행
    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 성공!");
        //SetPopup(Define.PopupState.Wait);
        _pv.RPC("JoinPlayer", RpcTarget.All);
    }

    Define.PopupState _PopupState;
    /// <summary>
    /// 팝업창 종료 다른 플레이어를 기다리는 중이라면 Discnnect 후 종료
    /// '팝업창의 Cancel버튼과 연결되어 있음
    /// </summary>
    public void Button_Cancel()
    {
        if (_PopupState == Define.PopupState.Wait) { Managers.Network.LeaveRoom(); }
        Managers.UI.CloseUI<Pannel>("Match");
    }

    /// <summary>
    /// 선택한 위치를 네트워크 매니저에 저장
    /// 메인 화면의 A, B 버튼에 연결되어 있음
    /// </summary>
    /// <param name="_A">true : 포인트 A 선택, false : 포인트 B 선택</param>
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
