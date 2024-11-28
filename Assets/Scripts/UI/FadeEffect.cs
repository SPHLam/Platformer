using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;

    public void ScreenFadeEffect(float targetAlpha, float duration, System.Action onComplete = null)
    {
        StartCoroutine(FadeCoroutine(targetAlpha, duration, onComplete));
    }
    private IEnumerator FadeCoroutine(float targetAlpha, float duration, System.Action onComplete)
    {
        float time = 0;
        Color currentColor = _fadeImage.color;

        float startAlpha = currentColor.a;

        while (time < duration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);

            _fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

            yield return null;
        }

        _fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);

        onComplete?.Invoke();
    }
}
