using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    [SerializeField] internal GameObject startCanvas, loadingCanvas;
    [SerializeField] internal GameObject pause, restart, control, keyPanel, setting, exit;
    [SerializeField] bool load;
    [SerializeField] internal List<Button> Buttons;


    [Tooltip("검은 색되고 바로 흰색으로 변하니까 잠깐기다리는 시간")]
    public float waitTime = 0.1f;
    [SerializeField] internal TMP_Text[] buttonTexts;
    [SerializeField] internal Image[] buttonImages;
    [SerializeField] internal Vector2 targetResolution;
    [SerializeField] internal FullScreenMode mode;
    [SerializeField] FadeInOut fadeInOut;
    [SerializeField] internal List<Button> keyPads;
    [SerializeField] internal List<string> resetKeys;

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
    internal void BrightButtonText(Button bright)
    {
        foreach (Button bButton in Buttons)
        {
            if (bright == bButton)
            {
                buttonTexts = bButton.GetComponentsInChildren<TMP_Text>(false);
                buttonImages = bButton.GetComponentsInChildren<Image>(false);
                foreach (TMP_Text text in buttonTexts)
                {
                    text.color = new Color(255, 255, 255, 255);
                }
                foreach (Image image in buttonImages)
                {
                    image.color = new Color(255, 255, 255, 255);
                }
            }
            else
            {
                buttonTexts = bButton.GetComponentsInChildren<TMP_Text>(false);
                buttonImages = bButton.GetComponentsInChildren<Image>(false);
                foreach (TMP_Text text in buttonTexts)
                {
                    text.color = new Color(0, 0, 0, 255);
                }
                foreach (Image image in buttonImages)
                {
                    image.color = new Color(0, 0, 0, 255);
                }
            }
        }
    }
    /// <summary>
    /// 시작할때 전에 저장한 스테이지 불러오기
    /// </summary>
    public IEnumerator loading()
    {
        fadeInOut.inOrOut = FadeInOut.InOrOut.Out;
        yield return new WaitUntil(() => fadeInOut.fOut);
        GameManager.instance.stageStart(PlayerPrefs.GetInt("SaveLevel"));
    }

    public void Pause()
    {
        if (keyPanel.activeSelf)
        {
            keyPanel.SetActive(false);
        }
        else
        if (restart.activeSelf)
        {
            BrightButtonText(Buttons[0]);
            restart.SetActive(false);
        }
        else if (exit.activeSelf)
        {
            BrightButtonText(Buttons[0]);
            exit.SetActive(false);
        }
        else if (control.activeSelf)
        {
            BrightButtonText(Buttons[0]);
            control.SetActive(false);
            for (int i = 0; i < keyPads.Count; ++i)
            {
                ButtonEvent bt = keyPads[i].GetComponent<ButtonEvent>();
                if (bt.keyPressed != "")
                {
                    PlayerPrefs.SetString(keyPads[i].name, bt.keyPressed);
                    PlayerController.instance.keys[i] = bt.keyPressed;
                }
            }

            Time.timeScale = 1;
        }
        else if (setting.activeSelf)
        {
            BrightButtonText(Buttons[0]);
            setting.SetActive(false);
            Time.timeScale = 1;
            Screen.SetResolution((int)targetResolution.x, (int)targetResolution.y, mode);
        }
        else
        {
            if (!pause.activeSelf)
            {
                pause.SetActive(true);
                Time.timeScale = 0;
                BrightButtonText(Buttons[0]);
            }
            else if (pause.activeSelf)
            {
                pause.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
    public void Resume()
    {
        pause.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        restart.SetActive(true);
    }
    public void RealRestart()
    {
        SceneManager.LoadScene(1);
    }
    public void Control()
    {
        BrightButtonText(Buttons[13]);
        control.SetActive(true);
        Time.timeScale = 0;
    }
    public void KeyReset()
    {
        for (int i = 0; i < keyPads.Count; ++i)
        {
            ButtonEvent bt = keyPads[i].GetComponent<ButtonEvent>();
            bt.keyPressed = resetKeys[i];
            Sprite sprite = Resources.Load<Sprite>("Images" + resetKeys[i]);
            // Debug.Log();
            keyPads[i].transform.Find("Key").GetComponent<Image>().sprite = sprite;
        }
    }

    public void Setting()
    {
        BrightButtonText(Buttons[5]);
        setting.SetActive(true);
        Time.timeScale = 0;
    }
    public void Exit()
    {
        BrightButtonText(Buttons[12]);
        exit.SetActive(true);
    }
    public void RealEixt()
    {
        PlayerPrefs.SetInt("SaveLevel", (int)GameManager.instance.age);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}