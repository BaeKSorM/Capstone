using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject startCanvas, loadingCanvas;
    public FadeInOut fadeInOut;

    public float fadeWaitTime;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        fadeInOut = FindObjectOfType<FadeInOut>();
    }
    public void GameStart()
    {
        if (Input.anyKeyDown)
        {
            loadingCanvas.SetActive(true);
            fadeInOut.FadeOut();
            StartCoroutine(loading());
        }
    }

    IEnumerator loading()
    {
        yield return new WaitForSeconds(GameManager.instance.fadeTime);
        GameManager.instance.stageStart(PlayerPrefs.GetInt("SaveLevel"));
    }
}
