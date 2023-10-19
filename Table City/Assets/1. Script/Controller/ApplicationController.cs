using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Managers))]
public class ApplicationController : MonoBehaviour
{
    private void Awake()
    {
        Managers.Root = Util.GetOrAddComponent<Managers>(gameObject);
        Managers.Root.Init();
        DontDestroyOnLoad(gameObject);
    }
}
