using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : Single<AudioManager>
{
    [SerializeField] private GameObject soundPrefab = null;

    [Header("Other")]
    // Sound list and dictionary
    [SerializeField] private SO_SoundList so_soundList = null;
    [SerializeField] private SO_SceneSoundList so_sceneSoundsList = null;
    [SerializeField] private float defaultSceneMusicPlayTimeSeconds = 120f;
    [SerializeField] private float sceneMusicStartMinSecs = 20f;
    [SerializeField] private float sceneMusicStartMaxSecs = 40f;
    [SerializeField] private float musicTransitionSecs = 8f;
    private Dictionary<SoundName, SoundItem> soundDictionary;
    private Dictionary<SceneName, SceneSoundItem> sceneSoundDictionary;
    private Coroutine playSceneSoundsCoroutine;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource ambientSoundAudioSource = null;
    [SerializeField] private AudioSource gameMusicAudioSource = null;
    [Header("Audio Mixers")]
    [SerializeField] private AudioMixer gameAudioMixer = null;
    [Header("Audio Snapshots")]
    [SerializeField] private AudioMixerSnapshot gameMusicSnapshot = null;
    [SerializeField] private AudioMixerSnapshot gameAmbientSnapshot = null;
    protected override void Awake()
    {
        base.Awake();

        // Initialise sound dictionary
        soundDictionary = new Dictionary<SoundName, SoundItem>();
        sceneSoundDictionary = new Dictionary<SceneName, SceneSoundItem>();
        // Load sound dictionary with sounds
        foreach (SoundItem soundItem in so_soundList.soundItemList)
        {
            soundDictionary.Add(soundItem.soundName, soundItem);
        }
        // Load sound dictionary with scene sounds
        foreach (SceneSoundItem sceneSoundItem in so_sceneSoundsList.sceneSoundItemList)
        {
            sceneSoundDictionary.Add(sceneSoundItem.sceneName, sceneSoundItem);
        }
    }
    private void OnEnable()
    {
        EventHandler.SceneLoadAfterEvent += PlaySceneSounds;
    }

    

    private void OnDisable()
    {
        EventHandler.SceneLoadAfterEvent -= PlaySceneSounds;
    }

    private void PlaySceneSounds()
    {
        SoundItem musicSoundItem = null;
        SoundItem ambientSoundItem = null;
        float musicPlayTime = defaultSceneMusicPlayTimeSeconds;
        // Try Get current scene
        if (Enum.TryParse<SceneName>(SceneManager.GetActiveScene().name, true, out SceneName currentSceneName))
        {
            // Get Music and Ambient Sounds For Scene
            if (sceneSoundDictionary.TryGetValue(currentSceneName, out SceneSoundItem sceneSoundsItem))
            {
                soundDictionary.TryGetValue(sceneSoundsItem.musicForScene, out musicSoundItem);
                soundDictionary.TryGetValue(sceneSoundsItem.ambientSoundForScene, out ambientSoundItem);
            }
            else
            {
                return;
            }
            // Stop any scene sounds already playing
            if (playSceneSoundsCoroutine != null)
            {
                StopCoroutine(playSceneSoundsCoroutine);
            }
            // Play scene ambient sounds and music
            playSceneSoundsCoroutine = StartCoroutine(PlaySceneSoundsRoutine(musicPlayTime, musicSoundItem, ambientSoundItem));
        }
    }

    private IEnumerator PlaySceneSoundsRoutine(float musicPlaySeconds, SoundItem musicSoundItem, SoundItem ambientSoundItem)
    {
        if (musicSoundItem != null && ambientSoundItem != null)
        {
            // Start with ambient sound
            PlayAmbientSoundClip(ambientSoundItem, 0f);
            // Wait for random range of seconds before playing music
            yield return new WaitForSeconds(UnityEngine.Random.Range(sceneMusicStartMinSecs, sceneMusicStartMaxSecs));
            // Play music
            PlayMusicSoundClip(musicSoundItem, musicTransitionSecs);
            // Wait for music play seconds before transitioning to ambient sounds
            yield return new WaitForSeconds(musicPlaySeconds);
            // Play ambient sound clip
            PlayAmbientSoundClip(ambientSoundItem, musicTransitionSecs);
        }
    }
    private void PlayMusicSoundClip(SoundItem musicSoundItem, float transitionTimeSeconds)
    {
        // Set Volume
        gameAudioMixer.SetFloat("MusicVolume", ConvertSoundVolumeDecimalFractionToDecibels(musicSoundItem.soundVolume));
        // Set clip & play
        gameMusicAudioSource.clip = musicSoundItem.soundClip;
        gameMusicAudioSource.Play();
        // Transition to music snapshot
        gameMusicSnapshot.TransitionTo(transitionTimeSeconds);
    }
    private void PlayAmbientSoundClip(SoundItem ambientSoundItem, float transitionTimeSeconds)
    {
        // Set Volume
        gameAudioMixer.SetFloat("AmbientVolume", ConvertSoundVolumeDecimalFractionToDecibels(ambientSoundItem.soundVolume));
        // Set clip & play
        ambientSoundAudioSource.clip = ambientSoundItem.soundClip;
        ambientSoundAudioSource.Play();
        // Transition to ambient
        gameAmbientSnapshot.TransitionTo(transitionTimeSeconds);
    }
    private float ConvertSoundVolumeDecimalFractionToDecibels(float volumeDecimalFraction)
    {
        // Convert volume from decimal fraction to -80 to +20 decibel range
        return (volumeDecimalFraction * 100f - 80f);
    }

    public void PlaySound(SoundName soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out SoundItem soundItem) && soundPrefab != null)
        {
            GameObject soundGameObject = PoolManager.Instance.ReuseObject(soundPrefab, Vector3.zero, Quaternion.identity);
            Sounds sound = soundGameObject.GetComponent<Sounds>();

            sound.SetSound(soundItem);
            soundGameObject.SetActive(true);
            StartCoroutine(DisableSound(soundGameObject, soundItem.soundClip.length));
        }
    }
    private IEnumerator DisableSound(GameObject soundGameObject, float soundDuration)
    {
        yield return new WaitForSeconds(soundDuration);
        soundGameObject.SetActive(false);
    }
}
