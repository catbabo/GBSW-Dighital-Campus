using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
	private bool _isMine;
	[SerializeField] private TMP_Text _NickNameTMP;

	public void Init(bool isMine)
	{
		_isMine = isMine;

        SetNick();
	}

	private void SetNick()
	{
		//_NickNameTMP.text = _isMine ? Managers.Network._nickName : Managers.Network._otherNickName;
	}
}