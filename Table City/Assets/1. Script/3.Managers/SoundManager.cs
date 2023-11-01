using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : ManagerBase
{
    private AudioSource bgmSource;

    private List<AudioSource> sfxSource = new List<AudioSource>();

    private Dictionary<string, AudioClip> bgmClip = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxClip = new Dictionary<string, AudioClip>();

    private float bgmVolume = 0.5f, sfxVolume = 1;
    private float maxSfxCount = 10;

    public override void Init()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;

        Util.ResourceLoad(bgmClip, "1.Sound/1.BGM");
        Util.ResourceLoad(sfxClip, "1.Sound/2.SFX");
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

    public void SfxPlay(string name)
    {
        sfxSource.RemoveAll(list => list == null);

        if (sfxSource.Count > maxSfxCount) return;

        if (sfxClip.ContainsKey(name)) return;

        int i;
        for(i = 0; i < sfxSource.Count; i++)
        {
            if(!sfxSource[i].isPlaying)
            {
                sfxSource[i].PlayOneShot(sfxClip[name], sfxVolume);
            }
        }

        if(i == sfxSource.Count)
        {
            sfxSource.Add(gameObject.AddComponent<AudioSource>());
            sfxSource[i].PlayOneShot(sfxClip[name], sfxVolume);
        }
    }
}
