using System.Collections;
using UnityEngine;

public class PulseIntensity : MonoBehaviour
{
    
    private SpriteRenderer spriteRenderer;

    [SerializeField] private bool isTrigger;

    public float minBloomIntensity = 0.5f;
    public float maxBloomIntensity = 3f;
   
    public float speed = 2f;

    public float pulseDuration = 1f;

    private Material material;
    private Color baseColor;
    private bool isPulsing = false;

    private void Start()
    {
        ResetMaterial();
    }

    public void ResetMaterial()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            // Clone material to avoid modifying the original asset
            material = Instantiate(spriteRenderer.material);
            spriteRenderer.material = material;

            // Get the original color from _MainTex
            baseColor = material.GetColor("_Color");

            // Debug.Log("[PulseIntensity] Material instantiated. Base Color: " + baseColor);
        }
        else
        {
            Debug.LogError("[PulseIntensity] SpriteRenderer not found!");
        }
    }

    private void Update()
    {
        if(isTrigger)
            return;

        if (material == null) 
        {
            // Debug.Log("MATERIAL IS NULL");
            return;
        }

        // Lerp bloom intensity using PingPong
        float intensity = Mathf.Lerp(minBloomIntensity, maxBloomIntensity, Mathf.PingPong(Time.time * speed, 1));


        // Cycle color: yellow -> pink -> blue -> yellow
        float t = Mathf.PingPong(Time.time * speed, 1f);

        // Use a triangle wave to cycle through three colors
        Color colorA = Color.yellow;
        Color colorB = new Color(1f, 0.2f, 0.8f); // pink
        Color colorC = Color.cyan; // blue

        Color lerpedColor;
        if (t < 0.5f)
        {
            lerpedColor = Color.Lerp(colorA, colorB, t * 2f);
        }
        else
        {
            lerpedColor = Color.Lerp(colorB, colorC, (t - 0.5f) * 2f);
        }

        // Optionally, cycle back to yellow after blue
        float t2 = Mathf.PingPong(Time.time * speed / 2f, 1f);
        if (t2 > 0.66f)
        {
            lerpedColor = Color.Lerp(colorC, colorA, (t2 - 0.66f) * 3f);
        }

        // Apply intensity boost to the sprite color
        Color newColor = lerpedColor * intensity;
        material.SetColor("_Color", newColor);

        // Debug.Log($"[PulseIntensity] Bloom Intensity: {intensity}, New Color: {newColor}");
    }

    public void TriggerPulse()
    {
        ResetMaterial();
        // Debug.Log("[PulseIntensity] TriggerPulse called");
        if (!isPulsing)
        {
            PulseEffect();
        }
    }



    private void PulseEffect()
    {
        isPulsing = true;
        
        LeanTween.value(gameObject, minBloomIntensity, maxBloomIntensity, pulseDuration / 2)
            .setOnUpdate((float intensity) =>
            {
                material.SetColor("_Color", baseColor * intensity);
            })
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                LeanTween.value(gameObject, maxBloomIntensity, minBloomIntensity, pulseDuration / 2)
                    .setOnUpdate((float intensity) =>
                    {
                        material.SetColor("_Color", baseColor * intensity);
                    })
                    .setEase(LeanTweenType.easeInOutSine)
                    .setOnComplete(() => isPulsing = false);
            });
    }

    public void LightOn()
    {
        LeanTween.value(gameObject, minBloomIntensity, maxBloomIntensity, pulseDuration / 2)
            .setOnUpdate((float intensity) =>
            {
                material.SetColor("_Color", baseColor * intensity);
            })
            .setEase(LeanTweenType.easeInOutSine);
    }

    public void LightOff()
    {
        LeanTween.value(gameObject, maxBloomIntensity, minBloomIntensity, pulseDuration / 2)
                    .setOnUpdate((float intensity) =>
                    {
                        material.SetColor("_Color", baseColor * intensity);
                    })
                    .setEase(LeanTweenType.easeInOutSine);
    }

}
