using Oculus.Interaction.Input.Visuals;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PN = Photon.Pun.PhotonNetwork;

public class PlayerController : MonoBehaviour
{
    private PhotonView _pv;
    private PlayerInitioner initoner;
    private PlayerUI ui;
    private string nickName;
    [SerializeField]
    private bool isLocal;

    private void Awake()
    {
        Managers.player = this;
        _pv = GetComponent<PhotonView>();
        ui = GetComponent<PlayerUI>();

        isLocal = !Managers.Network.IsInRoom();
        if (_pv.IsMine)
        {
            if(!isLocal)
            {
                Debug.Log("1");
                initoner = Util.AddOrGetComponent<PlayerInitioner>(gameObject);
                initoner.Init(false);
            
                //ui.Init(true);
            }
            else
            {
                Debug.Log("2");
                //initoner = Util.AddOrGetComponent<PlayerInitioner>(gameObject);
                //initoner.Init(true);

                //ui.Init(true);
            }
        }
        else
        {
            if (isLocal)
            {
                isLocal = false;
                Debug.Log("3");
                initoner = Util.AddOrGetComponent<PlayerInitioner>(gameObject);
                initoner.Init(true);

                //ui.Init(true);
            }
        }
    }

    public string GetNickName() { return nickName; }

    public void SetNickName(string name) { nickName = name; }

    public void Destroy()
    {
        Debug.Log("Destroy");
        Managers.player = null;
        if(isLocal)
        {
            Destroy(gameObject);
        }
        else
        {
            PN.Destroy(_pv);
        }
    }
}
