using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelNumberText;
    [SerializeField] private TextMeshProUGUI _bestTimeText;
    [SerializeField] private TextMeshProUGUI _fruitsText;
    private string _sceneName;
    private int _levelIndex;
    public void LoadLevel()
    {
        int difficultyIndex = (int)DifficultyManager.Instance.difficulty;
        PlayerPrefs.SetInt("GameDifficulty", difficultyIndex);
        SceneManager.LoadScene(_sceneName);
        AudioManager.Instance.PlaySFX(4);
    }

    public void SetUpButton(int levelIndex)
    {
        _levelIndex = levelIndex;
        _levelNumberText.text = "Level " + _levelIndex;
        _sceneName = "Level_" + _levelIndex;
        _bestTimeText.text = TimerInfoText();
        _fruitsText.text = FruitsInfoText();
    }

    private string TimerInfoText()
    {
        float timerValue = PlayerPrefs.GetFloat("Level" + _levelIndex + "BestTime", 0);
        return "Best Time: " + timerValue.ToString("00");
    }

    private string FruitsInfoText()
    {
        int totalFruits = PlayerPrefs.GetInt("Level" + _levelIndex + "TotalFruits", 0);
        string totalFruitsText = (totalFruits == 0) ? "?" : totalFruits.ToString();
        int fruitsCollected = PlayerPrefs.GetInt("Level" + _levelIndex + "FruitsCollected");
        return "Fruits: " + fruitsCollected + "/" + totalFruitsText;
    }
}
