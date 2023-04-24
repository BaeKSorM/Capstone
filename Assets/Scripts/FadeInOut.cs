using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut instance;

    [Tooltip("페이드 인아웃되는 시간")]
    [SerializeField] internal float fadeTime;
    [SerializeField] internal enum InOrOut { In, Out, Default };
    [SerializeField] internal InOrOut inOrOut;
    [SerializeField] internal Image background;
    float ff;
    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        switch (inOrOut)
        {
            case InOrOut.In:
                StartCoroutine(FadeInCor(fadeTime));
                inOrOut = InOrOut.Default;
                break;
            case InOrOut.Out:
                StartCoroutine(FadeOutCor(fadeTime));
                inOrOut = InOrOut.Default;
                break;
            default:
                break;
        }
    }
    // 밝아지기
    public IEnumerator FadeInCor(float fadeTime)
    {
        while (background.color.a > 0)
        {
            background.color -= new Color(0, 0, 0, 1 / (fadeTime * 100));
            ff += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
            ff += 0.01f;
        }
        // 인 시간
        // Debug.Log(ff);
    }
    // 어두워지기
    public IEnumerator FadeOutCor(float fadeTime)
    {
        while (background.color.a < 1)
        {
            background.color += new Color(0, 0, 0, 1 / (fadeTime * 100));
            yield return new WaitForSeconds(0.01f);
        }
    }
}
