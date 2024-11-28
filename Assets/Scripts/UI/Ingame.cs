using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ingame : MonoBehaviour
{
    public static Ingame Instance;
    public FadeEffect fadeEffect { get; private set; }

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _fruitText;

    [SerializeField] private GameObject _pauseUI;

    private bool _isPaused;
    private void Awake()
    {
        Instance = this;
        fadeEffect = GetComponentInChildren<FadeEffect>();
    }
    private void Start()
    {
        fadeEffect.ScreenFadeEffect(0, 1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseButton();
        }
    }

    public void PauseButton()
    {
        if (_isPaused)
        {
            _isPaused = false;
            Time.timeScale = 1;
            _pauseUI.SetActive(false);
        }
        else
        {
            _isPaused = true;
            Time.timeScale = 0;
            _pauseUI.SetActive(true);
        }
    }

    public void GoToMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
    
    public void UpdateFruitUI(int collectedFruits, int totalFruits)
    {
        _fruitText.text = collectedFruits + "/" + totalFruits;
    }

    public void UpdateTimerUI(float timer)
    {
        _timerText.text = timer.ToString("00") + " s";
    }
}
