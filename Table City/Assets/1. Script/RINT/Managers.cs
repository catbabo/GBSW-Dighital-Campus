using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static GameManager system { get; set; }
    public static UIManager ui { get; private set; }
    public static SoundManager sound { get; private set; }
    public static PrefabManager instantiate { get; private set; }

    private void Awake()
    {
        if(system == null)
        {
            system = gameObject.GetComponent<GameManager>();

            GameObject soundObject = EmptyInstantiate("SoundManager");
            sound = soundObject.AddComponent<SoundManager>();

            GameObject uiObject = EmptyInstantiate("UI_Manager");
            ui = uiObject.AddComponent<UIManager>();

            GameObject instantiateObject = EmptyInstantiate("PrefabMangaer");
            instantiate = instantiateObject.AddComponent<PrefabManager>();

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if(system.gameObject != this.gameObject)
                Destroy(this.gameObject);
        }

    }
    
    private GameObject EmptyInstantiate(string name)
    {
        GameObject empty = new GameObject(name);
        empty.transform.parent = transform;

        return empty;
    }
}
