using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssetCountUI : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _assetCount;
    public bool _PointA;

	private void FixedUpdate()
	{
		if (_PointA)
		{
			for (int i = 0; i < 6; i++) { _assetCount[i].text = Managers.Game.asset[i].ToString(); }
		}
		else
		{
			for (int i = 0; i < 6; i++) { _assetCount[i].text = Managers.Game.asset[i+6].ToString(); }
		}
	}

}
