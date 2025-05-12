using UnityEngine;
using System.Collections.Generic;
using Settings;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    private AudioSource _audioSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip _mainMenuMusic;
    [SerializeField] private List<AudioClip> _inGameMusicClips = new List<AudioClip>();
    [SerializeField] private AudioClip extraRoundMusic;
    [SerializeField] private AudioClip gameOverMusic;

    private int _currentInGameTrackIndex = 0;
    private bool _isInGameMusic = false;

    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MusicManager>();
                
                if (_instance == null)
                {
                    GameObject obj = new GameObject("MusicManager");
                    _instance = obj.AddComponent<MusicManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = true;
        }
        
        UpdateVolume();
    }

    public void PlayMainMenuMusic()
    {
        if (_mainMenuMusic == null)
        {
            Debug.LogWarning("Main menu music clip not assigned!");
            return;
        }
        
        Debug.Log("PLAYING MUSIC");
        _isInGameMusic = false;
        _audioSource.clip = _mainMenuMusic;
        _audioSource.Play();
    }
    

    public void PlayInGameMusic(bool shuffle = false)
    {
        if (_inGameMusicClips.Count == 0)
        {
            Debug.LogWarning("No in-game music clips assigned!");
            return;
        }
        Debug.Log("PLAYING INGAME");

        _isInGameMusic = true;
        
        if (shuffle)
        {
            _currentInGameTrackIndex = Random.Range(0, _inGameMusicClips.Count);
        }

        _audioSource.clip = _inGameMusicClips[_currentInGameTrackIndex];
        _audioSource.Play();
    }

    public void PlayInGameMusicExtraRound()
    {
        if (gameOverMusic == null)
        {
            Debug.LogWarning("No in-game music clips assigned!");
            return;
        }
        Debug.Log("PLAYING EXTRA ROUND MUSIC");
        
        _audioSource.clip = extraRoundMusic;
        _audioSource.Play();
    }

    public void PlayInGameMusicGameOver()
    {
        if (gameOverMusic == null)
        {
            Debug.LogWarning("No in-game music clips assigned!");
            return;
        }
        Debug.Log("PLAYING GAME OVER MUSIC");
        
        _audioSource.clip = gameOverMusic;
        _audioSource.Play();
    }

    public void PlayNextInGameTrack()
    {
        if (!_isInGameMusic) return;
        
        _currentInGameTrackIndex = (_currentInGameTrackIndex + 1) % _inGameMusicClips.Count;
        _audioSource.clip = _inGameMusicClips[_currentInGameTrackIndex];
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }

    public static void UpdateMusicVolume()
    {
        if (Instance != null)
        {
            Instance.UpdateVolume();
        }
    }

    private void UpdateVolume()
    {
        var config = ConfigManager.LoadConfig();
        _audioSource.volume = config.settings.musicvolume;
    }
}