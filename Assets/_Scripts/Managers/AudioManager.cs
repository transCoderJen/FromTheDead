using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

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
    }

    private void Start()
    {
        bgmStarted = true;
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
            if (source != null && Vector2.Distance(PlayerManager.Instance.transform.position, source.position) > sfxMinimumDistance)
                return;

            sfx[index].pitch = Random.Range(.85f, 1.15f);
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

    private void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
