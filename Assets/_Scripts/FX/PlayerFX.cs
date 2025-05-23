using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerFX : EntityFX
{
    [Header("Vignette FX")]
    [SerializeField] private Volume volume;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float flashSpeed;
    [SerializeField] private Vignette vignette;

    [SerializeField] private float currentVignetteIntensity;
    
    [Header("Screen Shake FX")]
    [SerializeField] private float shakeMultiplier;
    public Vector3 lightShakePower;
    public Vector3 heavyShakePower;
    private CinemachineImpulseSource screenShake;

    [Header("After Image FX")]
    // [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float fadeDuration;
    public float afterImageRate;

    Player player;

    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();
        player = PlayerManager.Instance.player;
        if (volume.profile.TryGet(out vignette))
        {
            Debug.Log("Vignette found");
            vignette.intensity.Override(0f);
        }
    }

    public void FlashVignette()
    {
        if (vignette != null)
        {
            LeanTween.value(gameObject, 0f, maxIntensity, flashSpeed)
                .setOnUpdate((float value) =>
                {
                    vignette.intensity.Override(value);
                    currentVignetteIntensity = value;
                })
                .setOnComplete(() =>
                {
                    LeanTween.value(gameObject, currentVignetteIntensity, 0f, flashSpeed)
                        .setOnUpdate((float value) =>
                        {
                            vignette.intensity.Override(value);
                            currentVignetteIntensity = value;
                        });
                });
        }
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void CreateAfterImageFX(Transform playerTransform)
    {
        // Create a new GameObject to hold the afterimage
        GameObject newAfterImage = new GameObject("AfterImage");

        // Set the new GameObject's position and rotation to match the player
        newAfterImage.transform.position = playerTransform.position;
        newAfterImage.transform.rotation = playerTransform.rotation;

        // Add a SpriteRenderer component to the new GameObject
        SpriteRenderer afterImageRenderer = newAfterImage.AddComponent<SpriteRenderer>();

        // Get the SpriteRenderer from the player
        SpriteRenderer playerSpriteRenderer = playerTransform.GetComponentInChildren<SpriteRenderer>();

        // Copy the sprite and other properties from the player's SpriteRenderer
        afterImageRenderer.sprite = playerSpriteRenderer.sprite;
        afterImageRenderer.color = playerSpriteRenderer.color;
        afterImageRenderer.flipX = playerSpriteRenderer.flipX;
        afterImageRenderer.flipY = playerSpriteRenderer.flipY;
        afterImageRenderer.sortingLayerID = playerSpriteRenderer.sortingLayerID;
        afterImageRenderer.sortingOrder = playerSpriteRenderer.sortingOrder;

        // Start the fade coroutine
        StartCoroutine(FadeAfterImageFX(newAfterImage));
    }

    IEnumerator FadeAfterImageFX(GameObject _afterImage)
    {
        SpriteRenderer spriteRenderer = _afterImage.GetComponent<SpriteRenderer>();
        Color startColor = spriteRenderer.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // Ensure the alpha is set to 0 after the loop
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        Destroy(_afterImage);
    }
}
