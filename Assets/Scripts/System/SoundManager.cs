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
    void Start()
    {
        volume.value = PlayerPrefs.GetFloat("Volume");
        music.value = PlayerPrefs.GetFloat("Music");
        soundEffect.value = PlayerPrefs.GetFloat("SFX");
    }
    public void SetVolme()
    {
        audioMixer.SetFloat("Volume", volume.value);
    }
    public void SetMusicVolme()
    {
        audioMixer.SetFloat("Music", music.value);
    }
    public void SetSoundEffectVolme()
    {
        audioMixer.SetFloat("SoundEffect", soundEffect.value);
    }
    public void VolumeSave()
    {
        PlayerPrefs.SetFloat("Volume", volume.value);
        PlayerPrefs.SetFloat("Music", music.value);
        PlayerPrefs.SetFloat("SFX", soundEffect.value);
    }
}
