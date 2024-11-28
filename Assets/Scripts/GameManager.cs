using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Ingame _ingame;

    [Header("Player Management")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private float _respawnDelay;
    public Player player;

    [Header("Fruit Management")]
    public int fruitsCollected;
    public int totalFruits;
    public Fruit[] allFruits;

    [Header("Traps")]
    public GameObject arrowPrefab;

    [Header("Level Management")]
    [SerializeField] private int _currentLevelIndex;
    private int _nextLevelIndex;
    [SerializeField] private float _levelTimer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    private void Start()
    {
        _ingame = Ingame.Instance;
        _currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        CollectFruitInfo();
        _nextLevelIndex = _currentLevelIndex + 1;
    }

    private void Update()
    {
        _levelTimer += Time.deltaTime;

        _ingame.UpdateTimerUI(_levelTimer);
    }

    #region Fruit

    private void CollectFruitInfo()
    {
        allFruits = FindObjectsOfType<Fruit>();
        totalFruits = allFruits.Length;
        _ingame.UpdateFruitUI(fruitsCollected, totalFruits);
        PlayerPrefs.SetInt("Level" + _currentLevelIndex + "TotalFruits", totalFruits);
    }

    public void AddFruit()
    {
        fruitsCollected++;
        _ingame.UpdateFruitUI(fruitsCollected, totalFruits);
    }

    public void RemoveFruit()
    {
        fruitsCollected--;
        _ingame.UpdateFruitUI(fruitsCollected, totalFruits);
    }

    public int GetFruitsCollected()
    {
        return fruitsCollected;
    }

    #endregion

    #region Player

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(_respawnDelay);
        GameObject newPlayer = Instantiate(_playerPrefab, _respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player>();
    }

    public void RespawnPlayer()
    {
        // Only respawn player if the difficulty is NOT hard
        DifficultyManager difficultyManager = DifficultyManager.Instance;
        if (difficultyManager != null && difficultyManager.difficulty == DifficultyType.Hard)
            return;

        StartCoroutine(RespawnCoroutine());
    }

    public void UpdateCheckpointPosition(Transform newRespawnPoint)
    {
        _respawnPoint = newRespawnPoint;
    }

    #endregion

    #region Traps

    private IEnumerator CreateObjectCoroutine(GameObject prefab, Transform target, float delay)
    {
        Vector3 newPosition = target.position;
        yield return new WaitForSeconds(delay);
        GameObject newObject = Instantiate(prefab, newPosition, Quaternion.identity);
    }

    public void CreateObject(GameObject prefab, Transform target, float delay = 0f)
    {
        StartCoroutine(CreateObjectCoroutine(prefab, target, delay));
    }

    #endregion

    #region Levels & Scenes
    private void LoadCurrentScene()
    {
        SceneManager.LoadScene("Level_" + _currentLevelIndex);
    }
    public void RestartLevel()
    {
        Ingame.Instance.fadeEffect.ScreenFadeEffect(1, 1.5f, LoadCurrentScene);
    }
    private void LoadTheEndScene()
    {
        SceneManager.LoadScene("TheEnd");
    }

    private void LoadNextLevel()
    {   
        SceneManager.LoadScene("Level_" + _nextLevelIndex);
    }
    public void LevelFinished()
    {
        SaveLevelProgression();
        SaveBestTime();
        SaveFruitsInfo();
        LoadNextScene();
    }

    private void SaveLevelProgression()
    {
        PlayerPrefs.SetInt("Level" + _nextLevelIndex + "Unlocked", 1);

        if (!NoMoreLevels())
            PlayerPrefs.SetInt("ContinueLevelNumber", _nextLevelIndex);
    }

    private void LoadNextScene()
    {
        FadeEffect fadeEffect = Ingame.Instance.fadeEffect;

        if (NoMoreLevels())
            fadeEffect.ScreenFadeEffect(1, 2f, LoadTheEndScene);
        else
            fadeEffect.ScreenFadeEffect(1, 2f, LoadNextLevel);
    }

    private bool NoMoreLevels()
    {
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 2; // Except the Main Menu & The End scene in Scene Manager

        return _currentLevelIndex == lastLevelIndex;
    }
    #endregion

    #region Best Records

    private void SaveBestTime()
    {
        float lastTime = PlayerPrefs.GetFloat("Level" + _currentLevelIndex + "BestTime", 99);
        
        if (_levelTimer <  lastTime)
            PlayerPrefs.SetFloat("Level" + _currentLevelIndex + "BestTime", _levelTimer);
    }

    private void SaveFruitsInfo()
    {
        int fruitsCollectedBefore = PlayerPrefs.GetInt("Level" + _currentLevelIndex + "FruitsCollected");

        if (fruitsCollectedBefore < fruitsCollected)
            PlayerPrefs.SetInt("Level" + _currentLevelIndex + "FruitsCollected", fruitsCollected);

        int totalFruitsInBank = PlayerPrefs.GetInt("TotalFruitsAmount");

        PlayerPrefs.SetInt("TotalFruitsAmount", totalFruitsInBank + fruitsCollected);
    }

    #endregion
}
