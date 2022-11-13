using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource backgroundAudioSource;
    [SerializeField] float volume;

    [SerializeField] Image volumeButtonIcon;
    [SerializeField] Sprite[] volumeIcons;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        volume = PlayerPrefs.GetFloat("volume", 1);
    }

    private void Update()
    {
        volume = PlayerPrefs.GetFloat("volume", 1);
        audioSource.volume = volume;
        backgroundAudioSource.volume = volume;
    }

    public void ToggleAudio()
    {
        if (volume > 0)
            volume = 0;
        else
            volume = 1;

        PlayerPrefs.SetFloat("volume", volume);
        volumeButtonIcon.sprite = volumeIcons[(int)volume];
    }

    public void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
