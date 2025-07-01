using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    #region Instance
    public static AudioManager Instance;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
    }
    #endregion

    [Header("Variables")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private float _initMasterVol = 0.75f;
    [SerializeField] private float _initMusicVol = 0.5f;
    [SerializeField] private float _initSFXVol = 0.5f;
    [SerializeField] private float _initUIVol = 0.2f;
    [SerializeField] private string _masterFloatName = "MasterVolume";
    [SerializeField] private string _musicFloatName = "MusicVolume";
    [SerializeField] private string _sfxFloatName = "SFXVolume";
    [SerializeField] private string _uiFloatName = "UIVolume";

    public float InitMasterVol => _initMasterVol;
    public float InitMusicVol => _initMusicVol;
    public float InitSFXVol => _initSFXVol;
    public float InitUIVol => _initUIVol;

    private AudioSource _audioSource;

    private void Start()
    {
        SetMasterVolume(InitMasterVol);
        SetMusicVolume(InitMusicVol);
        SetSFXVolume(InitSFXVol);
        SetUIVolume(InitUIVol);
    }

    public void SetMasterVolume(float value)
    {
        if (value <= 0f) value = 0.0001f;

        _audioMixer.SetFloat(_masterFloatName, Mathf.Log(value) * 20f);
    }

    public void SetMusicVolume(float value)
    {
        if (value <= 0f) value = 0.0001f;

        _audioMixer.SetFloat(_musicFloatName, Mathf.Log(value) * 20f);
    }

    public void SetSFXVolume(float value)
    {
        if (value <= 0f) value = 0.0001f;

        _audioMixer.SetFloat(_sfxFloatName, Mathf.Log(value) * 20f);
    }

    public void SetUIVolume(float value)
    {
        if (value <= 0f) value = 0.0001f;

        _audioMixer.SetFloat(_uiFloatName, Mathf.Log(value) * 20f);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (_audioSource.isPlaying) _audioSource.Stop();

        _audioSource.clip = clip;

        _audioSource.Pause();
    }

    public void PlayUI(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}