using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource sfxSource; // Base source for cloning

    [Header("Music Clips")]
    public AudioClip musicBg;
    public AudioClip combatBg;
    public AudioClip introBossRoomBg;
    public AudioClip bossRoomBg;
    public AudioClip hidingSfx;
    public AudioClip ambientHorror;

    [Header("SFX Clips")]
    public AudioClip pickUpSfx;
    public AudioClip pickUpWrongSfx;
    public AudioClip interactSfx;
    public AudioClip interactWrongSfx;
    public AudioClip enterDoorSfx;
    public AudioClip closeDoorSfx;
    public AudioClip ghostSfx;
    public AudioClip damageSfx;
    public AudioClip hittingSfx;
    public AudioClip[] walkingSfx;

    [Header("Settings")]
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;
    [SerializeField] private float fadeOutDuration = 1.0f;
    [SerializeField] private float fadeInDuration = 1.0f;
    [SerializeField] private float minTimeBetweenSteps = 0.3f;
    [SerializeField] private float maxTimeBetweenSteps = 0.6f;

    private float timeSinceLastStep;
    private bool isMusicMuted = false;
    private List<AudioSource> activeSfxClones = new List<AudioSource>(); // Track cloned SFX sources

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(musicBg);
        PlayAmbient(ambientHorror);
    }

    // Music Control
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.volume = 0f;
        musicSource.Play();
        StartCoroutine(FadeAudio(musicSource, 1.0f, fadeInDuration));
    }

    public void ChangeMusic(AudioClip newClip)
    {
        StopMusicFadeOut();
        StartCoroutine(PlayNewMusicAfterFadeOut(newClip));
    }

    private IEnumerator PlayNewMusicAfterFadeOut(AudioClip newClip)
    {
        yield return new WaitForSeconds(fadeOutDuration);
        PlayMusic(newClip);
    }

    public void StopMusicFadeOut() => StartCoroutine(FadeAudio(musicSource, 0f, fadeOutDuration));

    // Ambient Control
    public void PlayAmbient(AudioClip clip)
    {
        ambientSource.clip = clip;
        ambientSource.volume = 0f;
        ambientSource.Play();
        StartCoroutine(FadeAudio(ambientSource, 1.0f, fadeInDuration));
    }

    public void StopAmbientFadeOut() => StartCoroutine(FadeAudio(ambientSource, 0f, fadeOutDuration));

    // SFX Control
    public void PlaySFXClone(AudioClip clip, float volume = 1.0f)
    {
        // Create and configure a new GameObject with an AudioSource for each SFX clone
        GameObject sfxCloneObject = new GameObject("SFXClone_" + clip.name);
        AudioSource cloneSource = sfxCloneObject.AddComponent<AudioSource>();

        // Set up clone properties
        cloneSource.clip = clip;
        cloneSource.volume = volume;
        cloneSource.pitch = Random.Range(minPitch, maxPitch);
        cloneSource.spatialBlend = sfxSource.spatialBlend;
        cloneSource.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;

        // Play, track, and destroy after playing
        cloneSource.Play();
        activeSfxClones.Add(cloneSource);
        Destroy(sfxCloneObject, clip.length);

        // Clean up tracking list after clip finishes
        StartCoroutine(RemoveCloneFromList(cloneSource, clip.length));
    }

    private IEnumerator RemoveCloneFromList(AudioSource cloneSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        activeSfxClones.Remove(cloneSource);
    }

    public void StopSpecificSFXClone(AudioClip clip)
    {
        foreach (var cloneSource in activeSfxClones)
        {
            if (cloneSource.clip == clip && cloneSource.isPlaying)
            {
                cloneSource.Stop();
                Destroy(cloneSource.gameObject);
                activeSfxClones.Remove(cloneSource);
                break;
            }
        }
    }

    public void StopAllSFXClones()
    {
        foreach (var cloneSource in activeSfxClones)
        {
            cloneSource.Stop();
            Destroy(cloneSource.gameObject);
        }
        activeSfxClones.Clear();
    }

    // Walking SFX
    public void PlayWalkingSFX()
    {
        if (walkingSfx.Length == 0 || Time.time - timeSinceLastStep < Random.Range(minTimeBetweenSteps, maxTimeBetweenSteps))
            return;

        AudioClip stepClip = walkingSfx[Random.Range(0, walkingSfx.Length)];
        PlaySFXClone(stepClip);
        timeSinceLastStep = Time.time;
    }

    // General Volume Control
    public void SetMusicVolume(float volume) => musicSource.volume = volume;
    public void SetSFXVolume(float volume) => sfxSource.volume = volume;
    public void SetAmbientVolume(float volume) => ambientSource.volume = volume;

    // Muting Functions
    public void MuteMusic()
    {
        if (!isMusicMuted)
        {
            musicSource.volume = 0f;
            isMusicMuted = true;
        }
    }

    public void UnmuteMusic(float volume = 1.0f)
    {
        if (isMusicMuted)
        {
            musicSource.volume = volume;
            isMusicMuted = false;
            musicSource.Play();
        }
    }

    // Helper for Audio Fading
    private IEnumerator FadeAudio(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float fadeSpeed = (targetVolume - startVolume) / duration;

        while (!Mathf.Approximately(source.volume, targetVolume))
        {
            source.volume = Mathf.MoveTowards(source.volume, targetVolume, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        if (Mathf.Approximately(targetVolume, 0f))
            source.Stop();
    }
}
