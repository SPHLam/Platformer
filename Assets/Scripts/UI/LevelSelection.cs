using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private LevelButton _levelButtonPrefab;
    [SerializeField] private Transform _buttonsParent;

    [SerializeField] private bool[] _levelsUnlocked;

    private void Start()
    {
        LoadLevelsInfo();
        CreateLevelButtons();
    }

    private void CreateLevelButtons()
    {
        int totalLevels = SceneManager.sceneCountInBuildSettings - 1;
        for (int i = 1; i < totalLevels; i++)
        {
            if (!IsLevelUnlocked(i))
                return; 

            LevelButton newButton = Instantiate(_levelButtonPrefab, _buttonsParent);
            newButton.SetUpButton(i);
        }
    }

    private bool IsLevelUnlocked(int levelIndex)
    {
        return _levelsUnlocked[levelIndex];
    }

    private void LoadLevelsInfo()
    {
        int totalLevels = SceneManager.sceneCountInBuildSettings - 1;

        _levelsUnlocked = new bool[totalLevels];

        for (int i = 1; i < totalLevels; i++)
        {
            bool levelUnlocked = PlayerPrefs.GetInt("Level" + i + "Unlocked", 0) == 1;

            if (levelUnlocked)
            {
                _levelsUnlocked[i] = true;
            }
        }

        _levelsUnlocked[1] = true;
    }
}
