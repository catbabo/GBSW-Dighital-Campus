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
            RoomManager.room.SyncSpawnObejct(Define.prefabType.effect, "Explosion", collision.contacts[0].point, Quaternion.identity);
        }
        else
        {

        }
    }


}
