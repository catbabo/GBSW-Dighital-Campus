using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FactoyData : MonoBehaviour
{
    public Factory data;
    public bool speedUpState { get; set; } = false;

    [field:Header("저장된 자원"),SerializeField]
    public int[] asset { get; set; } = new int[12];
    private GameObject child;
    private GameObject[] fx = new GameObject[3];

    private int lvSection;
    private Animator animator;

    private void Start()
    {
        child = transform.GetChild(0).gameObject;
        fx[0] = transform.GetChild(1).gameObject;
        fx[1] = transform.GetChild(2).gameObject;
        fx[2] = transform.GetChild(3).gameObject;
        lvSection = (int)((float)data.maxLv / 3);
        animator = child.GetComponent<Animator>();
    }

    private void Update()
    {
        LvUpParticle();
        ItemGet();
    }

    float timer =0,speedUpTimer;
    private void ItemGet()
    {
        if (data.lv <= 0)
        {
            if (speedUpState == true) 
                speedUpState = false;
            return;
        }

        if(speedUpState == true)
        {
            timer += Time.deltaTime*2;
            speedUpTimer += Time.deltaTime;
            if(speedUpTimer > 10)
            {
                speedUpState = false;
                speedUpTimer = 0;
            }
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (timer < (float)(data.maxLv + 1 - data.lv )/ 2 )return;

        animator.SetTrigger("GetAsset");

        if (PhotonNetwork.IsMasterClient) AssetManager._asset.SyncFactroyCreateAsset(data.createAsset, 1);

        Managers.instantiate.UsePoolingObject(Define.prefabType.effect+data.createAsset.ToString(),transform.position,Quaternion.identity);
        timer = 0;
    }
    private void LvUpParticle()
    {
        if (data.lv == 0 && child.activeSelf == true)
            child.SetActive(false);
        else if (data.lv != 0 && child.activeSelf == false)
            child.SetActive(true);


        if (data.lv == lvSection)
        {
            fx[0].SetActive(true);
            fx[1].SetActive(false);
            fx[2].SetActive(false);
        }
        else if (data.lv == lvSection * 2)
        {
            fx[1].SetActive(true);
            fx[0].SetActive(false);
            fx[2].SetActive(false);
        }
        else if (data.lv == lvSection * 3)
        {
            fx[2].SetActive(true);
            fx[1].SetActive(false);
            fx[0].SetActive(false);
        }
    }
}
