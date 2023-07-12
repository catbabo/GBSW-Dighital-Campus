using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    PrefabManager instantiate;
    float time = 0;
    private void Start()
    {
        instantiate = Managers.instantiate;
    }
    private void Update()
    {
        time += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(time > 0.25f)
        {
            if (collision.transform.CompareTag("Stone"))
            {
                RoomManager.room.SyncSpawnObejct(Define.prefabType.effect, "ExplosionStone", collision.contacts[0].point, Quaternion.identity, Define.AssetData.stone);
                AssetManager._asset.SyncFactroyCreateAsset(Define.AssetData.stone, 1);
                Managers.instantiate.UsePoolingObject(Define.prefabType.effect + Define.AssetData.stone.ToString(), transform.position, Quaternion.identity);
            }
            if (collision.transform.CompareTag("Wood"))
            {
                RoomManager.room.SyncSpawnObejct(Define.prefabType.effect, "ExplosionWood", collision.contacts[0].point, Quaternion.identity, Define.AssetData.wood);
                AssetManager._asset.SyncFactroyCreateAsset(Define.AssetData.wood, 1);
                Managers.instantiate.UsePoolingObject(Define.prefabType.effect + Define.AssetData.wood.ToString(), transform.position, Quaternion.identity);
            }
            time = 0;
        }
    }


}