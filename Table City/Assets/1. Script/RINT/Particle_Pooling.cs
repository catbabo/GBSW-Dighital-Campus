using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Pooling : MonoBehaviour
{
    [SerializeField]
    float poolingTime =1;

    private void OnEnable()
    {
        Managers.Game.ActionTimer(poolingTime, () => Managers.Instance.AddPooling(gameObject));
    }

}
