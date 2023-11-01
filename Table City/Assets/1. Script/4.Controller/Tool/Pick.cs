using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    float time = 0;

    private void Update()
    {
        time += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(time > 0.1f)
        {
            Managers.Sound.SfxPlay("pick");
            if (collision.transform.CompareTag("Stone"))
            {
                Managers.Network.SyncSpawnObejct(Define.prefabType.effect, "ExplosionStone", collision.contacts[0].point, Quaternion.identity, Define.AssetData.stone);
                Managers.Asset.SyncFactroyCreateAsset(Define.AssetData.stone, 1);

                Managers.Instance.UsePoolingObject(Define.prefabType.effect + Define.AssetData.stone.ToString(), transform.position, Quaternion.identity);
            }
            if (collision.transform.CompareTag("Wood"))
            {
                Managers.Network.SyncSpawnObejct(Define.prefabType.effect, "ExplosionWood", collision.contacts[0].point, Quaternion.identity, Define.AssetData.wood);
                Managers.Asset.SyncFactroyCreateAsset(Define.AssetData.wood, 1);
                Managers.Instance.UsePoolingObject(Define.prefabType.effect + Define.AssetData.wood.ToString(), transform.position, Quaternion.identity);
            }
            time = 0;
        }
    }


}