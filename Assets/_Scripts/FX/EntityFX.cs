using System.Collections;
// using System.Numerics;



using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Audio;

using Random = UnityEngine.Random;

public enum DustParticleType
{
    Running,
    Landing
}
public class EntityFX : MonoBehaviour
{ 
    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpTextPrefab;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration;
    [SerializeField] private int flashCount;
    public Material originalMat;

    [Header("Ailment Colors")]
    [SerializeField] private Color chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment Particles")]
    [SerializeField] private ParticleSystem igniteFx;
    [SerializeField] private ParticleSystem chillFx;
    [SerializeField] private ParticleSystem shockFx;

    [Header("Ailment Audio")]
    [SerializeField] private AudioMixerGroup soundEffectsGroup;
    private AudioSource burningAudio;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFxPrefab;
    [SerializeField] private GameObject critHitFxPrefab;
    
    [Space]
    [SerializeField] private ParticleSystem runningdDustFx;
    [SerializeField] private ParticleSystem landingDustFx;

    SpriteRenderer[] srs;

    protected virtual void Start()
    {
        srs = GetComponentsInChildren<SpriteRenderer>();

        originalMat = srs[0].material;

        burningAudio = gameObject.AddComponent<AudioSource>();
        burningAudio.clip = AudioManager.Instance.getSFXAudioSource("burning").clip;
        burningAudio.outputAudioMixerGroup = soundEffectsGroup;
        burningAudio.volume = .5f;
    }


    
    public void CreatePopUpText(string _text)
    {
        if (UI.Instance.IsPopupTextEnabled() == false)
        {
            return;
        }
        
        float randomX = Random.Range(-.5f,.5f);
        float randomY = Random.Range(1, 3);

        Vector3 positionOffset = new Vector3(randomX, randomY);
        GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity, transform);
        
        newText.GetComponent<TextMeshPro>().text = _text;
    }

    public void MakeTransparent(bool _transparent)
    {
        CanvasGroup slider = GetComponentInChildren<CanvasGroup>();

        if (_transparent)
        {
            foreach (SpriteRenderer sr in srs)
            {
                sr.color = Color.clear;
            }
            slider.alpha = 0;
        }
        else
        {
            foreach (SpriteRenderer sr in srs)
            {
                sr.color = Color.white;
            }
            slider.alpha = 1;
        }
    }

    public void FlashHitFX()
    {
        StartCoroutine(FlashFX());
    }

    private IEnumerator FlashFX()
    {
        for (int i = 1; i < flashCount; i++)
        {
            foreach (SpriteRenderer sr in srs)
            {
                sr.material = hitMat;
            }

            yield return new WaitForSeconds(flashDuration);

            foreach (SpriteRenderer sr in srs)
            {
                sr.material = originalMat;
            }

            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void RedColorBlink()
    {
        if (srs[0].color != Color.white)
        {
            foreach (SpriteRenderer sr in srs)
            {
                sr.color = Color.white;
            }
        }
        else
        {
            foreach (SpriteRenderer sr in srs)
            {
                sr.color = Color.red;
            }
        }
    }

    public void IgniteFxFor(float _seconds)
    {

        Debug.Log("Ignite FX");
        igniteFx.Play();
        burningAudio.Play();
        InvokeRepeating("IgniteColorFX", 0, .15f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFxFor(float _seconds)
    {
        
        //TODO Add ChillFX
        // chillFx.Play();

        foreach (SpriteRenderer sr in srs)
        {

            // TODO FIX CHILL COLOR CHANGE IN SPRITE RENDERER
            sr.color = chillColor;
        }
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFxFor(float _seconds)
    {
        //TODO Shock ChillFX
        // shockFx.Play();
        InvokeRepeating("ShockColorFX", 0, .15f);
        Invoke("CancelColorChange", _seconds);

    }
    
    private void IgniteColorFX()
    {
        if (srs[0].color != igniteColor[0])
        {
            foreach (SpriteRenderer sr in srs)
            {
                sr.color = igniteColor[0];
            }
        }
        else
        {
            foreach (SpriteRenderer sr in srs)
            {
                sr.color = igniteColor[1];
            }
        }
    }

    private void ShockColorFX()
    {
        if (srs[0].color != shockColor[0])
        {
            foreach (SpriteRenderer sr in srs)
            {
                sr.color = shockColor[0];
            }
        }
        else
        {
            foreach (SpriteRenderer sr in srs)
            {
                sr.color = shockColor[1];
            }
        }
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        foreach (SpriteRenderer sr in srs)
        {
            sr.color = Color.white;
        }
        
        StopAllParticleFx();
        StopAllFxAudio();
    }

    public void StopAllFxAudio()
    {
        burningAudio.Stop();
    }

    private void StopAllParticleFx()
    {
        igniteFx.Stop();
        // TODO Enable after adding chill and shockFX
        // chillFx.Stop();
        // shockFx.Stop();
    }

    public void CreateHitFx(Transform _target)
    {
        float zRotation = Random.Range(-30, 30);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        GameObject newHitFx = Instantiate(hitFxPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);

        newHitFx.transform.Rotate(new Vector3(0, 0, zRotation));

        Destroy(newHitFx, .5f);
    }

    public void CreateCritHitFx(Transform _target, int _facingDir)
    {
        float xRotation = Random.Range(0, 60);
        float zRotation = Random.Range(-30, 30);
        float yRotation;

        if (_facingDir == -1)
            yRotation = 180;
        else
            yRotation = 0;

        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        GameObject newCritHitFx = Instantiate(critHitFxPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);

        newCritHitFx.transform.Rotate(new Vector3(xRotation, yRotation, zRotation));

        Destroy(newCritHitFx, .5f);
    }

    public void CreateDustParticles(DustParticleType _particleType)
    {
        switch (_particleType)
        {
            case DustParticleType.Running:
                if (!runningdDustFx.isPlaying)
                    runningdDustFx.Play();
                return;

            case DustParticleType.Landing:
                if (!landingDustFx.isPlaying)
                    landingDustFx.Play();
                return;
            
        }
    }
}
