using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Pooling : MonoBehaviour
{
    private void OnEnable()
    {
        Managers.Game.ActionTimer(1, () => Managers.Instance.AddPooling(gameObject));
    }
}
