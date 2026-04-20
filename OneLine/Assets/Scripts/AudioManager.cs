using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;
    private AudioSource audioSource;
    private bool justAwoken = true;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        AudioManager[] audioManagers = FindObjectsOfType<AudioManager>();
        AudioManager other = FindOtherManager(audioManagers);

        if (audioManagers.Length > 1 && PlayingSameTrack(other)) {
            Destroy(gameObject);
        }
        else {
            instance = this;
            if (transform.parent != null) {
                transform.SetParent(null);
            }
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioManager[] audioManagers = FindObjectsOfType<AudioManager>();
        AudioManager other = FindOtherManager(audioManagers);

        if (audioManagers.Length > 1 && !PlayingSameTrack(other) &&
            !justAwoken) {
            Destroy(gameObject);
        }

        justAwoken = false;
    }

    private void OnDestroy()
    {
        if (instance == this) {
            instance = null;
        }
    }

    private AudioManager FindOtherManager(AudioManager[] audioManagers)
    {
        foreach (AudioManager audioManager in audioManagers) {
            if (audioManager != this) {
                return audioManager;
            }
        }

        return null;
    }

    private bool PlayingSameTrack(AudioManager other)
    {
        if (audioSource != null && other != null && other.audioSource != null) {
            return audioSource.clip == other.audioSource.clip;
        }

        return false;
    }
}
