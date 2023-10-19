using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Pooling : MonoBehaviour
{
    InstantiateManager instantiate;
    GameManager system;
    [SerializeField]
    float poolingTime =1;
    private void Awake()
    {
        instantiate = Managers.Instance;
        system = Managers.Game;
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        system.ActionTimer(poolingTime, () => instantiate.AddPooling(gameObject));
    }

}
