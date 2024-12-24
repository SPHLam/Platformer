using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource[] _sfx;
    [SerializeField] private AudioSource[] _bgm;

    [SerializeField] private int _bgmIndex;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        InvokeRepeating(nameof(PlayMusicIfNeeded), 0, 2);
    }

    public void PlayRandomBGM()
    {
        _bgmIndex = Random.Range(0, _bgm.Length);
        PlayBGM(_bgmIndex);
    }

    public void PlayMusicIfNeeded()
    {
        if (_bgm[_bgmIndex].isPlaying == false)
            PlayRandomBGM();
    }

    public void PlaySFX(int sfxIndex, bool randomPitch = false)
    {
        if (sfxIndex >= _sfx.Length)
            return;

        if (randomPitch)
            _sfx[sfxIndex].pitch = Random.Range(0.9f, 1.1f);

        _sfx[sfxIndex].Play();
    }

    public void PlayBGM(int bgmIndex)
    {
        for (int i = 0; i < _bgm.Length; i++)
        {
            _bgm[i].Stop();
        }
        _bgmIndex = bgmIndex;
        _bgm[bgmIndex].Play();
    }

    public void StopSFX(int sfxIndex)
    {
        _sfx[sfxIndex].Stop();
    }
}
