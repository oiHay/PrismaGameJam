using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")] 
    [SerializeField] private AudioMixer audioMixer;

    [Header("Mixer Groups")] 
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Sources")] 
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private const string MasterParam = "MasterVolume";
    private const string MusicParam  = "MusicVolume";
    private const string SfxParam    = "SfxVolume";

    private const string MasterKey = "Audio_Master";
    private const string MusicKey = "Audio_Music";
    private const string SfxKey = "Audio_SFX";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.loop = true;

        sfxSource.outputAudioMixerGroup = sfxGroup;
    }

    private void Start()
    {
        LoadSavedVolume();
    }

    private void LoadSavedVolume()
    {
        
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat(MasterParam, LinearToDecibel(value));
        PlayerPrefs.SetFloat(MasterKey, value);
    }
    
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(MusicParam, LinearToDecibel(value));
        PlayerPrefs.SetFloat(MusicKey, value);
    }
    
    public void SetSfxVolume(float value)
    {
        audioMixer.SetFloat(SfxParam, LinearToDecibel(value));
        PlayerPrefs.SetFloat(SfxKey, value);
    }

    public float GetMasterVolume() => PlayerPrefs.GetFloat(MasterKey, 1f);
    public float GetMusicVolume()  => PlayerPrefs.GetFloat(MusicKey, 1f);
    public float GetSfxVolume()    => PlayerPrefs.GetFloat(SfxKey, 1f);

    private float LinearToDecibel(float linear) // Converte 0-1 para decibeis (-80 dB até 0 dB)
    {
        return Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusico()
    {
        musicSource.Stop();
        musicSource.clip = null;
    }
    
    public void PlaySfx(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip, volumeScale);
    }
    
    public static void PlaySound(AudioClip clip)
    {
        if (Instance != null)
            Instance.PlaySfx(clip);
    }
}
