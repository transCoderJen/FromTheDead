using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField] private Image doomAwaitsAllImage;
    [SerializeField] private Button restartButton;
    [SerializeField] private float fadeDuration;

    void OnEnable()
    {
        StartCoroutine(WaitAndFadeIn());
        AudioManager.Instance.playBGM = false;
        AudioManager.Instance.PlaySFX("deathScreen");
    }

    private IEnumerator WaitAndFadeIn()
    {
        // Wait for 2 seconds before starting the fade-in
        yield return new WaitForSeconds(2f);
        FadeIn();
    }

    void FadeIn()
    {
        if (doomAwaitsAllImage != null && doomAwaitsAllImage.color.a < 1f)
        {
            LeanTween.value(gameObject, 0f, 1f, fadeDuration)
                .setEase(LeanTweenType.linear)
                .setOnUpdate((float val) =>
                {
                    var color = doomAwaitsAllImage.color;
                    color.a = val;
                    doomAwaitsAllImage.color = color;
                })
                .setOnComplete(() =>
                {
                    if (restartButton != null)
                        restartButton.gameObject.SetActive(true);
                });
            // Prevent multiple tweens
            enabled = false;
        }
    }
}