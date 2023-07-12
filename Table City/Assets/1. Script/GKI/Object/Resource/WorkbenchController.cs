using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WorkbenchController : MonoBehaviour
{
    [SerializeField]
    private bool _isWoopdSide;
    private string _firstSourcePath = "ResourceItems/FirstSource/";
    private PhotonView pv;

    private Vector3[] BoxPos = new Vector3[6];

    private string
        _sourceInBoxPath = "ResourceItems/InBox/",
        _sourcePath;

    private string[] _resourceName = { "Woods", "Rubbers", "Coals", "Glasses", "Uraniums", "Mithrils",
        "Stones", "Papers", "Steels", "Electricitys","Semiconductors", "FloatingStones" };

    private void Start()
    {
        Init();
    }

    private void SetFirstResources()
    {
        if(_isWoopdSide)
        {
            _firstSourcePath += "Wood";
        }
        else
        {
            _firstSourcePath += "Stone";
        }

        Transform firstResourceRoot = transform.Find("FirstResource");

        GameObject firstObject = NetworkManager.Net.SpawnObject(_firstSourcePath);
        firstObject.transform.SetParent(firstResourceRoot);
        firstObject.transform.localPosition = Vector3.zero;
        firstObject.transform.localRotation = Quaternion.identity;
        firstObject.transform.localScale = Vector3.one;

    }

    private void Init()
    {
        pv = gameObject.GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            SetFirstResources();

            _isWoopdSide = NetworkManager.Net.IsPlayerTeamA();
            int resourceIndex = 0;
            if (_isWoopdSide)
            {
                resourceIndex = 0;
            }
            else
            {
                resourceIndex = 6;
            }
            SetResourceInBox(resourceIndex);
            
        }

        SetBoxPosition(NetworkManager.Net.IsPlayerTeamA());
    }

    private void SetResourceInBox(int startIndex)
    {
        Transform root = transform.Find("Player_Box");
        Transform box;
        GameObject _resourcePrefab;
        for (int resourceIndex = 0; resourceIndex < 6; resourceIndex++)
        {
            box = root.Find($"Crate{resourceIndex + 1 }");

            _sourcePath = _sourceInBoxPath + _resourceName[resourceIndex + startIndex];
            _resourcePrefab = NetworkManager.Net.SpawnObject(_sourcePath, box);

            _resourcePrefab.transform.SetParent(box);
            _resourcePrefab.transform.localPosition = Vector3.zero;
            _resourcePrefab.transform.localRotation = Quaternion.identity;
            _resourcePrefab.transform.localScale = Vector3.one;

            box.GetComponent<PlayerBoxController>().Init(_resourcePrefab, pv.IsMine, (Define.AssetData)(resourceIndex + startIndex));
        }

    }

    private void SetBoxPosition(bool _pointA)
	{
        Transform root = transform.Find("Player_Box");
        Transform box;

        for (int i = 0; i<6; i++)
		{
            box = root.Find($"Crate{i + 1 }");
            BoxPos[i] = box.position;
		}

        Managers.system.SetWorkbechPoint(BoxPos, pv.IsMine ? _pointA : !_pointA);
    }
}
