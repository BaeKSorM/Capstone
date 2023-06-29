using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ButtonEvent : MonoBehaviour
{
    Button button;
    [SerializeField] internal int count = 0;
    [SerializeField] internal string[] collections;
    [SerializeField] internal TMP_Text selected;
    [SerializeField] internal TMP_Text[] buttonTexts;
    [SerializeField] internal Image[] buttonImages;
    [SerializeField] internal bool resolusion;
    [SerializeField] internal bool screen;
    [SerializeField] internal bool changeKey;
    [SerializeField] internal bool control;
    [SerializeField] internal Image image;
    [SerializeField] internal GameObject keyPanel;
    [SerializeField] internal string keyPressed;
    void Awake()
    {
        button = GetComponent<Button>();
        if (resolusion)
        {
            selected.text = PlayerPrefs.GetString("Resolution");
        }
        if (screen)
        {
            selected.text = PlayerPrefs.GetString("Screen");
        }
        if (control)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Images");
            foreach (Sprite sprite in sprites)
            {
                if (sprite.name == PlayerPrefs.GetString(gameObject.name))
                {
                    image.sprite = sprite;
                }
            }
        }
    }
    public void CheckButton()
    {
        // Debug.Log(gameObject.name);
        UIManager.instance.BrightButtonText(button);
    }
    public void PointerDown()
    {
        buttonTexts = GetComponentsInChildren<TMP_Text>(false);
        buttonImages = GetComponentsInChildren<Image>(false);
        foreach (TMP_Text text in buttonTexts)
        {
            text.color = new Color(0, 0, 0, 255);
        }
        foreach (Image image in buttonImages)
        {
            image.color = new Color(0, 0, 0, 255);
        }
    }
    public void PointerUp()
    {
        buttonTexts = GetComponentsInChildren<TMP_Text>(false);
        buttonImages = GetComponentsInChildren<Image>(false);
        foreach (TMP_Text text in buttonTexts)
        {
            text.color = new Color(255, 255, 255, 255);
        }
        foreach (Image image in buttonImages)
        {
            image.color = new Color(255, 255, 255, 255);
        }
    }
    public void Next()
    {
        if (count + 1 < collections.Length)
        {
            ++count;
        }
        else
        {
            count = 0;
        }
        selected.text = collections[count];
        if (resolusion)
        {
            int n = selected.text.IndexOf("x");
            int x = int.Parse(selected.text.Substring(0, n));
            int y = int.Parse(selected.text.Substring(n + 1));
            UIManager.instance.targetResolution = new Vector2(x, y);
        }
        else if (screen)
        {
            if (selected.text == collections[0])
            {
                PlayerPrefs.SetString("Screen", selected.text);
                UIManager.instance.mode = FullScreenMode.FullScreenWindow;
                return;
            }
            if (selected.text == collections[1])
            {
                PlayerPrefs.SetString("Screen", selected.text);
                UIManager.instance.mode = FullScreenMode.ExclusiveFullScreen;
                return;
            }
            if (selected.text == collections[2])
            {
                PlayerPrefs.SetString("Screen", selected.text);
                UIManager.instance.mode = FullScreenMode.Windowed;
                return;
            }
        }
    }
    public void Prev()
    {
        if (count - 1 >= 0)
        {
            --count;
        }
        else
        {
            count = collections.Length - 1;
        }
        selected.text = collections[count];
        if (resolusion)
        {
            PlayerPrefs.SetString("Resolution", selected.text);
            int n = selected.text.IndexOf("x");
            int x = int.Parse(selected.text.Substring(0, n));
            int y = int.Parse(selected.text.Substring(n + 1));
            UIManager.instance.targetResolution = new Vector2(x, y);
        }
        else if (screen)
        {
            if (selected.text == collections[0])
            {
                PlayerPrefs.SetString("Screen", selected.text);
                UIManager.instance.mode = FullScreenMode.FullScreenWindow;
                return;
            }
            if (selected.text == collections[1])
            {
                PlayerPrefs.SetString("Screen", selected.text);
                UIManager.instance.mode = FullScreenMode.ExclusiveFullScreen;
                return;
            }
            if (selected.text == collections[2])
            {
                PlayerPrefs.SetString("Screen", selected.text);
                UIManager.instance.mode = FullScreenMode.Windowed;
                return;
            }
        }
    }
    void Update()
    {
        if (changeKey)
        {
            if (Input.anyKeyDown)
            {
                keyPressed = GetKeyPressed().ToString();
                Debug.Log(keyPressed);
                Sprite[] sprites = Resources.LoadAll<Sprite>("Images");
                foreach (Sprite sprite in sprites)
                {
                    if (sprite.name == keyPressed)
                    {
                        image.sprite = sprite;
                        break;
                    }
                }
                changeKey = false;
                keyPanel.SetActive(false);
            }
        }
    }
    public void ChangeKey()
    {
        keyPanel.SetActive(true);
        changeKey = true;
    }
    KeyCode GetKeyPressed()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                return keyCode;
            }
        }
        return KeyCode.None;
    }
}
