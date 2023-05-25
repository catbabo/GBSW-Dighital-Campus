using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{

	public GameObject TitleWindow;
	public GameObject MainWindow;

	public GameObject PopUp1;

	public TMP_InputField RoomCode;
	public TMP_InputField NickName;

    public void Button_Start()
	{
		NetworkManager.Net.Connect();
		TitleWindow.SetActive(false);
		MainWindow.SetActive(true);
	}

	public void Button_CreateOrJoin()
	{
		if(RoomCode.text.Length <= 0 || NickName.text.Length <= 0)
		{
			PopUp1.SetActive(true);

			return;
		}

		NetworkManager.Net.NickNameSet(NickName.text);
		NetworkManager.Net.JoinOrCreate(RoomCode.text);
	}

	public void Button_OK()
	{
		PopUp1.SetActive(false);
	}
}
