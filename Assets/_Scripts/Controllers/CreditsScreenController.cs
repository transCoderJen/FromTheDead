using System.Collections;
using UnityEngine;
using TMPro;

public class CreditScreenController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI creditText;
    public CanvasGroup canvasGroup;

    [Header("Timing Settings")]
    public float fadeDuration = 1.5f;
    public float displayDuration = 3f;
    public float delayBetween = 0.5f;

    [Header("Font Settings")]
    [Range(20, 100)]
    public int italicFontSizePercent = 75;

    [TextArea(3, 10)]
    public string[] rawCredits = new string[]
    {
        "DIMMING\n<i>A descent into the dying light.</i>",
        "PROGRAMMING\n<b>TransCoder</b>\n<i>Architect of the falling world.</i>",
        "MUSIC\n<b>Benelios</b>\n<i>Echoes from a broken soul.</i>",
        "ART\n<b>Magma</b>\n<i>Visions from beneath the surface.</i>",
        "Originally created for\n<b>Pixel Game Jam</b>\n<i>A flicker forged in pixels.</i>",
        "SPECIAL THANKS\nTo the jam organizers.\nTo the players who walk into the dark.</i>",
        "THANK YOU FOR PLAYING\n<i>The light was never going to last.</i>"
    };

    private string[] formattedCredits;

    private void Start()
    {
        canvasGroup.alpha = 0;
        FormatItalics();
        StartCoroutine(PlayCredits());
    }

    private void FormatItalics()
    {
        string sizeTag = $"<size={italicFontSizePercent}%>";
        string sizeClose = "</size>";
        formattedCredits = new string[rawCredits.Length];

        for (int i = 0; i < rawCredits.Length; i++)
        {
            string line = rawCredits[i];
            line = line.Replace("<i>", sizeTag + "<i>");
            line = line.Replace("</i>", "</i>" + sizeClose);
            formattedCredits[i] = line;
        }
    }

    IEnumerator PlayCredits()
    {
        foreach (var line in formattedCredits)
        {
            creditText.text = line;
            yield return StartCoroutine(FadeIn());
            yield return new WaitForSeconds(displayDuration);
            yield return StartCoroutine(FadeOut());
            yield return new WaitForSeconds(delayBetween);
        }

        // Optional: load a scene or stay on last credit
        // SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    IEnumerator FadeOut()
    {
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
