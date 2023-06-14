using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
	// Ÿ��Ʋ ȭ�� ( ���� ���� �� ���� ���� ��ư )
	public GameObject _TitleWindow;
	// ���� ȭ�� ( �� �̸��� �г��� ���� �� ���� ���� )
	public GameObject _MainWindow;

	// ���̳� �г����� ���� ���� ������ ������ �˾�
	public GameObject _PopUp;

	// �� �̸� (code) �� �г���
	public TMP_InputField RoomCode;
	public TMP_InputField NickName;

	// Ÿ��Ʋ ȭ�鿡�� ���� ȭ������ �̵�
	// Ÿ��Ʋ ȭ���� START��ư�� ����Ǿ� ����
    public void Button_Start()
	{
		_TitleWindow.SetActive(false);
		_MainWindow.SetActive(true);
	}

	// �� �̸��� �г����� NetworkManager�� �����ϰ� ���� �����ϰų� ����
	// ���� ȭ���� JOIN��ư�� ����Ǿ� ����
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

	// �˾�â�� ����
	// �˾�â�� OK��ư�� ����Ǿ� ����
	public void Button_OK()
	{
		_PopUp.SetActive(false);
	}
}
