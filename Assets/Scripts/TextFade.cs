using TMPro;
using UnityEngine;
using System.Collections;

public class FadeText : MonoBehaviour
{
    public TextMeshProUGUI text; // Reference to the TextMeshPro component
    public float fadeDuration = 1f; // Duration for each fade in/out
    public float delayBetweenFades = 0.5f; // Delay between fades

    private void Start()
    {
        if (!text) text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeInAndOut());
    }

    private IEnumerator FadeInAndOut()
    {
        while (true)
        {
            // Fade In
            yield return StartCoroutine(Fade(0.4f, 1f));

            // Optional delay
            yield return new WaitForSeconds(delayBetweenFades);

            // Fade Out
            yield return StartCoroutine(Fade(1f, 0.4f));

            // Optional delay
            yield return new WaitForSeconds(delayBetweenFades);
        }
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;
        Color originalColor = text.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);
    }
}
