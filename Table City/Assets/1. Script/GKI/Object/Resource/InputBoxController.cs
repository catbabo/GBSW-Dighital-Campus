using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputBoxController : MonoBehaviour
{
    public int[] asset { get; set; } = new int[12];
    [SerializeField]
    private GameObject sendUI;

    public Define.AssetData sendFactory { get; set; }
    public void Init()
    {

    }

    private void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Two))
        {
            sendUI.SetActive(!sendUI.activeSelf);
        }
    }

    public void OnDropResource(Define.AssetData type, int count)
    { 
        asset[(int)type] += count;
    }
    public void SendItem(Define.AssetData factoryType)
    {
        /*�����̰� ����������� �κ�*/

        //������ ����
        foreach (Define.AssetData i in Enum.GetValues(typeof(Define.AssetData)))
        {
            Managers.system.InputFactoryItem(factoryType, i, asset[(int)i]);
            asset[(int)i] = 0;
        }
        //����
        //RoomManager.room.SyncSpawnObejct(Define.prefabType.effect, "truck", transform.position, Quaternion.identity);
        //RoomManager.room.GetTruck().GetComponent<Throw>().m_Target = Managers.system.factoryScript[sendFactory].transform;
    }
}
