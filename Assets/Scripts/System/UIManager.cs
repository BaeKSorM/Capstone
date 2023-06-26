using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    [SerializeField] internal GameObject startCanvas, loadingCanvas;
    [SerializeField] bool load;

    [Tooltip("검은 색되고 바로 흰색으로 변하니까 잠깐기다리는 시간")]
    public float waitTime = 0.1f;

    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (Input.anyKeyDown && !load)
        {
            load = true;
            loadingCanvas.SetActive(true);
            StartCoroutine(loading());
        }
    }
    /// <summary>
    /// 시작할때 전에 저장한 스테이지 불러오기
    /// </summary>
    public IEnumerator loading()
    {
        Debug.Log("be");
        FadeInOut.instance.inOrOut = FadeInOut.InOrOut.Out;
        yield return new WaitUntil(() => FadeInOut.instance.fOut);
        Debug.Log("en");
        GameManager.instance.stageStart(PlayerPrefs.GetInt("SaveLevel"));
    }
}