using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private float _mixerMultiplier = 25f;

    [Header("SFX Settings")]
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private TextMeshProUGUI _sfxSliderText;
    [SerializeField] private string _sfxParameter;

    [Header("BGM Settings")]
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private TextMeshProUGUI _bgmSliderText;
    [SerializeField] private string _bgmParameter;

    public void SFXSliderValue(float value)
    {
        _sfxSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        float newValue = Mathf.Log10(value) * _mixerMultiplier;
        _audioMixer.SetFloat(_sfxParameter, newValue);
    }

    public void BGMSliderValue(float value)
    {
        _bgmSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        float newValue = Mathf.Log10(value) * _mixerMultiplier;
        _audioMixer.SetFloat(_bgmParameter, newValue);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(_sfxParameter, _sfxSlider.value);
        PlayerPrefs.SetFloat(_bgmParameter, _bgmSlider.value);
    }

    private void OnEnable()
    {
        _sfxSlider.value = PlayerPrefs.GetFloat(_sfxParameter, .7f);
        _bgmSlider.value = PlayerPrefs.GetFloat(_bgmParameter, .7f);
    }
}
