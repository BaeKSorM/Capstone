using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] internal AudioMixer audioMixer;
    [SerializeField] internal Slider volume;
    [SerializeField] internal Slider music;
    [SerializeField] internal Slider soundEffect;
    void Awake()
    {
        instance = this;
    }
    public void SetVolme()
    {
        PlayerPrefs.SetFloat("Volume", Mathf.Log10(volume.value) * 20);
        audioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("Volume"));
    }
    public void SetMusicVolme()
    {
        PlayerPrefs.SetFloat("Music", Mathf.Log10(music.value) * 20);
        audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("Music"));
    }
    public void SetSoundEffectVolme()
    {
        PlayerPrefs.SetFloat("SFX", Mathf.Log10(soundEffect.value) * 20);
        audioMixer.SetFloat("SoundEffect", PlayerPrefs.GetFloat("SFX"));
    }
}
