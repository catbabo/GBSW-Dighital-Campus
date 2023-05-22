using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource bgmSource;
    [SerializeField]
    private List<AudioSource> sfxSource = new List<AudioSource>();
    private Transform sfxParent;

    private Dictionary<string,AudioClip> bgmClip,sfxClip = new Dictionary<string, AudioClip>();

    private float bgmVolume =1,sfxVolume =1;
    [SerializeField]
    private float maxSfxCount = 10;

    void Awake()
    {
        if (bgmSource != null) return;

        GameObject bgmEmpty = EmptyInstantiate("bgm");
        bgmSource = bgmEmpty.AddComponent<AudioSource>();
        bgmSource.loop = true;

        sfxParent = EmptyInstantiate("sfx").transform;

        ResourceLoad(bgmClip, "1.Sound/1.BGM");
        ResourceLoad(sfxClip, "1.Sound/2.SFX");
    }

    private void Update()
    {
        /*테스트 부분
        if(Input.GetKey(KeyCode.Space))
            SfxPlay("a");
        */
    }


    private void ResourceLoad(Dictionary<string, AudioClip> dictionary, string filePath)
    {

        AudioClip[] gameObject = Resources.LoadAll<AudioClip>(filePath);

        foreach (AudioClip oneGameObject in gameObject)
        {
            dictionary.Add(oneGameObject.name, oneGameObject);
        }
    }

    private GameObject EmptyInstantiate(string name)
    {
        GameObject empty = new GameObject(name);
        empty.transform.parent = transform;

        return empty;
    }

    public void BgmPlay(string name)
    {
        if(bgmClip.ContainsKey(name))
        {
            bgmSource.clip = bgmClip[name];
            bgmSource.volume = bgmVolume;
            bgmSource.Play();
        }
    }
    public void BgmStop() => bgmSource.Stop();
    public void ChangeBgmVolume(float volume) { bgmVolume = volume; bgmSource.volume = volume; }
    public void SfxPlay(string name)
    {
        sfxSource.RemoveAll(list => list == null);

        if (sfxSource.Count > maxSfxCount) return;

        if (sfxClip.ContainsKey(name))
        {
            sfxSource.Add(EmptyInstantiate(name).AddComponent<AudioSource>());
            sfxSource[sfxSource.Count - 1].transform.parent = sfxParent;
            Destroy(sfxSource[sfxSource.Count - 1].gameObject, sfxClip[name].length);
            sfxSource[sfxSource.Count-1].PlayOneShot(sfxClip[name],sfxVolume);
        }
    }
    public void ChangeSfxVolume(float volume) { sfxVolume = volume; for(int i = 0; i < sfxSource.Count; i++) sfxSource[i].volume = volume; }


}
