using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class LobbyController : MonoBehaviourPunCallbacks
{
	#region Sprite
	/// <summary> 포인트 선택 스프라이트 </summary>
	[SerializeField] private Sprite _Sprite_Check;

	/// <summary> 포인트 선택 불가 스프라이트 </summary>
	[SerializeField] private Sprite _Sprite_X;
	#endregion

	private struct PopupInfo
	{
		public string _header, _text;

        public void SetInfo(string header, string text)
        {
            _header = header;
			SetText(text);
        }

        public void SetText(string text)
        {
            _text = text;
        }
    }
	private List<PopupInfo> popupInfos = new List<PopupInfo>();

    private void OnEnterTitle()
    {
        InitWindow();
        InitPopupInfo();
        ShowPannel("Title");
		if (Managers.Network._forceOut)
		{
			SetPopup(Define.PopupState.Warning);
			Managers.Network.SetForceOut(false);
		}
    }

    private void OnEnterLobby()
    {

    }

    private void InitWindow()
	{
        GameObject go = null;

		Util.SetRoot("LobbyCanvas");
        Managers.UI.AddUI<Pannel>("Title");
        Managers.UI.AddUI<Pannel>("Lobby");
        Managers.UI.AddUI<Pannel>("Match");

        Util.SetRoot("Lobby", true);
        Managers.UI.AddUI<TMP_InputField>("InputField_RoomCode");
        Managers.UI.AddUI<TMP_InputField>("InputField_NickName");

		go = Managers.UI.GetUIObject<Pannel>("Match");
        Util.SetRoot(go);
        Util.SetRoot("Popup01");
        Managers.UI.AddUI<TMP_Text>("Header");
        Managers.UI.AddUI<TMP_Text>("Subject");

		go = Util.Find("Select Point Button");
        Managers.UI.AddUI<GameObject>("Button_PointA");
        Util.SetRoot("Button_PointA");
        Managers.UI.AddUI<Image>("Image_Select_PointA");

        Util.SetRoot(go);
        Managers.UI.AddUI<GameObject>("Button_PointB");
        Util.SetRoot("Button_PointB");
        Managers.UI.AddUI<Image>("Image_Select_PointB");
    }

    private void InitPopupInfo()
    {
        popupInfos.Add(new PopupInfo { });
        popupInfos[(int)Define.PopupState.Wait].SetInfo("Wait for Player", "Player : 0 / 2");

        popupInfos.Add(new PopupInfo { });
        popupInfos[(int)Define.PopupState.MaxPlayer].SetInfo("Choose your tools", "You choice : ");

        popupInfos.Add(new PopupInfo { });
        popupInfos[(int)Define.PopupState.Warning].SetInfo("Warning", "Nothing Enter");
    }

    public void SetPopupInfo(Define.PopupState popup, string header, string text)
    {
        popupInfos[(int)popup].SetInfo(header, text);
    }

    public void SetPopup(Define.PopupState state)
	{
        ShowPannel("Match");

        // 대기중이라면 플레이어가 현재 몇명 들어와 있는지 출력
        if (state == Define.PopupState.Wait)
        { popupInfos[(int)state].SetText(Managers.Network.GetInRoomPlayerCount()); }

        Managers.UI.SetText("MatchHeader", popupInfos[(int)state]._header);
        Managers.UI.SetText("MatchText", popupInfos[(int)state]._text);

		// 플레이어가 모두 들어오면 시작지점 선택 버튼 등장
		Managers.UI.ShowUIOnPopup(Define.PopupState.MaxPlayer);
    }
    public void ShowPannel(string name)
    {
        Managers.UI.CloseUI<Pannel>();
        Managers.UI.ShowUI<Pannel>(name);
    }
}
