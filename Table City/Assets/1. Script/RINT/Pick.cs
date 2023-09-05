using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    InstantiateManager instantiate;
    float time = 0;
    private void Start()
    {
        instantiate = Managers._inst;
    }
    private void Update()
    {
        time += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(time > 0.1f)
        {
            Managers._sound.SfxPlay(Define.SoundClipName.pick);
            if (collision.transform.CompareTag("Stone"))
            {
                Managers._room.SyncSpawnObejct(Define.prefabType.effect, "ExplosionStone", collision.contacts[0].point, Quaternion.identity, Define.AssetData.stone);
                Managers._asset.SyncFactroyCreateAsset(Define.AssetData.stone, 1);

                Managers._inst.UsePoolingObject(Define.prefabType.effect + Define.AssetData.stone.ToString(), transform.position, Quaternion.identity);
            }
            if (collision.transform.CompareTag("Wood"))
            {
                Managers._room.SyncSpawnObejct(Define.prefabType.effect, "ExplosionWood", collision.contacts[0].point, Quaternion.identity, Define.AssetData.wood);
                Managers._asset.SyncFactroyCreateAsset(Define.AssetData.wood, 1);
                Managers._inst.UsePoolingObject(Define.prefabType.effect + Define.AssetData.wood.ToString(), transform.position, Quaternion.identity);
            }
            time = 0;
        }
    }


}