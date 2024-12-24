using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ingame : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Player _player;
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
        _playerInput = new PlayerInput();
    }
    private void Start()
    {
        fadeEffect.ScreenFadeEffect(0, 1f);
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.UI.Pause.performed += ctx => PauseButton();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.UI.Pause.performed -= ctx => PauseButton();
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
            UnPauseTheGame();

        else
            PauseTheGame();
    }

    private void PauseTheGame()
    {
        _player._playerInput.Disable();
        _isPaused = true;
        Time.timeScale = 0;
        _pauseUI.SetActive(true);
    }

    private void UnPauseTheGame()
    {
        _player._playerInput.Enable();
        _isPaused = false;
        Time.timeScale = 1;
        _pauseUI.SetActive(false);
    }

    public void GoToMainMenuButton()
    {
        Time.timeScale = 1;
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
