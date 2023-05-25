using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Pooling : MonoBehaviour
{
    PrefabManager instantiate;
    GameManager system;
    ParticleSystem particle;
    [SerializeField]
    float poolingTime =1;
    private void Awake()
    {
        instantiate = Managers.instantiate;
        system = Managers.system;
        particle = GetComponent<ParticleSystem>();
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        system.ActionTimer(poolingTime, () => instantiate.AddPooling(gameObject));
    }

}
