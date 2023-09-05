using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : ManagerBase
{
    private AudioSource bgmSource;
    private List<AudioSource> sfxSource = new List<AudioSource>();

    private string[] paths =
    {
        "bgm1", "bgm6",  "button", "countDown", "countUp",
        "event", "fireTruck", "pick", "sharara"
    };
    private List<AudioClip> clips = new List<AudioClip>();

    private float bgmVolume = 0.5f, sfxVolume =1;
    
    [SerializeField]
    private int maxSfxCount = 10;
    private int sourceIndex = 0;

    public override void Init()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;

        SoundClipLoad();
    }

    private void SoundClipLoad()
    {
        foreach (string name in paths)
        {
            AudioClip clip = Resources.Load<AudioClip>("1.Sound/"+name);
        }
    }

    public void BgmPlay(Define.SoundClipName name)
    {
        bgmSource.clip = clips[(int)name];
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }

    public void SfxPlay(Define.SoundClipName name)
    {
        for(int i = 0; i < sfxSource.Count; i++)
        {
            if (sfxSource[sourceIndex].isPlaying)
            {
                if(i == sfxSource.Count - 1)
                {
                    if (sfxSource.Count > maxSfxCount)
                        break;

                    sfxSource.Add(new AudioSource());
                    sfxSource[sourceIndex].PlayOneShot(clips[(int)name],sfxVolume);
                    break;
                }
            }
            else
            {
                sfxSource[sourceIndex].PlayOneShot(clips[(int)name], sfxVolume);
                break;
            }
            sourceIndex++;
        }
    }

    public void BgmStop() => bgmSource.Stop();

    public void ChangeBgmVolume(float volume) { bgmVolume = volume; bgmSource.volume = volume; }

    public void ChangeSfxVolume(float volume) { sfxVolume = volume; for(int i = 0; i < sfxSource.Count; i++) sfxSource[i].volume = volume; }


}
