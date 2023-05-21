using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    public GameObject startCanvas, loadingCanvas;

    [Tooltip("검은 색되고 바로 흰색으로 변하니까 잠깐기다리는 시간")]
    public float waitTime = 0.1f;

    private void Awake()
    {
        instance = this;
    }
    public void GameStart()
    {
        if (Input.anyKeyDown)
        {
            loadingCanvas.SetActive(true);
            StartCoroutine(loading());
        }
    }

    public IEnumerator loading()
    {
        yield return new WaitForSeconds(FadeInOut.instance.fadeTime + waitTime);
        GameManager.instance.stageStart(PlayerPrefs.GetInt("SaveLevel"));
    }
}
