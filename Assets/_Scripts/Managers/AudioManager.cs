using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private AudioSource[] bossMusic;

    public bool playBGM;
    private int bgmIndex;
    private bool bgmStarted = false;

    private Dictionary<string, int> sfxDictionary = new Dictionary<string, int>();

    protected override void Awake()
    {
        base.Awake();
        PopulateSFXDictionary();
        if (!gameObject.transform.parent)
            DontDestroyOnLoad(this.gameObject);
        // bgmStarted = true;
    }

    private void Start()
    {

        PlayRandomBGM();
    }

    private void PopulateSFXDictionary()
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (sfx[i] != null)
            {
                sfxDictionary[sfx[i].gameObject.name] = i;
            }
        }
    }
    public void PlaySFX(string sfxName, Transform source = null)
    {
        if (sfxDictionary.TryGetValue(sfxName, out int index))
        {
            if (source != null && Vector2.Distance(PlayerManager.Instance.player.transform.position, source.position) > sfxMinimumDistance)
            {
                return;
            }

            // Always randomize pitch before playing
            sfx[index].pitch = Random.Range(.85f, 1.15f);

            // If looping, stop and play again to apply new pitch
            if (sfx[index].loop && sfx[index].isPlaying)
            {
                sfx[index].Stop();
            }
            sfx[index].Play();
        }
        else
        {
            Debug.LogWarning($"SFX '{sfxName}' not found!");
        }
    }

    public void PlaySFX(int _sfxIndex)
    {
        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.15f);
            sfx[_sfxIndex].Play();
        }
        else
        {
            Debug.LogWarning($"SFX Index '{_sfxIndex}' not found!");
        }
    }

    private void Update()
    {
        if (!playBGM)
        {
            StopAllBGM();
            return;
        }

        if (bgmStarted && !bgm[bgmIndex].isPlaying)
        {
            PlayRandomBGM();
        }
    }

    public AudioSource getSFXAudioSource(string sfxName)
    {
        if (sfxDictionary.TryGetValue(sfxName, out int index))
        {
            return sfx[index];
        }
        else return null;
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void StopSFX(string sfxName)
    {
        if (sfxDictionary.TryGetValue(sfxName, out int index))
        {
            sfx[index].Stop();
        }
        else
        {
            Debug.LogWarning($"SFX '{sfxName}' not found!");
        }
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;
        StopAllBGM();
        bgm[bgmIndex].Play();
    }

    public void PlayBossMusic()
    {
        StopAllBGM();
        bossMusic[0].Play();
    }

    public void StopBossMusic()
    {
        bossMusic[0].Stop();
        playBGM = true;
    }

    private void StopAllBGM()
    {
        Debug.Log("Stopping all BGM");
        for (int i = 0; i < bgm.Length; i++)
        {
            Debug.Log($"Stopping BGM {i}");
            bgm[i].Stop();
        }
    }

    public void PauseAllSFX()
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (sfx[i].isPlaying)
            {
                sfx[i].Pause();
            }
        }
    }

    public void PlayNextBGM()
    {
        bgmIndex += 1;
        if (bgmIndex > bgm.Length - 1)
            bgmIndex = 0;
        PlayBGM(bgmIndex);
    }

    public void ResumeAllSFX()
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (sfx[i].isPlaying)
            {
                sfx[i].UnPause();
            }
        }
    }

}
