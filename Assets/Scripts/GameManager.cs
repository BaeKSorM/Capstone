using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("스테이지")]
    [Tooltip("스테이지 이름")]
    public List<string> stages;
    [Tooltip("현재 스테이지")]
    public int saveStageLevel;
    [Header("페이드 인아웃")]
    [Tooltip("페이드 인아웃되는 배경")]
    public Image fadePanel;
    [Tooltip("페이드 인아웃되는 시간")]
    public float fadeTime;
    public FadeInOut fadeInOut;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        fadeInOut = FindObjectOfType<FadeInOut>();
    }
    public void stageStart(int _stageLevel)
    {
        saveStageLevel = _stageLevel;
        SceneManager.LoadScene(stages[_stageLevel]);
        fadeInOut.FadeIn();
        Debug.Log(stages[_stageLevel]);
    }
    public void GameEixt()
    {
        PlayerPrefs.SetInt("SaveLevel", saveStageLevel);
    }
}
