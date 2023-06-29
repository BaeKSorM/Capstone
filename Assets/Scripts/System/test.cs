using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Slider slider;
    public AudioMixer audioMixer;
    public string aname;
    void Awake()
    {
        slider = GetComponent<Slider>();
    }
    public void SetBgmVolme()
    {
        // 로그 연산 값 전달
        audioMixer.SetFloat(aname, Mathf.Log10(slider.value) * 20);
    }

    void Start()
    {
        // "Horizontal"의 negative button을 "alpha5"로 변경

    }
}
