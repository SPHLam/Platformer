using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    private FadeEffect _fadeEffect;
    [SerializeField] private RectTransform _rectT;
    [SerializeField] private float _scrollSpeed = 100f;
    private float _offScreenPosition = 1300f;
    private bool _creditsSkip;
    private void Awake()
    {
        _fadeEffect = GetComponentInChildren<FadeEffect>();
        _fadeEffect.ScreenFadeEffect(0, 2f);
    }

    private void Update()
    {
        _rectT.anchoredPosition += Vector2.up * _scrollSpeed * Time.deltaTime;

        if (_rectT.anchoredPosition.y >= _offScreenPosition)
            GoToMainMenu();
    }
    private void GoToMainMenu()
    {
        _fadeEffect.ScreenFadeEffect(1, 1f, SwitchToMenuScene);
    }

    private void SwitchToMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
