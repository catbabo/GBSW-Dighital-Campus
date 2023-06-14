using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
	// 타이틀 화면 ( 게임 제목 및 게임 시작 버튼 )
	public GameObject _TitleWindow;
	// 메인 화면 ( 방 이름과 닉네임 설정 및 서버 입장 )
	public GameObject _MainWindow;

	// 방이나 닉네임이 적혀 있지 않으면 나오는 팝업
	public GameObject _PopUp;

	// 방 이름 (code) 과 닉네임
	public TMP_InputField RoomCode;
	public TMP_InputField NickName;

	// 타이틀 화면에서 메인 화면으로 이동
	// 타이틀 화면의 START버튼과 연결되어 있음
    public void Button_Start()
	{
		_TitleWindow.SetActive(false);
		_MainWindow.SetActive(true);
	}

	// 방 이름과 닉네임을 NetworkManager에 전달하고 방을 입장하거나 생성
	// 메인 화면의 JOIN버튼과 연결되어 있음
	public void Button_CreateOrJoin()
	{
		if(RoomCode.text.Length <= 0 || NickName.text.Length <= 0)
		{
			_PopUp.SetActive(true);

			return;
		}

		NetworkManager.Net._SetRoomCode(RoomCode.text);
		NetworkManager.Net._SetNickName(NickName.text);
		NetworkManager.Net.Connect();
	}

	// 팝업창을 내림
	// 팝업창의 OK버튼과 연결되어 있음
	public void Button_OK()
	{
		_PopUp.SetActive(false);
	}
}
