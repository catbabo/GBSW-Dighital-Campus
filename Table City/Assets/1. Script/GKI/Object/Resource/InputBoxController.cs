using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class InputBoxController : MonoBehaviour
{
    [field:SerializeField]
    public PhotonView _pv_workBench { get; set; }
    [field:SerializeField]
    public int[] asset { get; set; } = new int[12];
    [field: SerializeField]
    public bool speedUpSkill { get; set; } = false;
    public float coolTime { get; set; } = 0;

    [SerializeField] private GameObject sendUI;

    public Define.AssetData sendFactory { get; set; }

	private void Update()
    {
        if(coolTime > 20)
        {
            speedUpSkill = true;
        }

        if((OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Four)) && _pv_workBench.IsMine)
        {
            sendUI.SetActive(!sendUI.activeSelf);
        }
    }

    public void OnDropResource(Define.AssetData type, int count) => asset[(int)type] += count;

    public void SendItem(Define.AssetData factoryType)
    {
        foreach (Define.AssetData _assetData in Enum.GetValues(typeof(Define.AssetData)))
        {

            AssetManager._asset.SetAssetData(_assetData, asset[(int)_assetData]);
            asset[(int)_assetData] = 0;
        }
        
        AssetManager._asset.SyncFactroyData(factoryType);

        RoomManager.room.SyncSpawnObejct(Define.prefabType.effect, "truck", transform.position, Quaternion.identity, factoryType);
        
    }
    public void SpeedUp(Define.AssetData factoryType) => Managers.system.factoryScript[factoryType].speedUpState = true;
}
