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
        audioMixer.SetFloat("Volume", volume.value - 80);
    }
    public void SetMusicVolme()
    {
        PlayerPrefs.SetFloat("Music", music.value - 80);
    }
    public void SetSoundEffectVolme()
    {
        PlayerPrefs.SetFloat("SFX", soundEffect.value - 80);
    }
    public void VolumeSave()
    {
        PlayerPrefs.SetFloat("Volume", volume.value - 80);
        audioMixer.SetFloat("Music", music.value - 80);
        audioMixer.SetFloat("SoundEffect", soundEffect.value - 80);
    }
}
