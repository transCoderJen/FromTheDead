using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject introBackground;
    public Image image1;
    public Image image2;

    [Header("Transition Settings")]
    public float fadeDuration = 1.5f;
    public float displayDuration = 1f;
    public string gameSceneName = "GameScene";

    private void Start()
    {
        // Ensure images are hidden at start
        SetImageAlpha(image1, 0f);
        SetImageAlpha(image2, 0f);
    }

    public void OnStartGamePressed()
    {
        introBackground.SetActive(true);
        StartCoroutine(PlayIntroSequence());
    }

    public void OnCreditsPressed()
    {
        SceneManager.LoadScene("Credits");
    }

    private IEnumerator PlayIntroSequence()
    {
        yield return StartCoroutine(FadeImage(image1, true));
        yield return new WaitForSeconds(displayDuration);
        yield return StartCoroutine(FadeImage(image1, false));

        yield return new WaitForSeconds(0.25f); // small buffer

        yield return StartCoroutine(FadeImage(image2, true));
        yield return new WaitForSeconds(displayDuration);
        yield return StartCoroutine(FadeImage(image2, false));

        SceneManager.LoadScene(gameSceneName);
    }

    private IEnumerator FadeImage(Image img, bool fadeIn)
    {
        float elapsed = 0f;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            SetImageAlpha(img, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetImageAlpha(img, endAlpha);
    }

    private void SetImageAlpha(Image img, float alpha)
    {
        if (img != null)
        {
            Color color = img.color;
            color.a = alpha;
            img.color = color;
        }
    }
}
