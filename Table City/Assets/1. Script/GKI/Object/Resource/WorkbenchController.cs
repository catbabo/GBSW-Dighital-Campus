using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WorkbenchController : MonoBehaviour
{
    [SerializeField]
    private bool _isWoopdSide;
    private string _firstSourcePath = "ResourceItems/FirstSource/";
    private bool _isMine;

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
            _firstSourcePath = "Wood";
        }
        else
        {
            _firstSourcePath = "Steel";
        }

        Photon.Pun.PhotonNetwork.Instantiate(_firstSourcePath, Vector3.zero, Quaternion.identity);
    }

    private void Init()
    {
        PhotonView pv = null;
        pv = GetComponent<PhotonView>();
        _isMine = (pv.IsMine);

        if (_isMine)
        {
            SetFirstResources();

            _isWoopdSide = NetworkManager.Net.IsPlayerTeamA();
            int resourceIndex = 0;
            if (_isWoopdSide)
            {
                resourceIndex = 1;
            }
            else
            {
                resourceIndex = 6;
            }
            SetResourceInBox(resourceIndex);
            
        }
    }

    private void SetResourceInBox(int startIndex)
    {
        Transform root = transform.Find("Player_Box");
        Transform box;
        GameObject _resourcePrefab;
        for (int resourceIndex = 0; resourceIndex < 6; resourceIndex++)
        {
            box = root.Find($"Crate{resourceIndex + 1}");

            _sourcePath = _sourceInBoxPath + _resourceName[resourceIndex];
            _resourcePrefab = Photon.Pun.PhotonNetwork.Instantiate(_sourcePath, box.position, Quaternion.identity);
            _resourcePrefab.transform.SetParent(box);

            box.GetComponent<PlayerBoxController>().Init(_resourcePrefab, _isMine, (Define.AssetData)resourceIndex);
        }
    }
}
