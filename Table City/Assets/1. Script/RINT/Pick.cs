using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    PrefabManager instantiate;
    private void Start()
    {
        instantiate = Managers.instantiate;
    }

    private void OnCollisionEnter(Collision collision)
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
    }


}