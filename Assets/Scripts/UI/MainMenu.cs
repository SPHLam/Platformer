using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevelName;
    private FadeEffect _fadeEffect;
    [SerializeField] private GameObject[] _uiElements;
    [SerializeField] private GameObject _continueButton;
    private void Awake()
    {
        _fadeEffect = GetComponentInChildren<FadeEffect>(); 
    }
    private void Start()
    {
        if (HasLevelProgression())
            _continueButton.SetActive(true);
        
        _fadeEffect.ScreenFadeEffect(0, 3f);
    }

    public void SwitchUI(GameObject uiToEnable)
    {
        foreach(GameObject uiElement in _uiElements)
        {
            uiElement.SetActive(false);
        }
        uiToEnable.SetActive(true);
    }
    public void NewGame()
    {
        _fadeEffect.ScreenFadeEffect(1, 3f, LoadLevelScene);
    }

    private void LoadLevelScene()
    {
        SceneManager.LoadScene(firstLevelName);
    }

    private bool HasLevelProgression()
    {
        return PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;
    }

    public void ContinueGame()
    {
        int difficultyIndex = PlayerPrefs.GetInt("GameDifficulty", 1);
        int levelToLoad = PlayerPrefs.GetInt("ContinueLevelNumber", 0);
        DifficultyManager.Instance.LoadDifficulty(difficultyIndex);
        SceneManager.LoadScene("Level_" + levelToLoad);
    }
}
