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
    [SerializeField] internal List<string> stages;
    [Tooltip("현재 스테이지")]
    [SerializeField] internal int saveStageLevel;
    private void Awake()
    {
        instance = this;
        PlayerPrefs.SetInt("SaveLevel", 0);
        DontDestroyOnLoad(this);
    }
    public void stageStart(int _stageLevel)
    {
        saveStageLevel = _stageLevel;
        SceneManager.LoadScene(stages[_stageLevel]);
    }
    public void GameEixt()
    {
        PlayerPrefs.SetInt("SaveLevel", saveStageLevel + 1);
    }
}
