using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut instance;
    [Tooltip("검은 색되고 바로 흰색으로 변하니까 잠깐기다리는 시간")]
    float waitTime = 0.1f;
    private void Awake()
    {
        instance = this;
    }
    public void FadeIn()
    {
        StartCoroutine(FadeInCor(GameManager.instance.fadePanel, GameManager.instance.fadeTime));
    }
    public void FadeOut()
    {
        StartCoroutine(FadeOutCor(GameManager.instance.fadePanel, GameManager.instance.fadeTime));
    }

    // 밝아지기
    public IEnumerator FadeInCor(Image fadeScreen, float fadeTime)
    {
        while (fadeScreen.color.a > 0)
        {
            fadeScreen.color -= new Color(0, 0, 0, 1 / (fadeTime * 100));
            yield return new WaitForSeconds(0.01f);
        }
    }
    // 어두워지기
    public IEnumerator FadeOutCor(Image fadeScreen, float fadeTime)
    {
        while (fadeScreen.color.a < 1)
        {
            fadeScreen.color += new Color(0, 0, 0, 1 / (fadeTime * 102));
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(waitTime);
    }
}
