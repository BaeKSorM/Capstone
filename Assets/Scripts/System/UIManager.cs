using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

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
    [SerializeField] internal int count;
    [SerializeField] internal bool wait;
    [SerializeField] internal Button eventButton;
    [SerializeField] internal KeyCode[] exceptionKeys;
    RectTransform keyRect;
    float spriteWidth;
    float spriteHeight;
    float spriteRatio;
    float height;
    float width;
    internal Color changeColor;
    internal Color oriColor;
    [SerializeField] internal float checkDelay;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        for (int i = 0; i < keyPads.Count; ++i)
        {
            ButtonEvent bt = keyPads[i].GetComponent<ButtonEvent>();
            PlayerController.instance.keys[i] = PlayerPrefs.GetString(keyPads[i].name);
            // Debug.Log(PlayerPrefs.GetString(keyPads[i].name));
        }
    }
    void Update()
    {
        if (fadeInOut.fIn && !load && Input.anyKeyDown)
        {
            load = true;
            loadingCanvas.SetActive(true);
            StartCoroutine(loading());
        }
        if (pause != null && pause.activeSelf)
        {
            if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerController.instance.keys[0])) && !wait && !eventButton.GetComponent<ButtonEvent>().changeKey)
            {
                StartCoroutine(UpCheck());
            }
            if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerController.instance.keys[1])) && !wait && !eventButton.GetComponent<ButtonEvent>().changeKey)
            {
                StartCoroutine(DownCheck());
            }
            if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerController.instance.keys[2])) && !wait && !eventButton.GetComponent<ButtonEvent>().changeKey)
            {
                StartCoroutine(LeftCheck());
            }
            if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerController.instance.keys[3])) && !wait && !eventButton.GetComponent<ButtonEvent>().changeKey)
            {
                StartCoroutine(RightCheck());
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Buttons[count].onClick.Invoke();
            }
        }
    }
    IEnumerator UpCheck()
    {
        wait = true;
        if (count == 23)
        {
            BrightButtonText(Buttons[22]);
        }
        else if (count == 25)
        {
            BrightButtonText(Buttons[24]);
        }
        else if (count < 22 && count > 12)
        {
            BrightButtonText(Buttons[--count]);
        }
        else if (count == 12)
        {
            BrightButtonText(Buttons[21]);
        }
        else if (count < 22 && count > 5)
        {
            BrightButtonText(Buttons[--count]);
        }
        else if (count < 22 && count == 5)
        {
            BrightButtonText(Buttons[11]);
        }
        else if (count < 22 && count > 0)
        {
            BrightButtonText(Buttons[--count]);
        }
        else if (count == 0)
        {
            BrightButtonText(Buttons[4]);
        }
        float c = 0;
        while (c < checkDelay)
        {
            c += Time.unscaledDeltaTime;
            yield return null;
        }
        wait = false;
    }
    IEnumerator DownCheck()
    {
        wait = true;
        if (count < 4)
        {
            BrightButtonText(Buttons[++count]);
        }
        else if (count == 4)
        {
            BrightButtonText(Buttons[0]);
        }
        else if (count < 11)
        {
            BrightButtonText(Buttons[++count]);
        }
        else if (count == 11)
        {
            BrightButtonText(Buttons[5]);
        }
        else if (count < 21)
        {
            BrightButtonText(Buttons[++count]);
        }
        else if (count == 21)
        {
            BrightButtonText(Buttons[12]);
        }
        else if (count == 22)
        {
            BrightButtonText(Buttons[23]);
        }
        else if (count == 24)
        {
            BrightButtonText(Buttons[25]);
        }
        float c = 0;
        while (c < checkDelay)
        {
            c += Time.unscaledDeltaTime;
            yield return null;
        }
        wait = false;
    }
    IEnumerator LeftCheck()
    {
        wait = true;
        if (count == 23)
        {
            BrightButtonText(Buttons[22]);
        }
        else if (count == 25)
        {
            BrightButtonText(Buttons[24]);
        }
        else if (count < 20 && count > 15)
        {
            count -= 4;
            BrightButtonText(Buttons[count]);
        }
        else if (count > 7 && count < 11)
        {
            Buttons[count].GetComponentInChildren<Slider>().value -= 5;
        }
        else if (count == 5 || count == 6)
        {
            Buttons[count].transform.Find("Left").GetComponent<Button>().onClick.Invoke();
        }
        float c = 0;
        while (c < checkDelay)
        {
            c += Time.unscaledDeltaTime;
            yield return null;
        }
        wait = false;
    }
    IEnumerator RightCheck()
    {
        wait = true;
        if (count == 22)
        {
            BrightButtonText(Buttons[23]);
        }
        else if (count == 24)
        {
            BrightButtonText(Buttons[25]);
        }
        else if (count < 16 && count > 11)
        {
            count += 4;
            BrightButtonText(Buttons[count]);
        }
        else if (count > 7 && count < 11)
        {
            Buttons[count].GetComponentInChildren<Slider>().value += 5;
        }
        else if (count == 5 || count == 6)
        {
            Buttons[count].transform.Find("Right").GetComponent<Button>().onClick.Invoke();
        }
        float c = 0;
        while (c < checkDelay)
        {
            c += Time.unscaledDeltaTime;
            yield return null;
        }
        wait = false;
    }
    internal void BrightButtonText(Button bright)
    {
        for (int i = 0; i < Buttons.Count; ++i)
        {
            if (bright == Buttons[i])
            {
                buttonTexts = Buttons[i].GetComponentsInChildren<TMP_Text>(false);
                buttonImages = Buttons[i].GetComponentsInChildren<Image>(false);
                foreach (TMP_Text text in buttonTexts)
                {
                    if (text.color != ButtonEvent.instance.textChangeColor)
                    {
                        if (Buttons[i].GetComponent<ButtonEvent>().buttonText)
                        {
                            text.color = ButtonEvent.instance.bTextChangeColor;
                        }
                        else
                        {
                            text.color = ButtonEvent.instance.textChangeColor;
                        }
                    }
                }
                foreach (Image image in buttonImages)
                {
                    if (image.color != ButtonEvent.instance.imageChangeColor)
                    {
                        image.color = ButtonEvent.instance.imageChangeColor;
                    }
                }
                count = i;
                Debug.Log(i);
            }
            else
            {
                buttonTexts = Buttons[i].GetComponentsInChildren<TMP_Text>(false);
                buttonImages = Buttons[i].GetComponentsInChildren<Image>(false);
                foreach (TMP_Text text in buttonTexts)
                {
                    if (text.color != ButtonEvent.instance.textOriColor)
                    {
                        if (Buttons[i].GetComponent<ButtonEvent>().buttonText)
                        {
                            text.color = ButtonEvent.instance.bTextChangeColor;
                        }
                        else
                        {
                            text.color = ButtonEvent.instance.textOriColor;
                        }
                    }
                }
                foreach (Image image in buttonImages)
                {
                    if (image.color != ButtonEvent.instance.imageOriColor)
                    {
                        image.color = ButtonEvent.instance.imageOriColor;
                    }
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
        if (!GameManager.instance.open && !GameManager.instance.end)
        {
            if (PlayerController.instance.hpbar.value <= 0)
            {
                PlayerController.instance.hpbar.value = PlayerController.instance.hpbar.maxValue;
            }
        }
        // Debug.Log(PlayerPrefs.GetInt("SaveLevel"));
        if (!GameManager.instance.open && !GameManager.instance.end)
        {
            PlayerPrefs.SetFloat("PlayerHp", PlayerController.instance.hpbar.value);
        }
        GameManager.instance.stageStart();
    }

    public void Pause()
    {
        if (keyPanel.activeSelf)
        {
            keyPanel.SetActive(false);
            Debug.Log(1);
        }
        else
        if (restart.activeSelf)
        {
            Debug.Log(2);
            BrightButtonText(Buttons[0]);
            restart.SetActive(false);
        }
        else if (exit.activeSelf)
        {
            Debug.Log(3);
            BrightButtonText(Buttons[0]);
            exit.SetActive(false);
        }
        else if (control.activeSelf)
        {
            Debug.Log(4);
            BrightButtonText(Buttons[0]);

            control.SetActive(false);
            for (int i = 0; i < keyPads.Count; ++i)
            {
                ButtonEvent bt = keyPads[i].GetComponent<ButtonEvent>();
                if (bt.keyPressed != "")
                {
                    PlayerPrefs.SetString(keyPads[i].name, bt.keyPressed);
                    PlayerController.instance.keys[i] = PlayerPrefs.GetString(keyPads[i].name);
                }
            }

            Time.timeScale = 1;
        }
        else if (setting.activeSelf)
        {
            Debug.Log(5);
            BrightButtonText(Buttons[0]);
            setting.SetActive(false);
            Time.timeScale = 0;
            SaveResolution();
            SoundManager.instance.VolumeSave();
        }
        else
        {
            if (!pause.activeSelf)
            {
                GameManager.instance.pause = true;
                Debug.Log(6);
                pause.SetActive(true);
                Time.timeScale = 0;
                BrightButtonText(Buttons[0]);
            }
            else if (pause.activeSelf)
            {
                GameManager.instance.pause = false;
                Debug.Log(7);
                pause.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
    public void SaveResolution()
    {
        PlayerPrefs.SetInt("ScreenWidth", (int)targetResolution.x);
        PlayerPrefs.SetInt("ScreenHeight", (int)targetResolution.y);
        PlayerPrefs.SetString("mode", mode.ToString());
        Screen.SetResolution((int)targetResolution.x, (int)targetResolution.y, mode);
    }
    public void Resume()
    {
        pause.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        restart.SetActive(true);
        BrightButtonText(Buttons[23]);
    }
    public void RealRestart()
    {
        PlayerPrefs.SetInt("SaveLevel", 0);
        Debug.Log("RealRestart");
        SceneManager.LoadScene(1);
    }
    public void Control()
    {
        BrightButtonText(Buttons[12]);
        control.SetActive(true);
        Time.timeScale = 0;
    }
    public void KeyReset()
    {
        for (int i = 0; i < keyPads.Count; ++i)
        {
            ButtonEvent bt = keyPads[i].GetComponent<ButtonEvent>();
            bt.keyPressed = resetKeys[i];
            Sprite[] sprites = Resources.LoadAll<Sprite>("Images");
            foreach (Sprite sprite in sprites)
            {
                if (sprite.name == resetKeys[i])
                {
                    keyPads[i].transform.Find("Key").GetComponent<Image>().sprite = sprite;
                    keyRect = keyPads[i].transform.Find("Key").GetComponent<RectTransform>();
                    spriteWidth = sprite.bounds.size.x;
                    spriteHeight = sprite.bounds.size.y;
                    spriteRatio = spriteWidth / spriteHeight;
                    height = keyRect.rect.height;
                    width = height * spriteRatio;
                    keyRect.sizeDelta = new Vector2(width, height);
                    PlayerController.instance.keys[i] = bt.keyPressed;
                    Debug.Log("reset");
                    break;
                }
            }
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
        BrightButtonText(Buttons[25]);
        exit.SetActive(true);
    }
    public void RealExit()
    {
        PlayerPrefs.SetInt("SaveLevel", (int)GameManager.instance.age);
        Debug.Log("RealExit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}