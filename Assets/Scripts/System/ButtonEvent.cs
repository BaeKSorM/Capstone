using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class ButtonEvent : MonoBehaviour
{
    public static ButtonEvent instance;
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
    [SerializeField] internal bool colorOn;
    [SerializeField] internal bool buttonText;
    [SerializeField] internal Image image;
    [SerializeField] internal GameObject keyPanel;
    [SerializeField] internal string keyPressed;
    RectTransform keyRect;
    float spriteWidth;
    float spriteHeight;
    float spriteRatio;
    float height;
    float width;
    [SerializeField] internal Color imageChangeColor;
    [SerializeField] internal Color imageOriColor;
    [SerializeField] internal Color textChangeColor;
    [SerializeField] internal Color textOriColor;
    [SerializeField] internal Color bTextOriColor;
    [SerializeField] internal Color bTextChangeColor;
    [SerializeField] bool once;
    void Awake()
    {
        instance = this;
        // spriteRenderer = transform.Find("Key").GetComponent<SpriteRenderer>();
        button = GetComponent<Button>();
        if (resolusion)
        {
            selected.text = PlayerPrefs.GetString("Resolution");
        }
        else if (screen)
        {
            selected.text = PlayerPrefs.GetString("Screen");
        }
        else if (control)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Images");
            foreach (Sprite sprite in sprites)
            {
                if (sprite.name == PlayerPrefs.GetString(gameObject.name))
                {
                    keyRect = transform.Find("Key").GetComponent<RectTransform>();
                    image.sprite = sprite;
                    spriteWidth = sprite.bounds.size.x;
                    spriteHeight = sprite.bounds.size.y;
                    spriteRatio = spriteWidth / spriteHeight;
                    height = keyRect.rect.height;
                    width = height * spriteRatio;
                    keyRect.sizeDelta = new Vector2(width, height);
                }
            }
        }
    }
    void OnEnable()
    {
        if (!colorOn)
        {
            if (PlayerPrefs.GetInt("SaveLevel") == 0)
            {
                textOriColor = new Color(0.8509804f, 0.7098039f, 0.2784314f, 1);
                textChangeColor = new Color(0.6509804f, 0.4156863f, 0.007843138f, 1);
                imageOriColor = new Color(0.7764706f, 0.6470588f, 0.2156863f, 1);
                imageChangeColor = new Color(0.6509804f, 0.4156863f, 0.007843138f, 1);
            }
            else if (PlayerPrefs.GetInt("SaveLevel") == 1)
            {
                textOriColor = new Color(1, 1, 1, 1);
                textChangeColor = new Color(0.5529412f, 0.5529412f, 0.5529412f, 1);
                imageOriColor = new Color(1, 1, 1, 1);
                imageChangeColor = new Color(0.5529412f, 0.5529412f, 0.5529412f, 1);
                bTextOriColor = new Color(0, 0, 0, 1);
                bTextChangeColor = new Color(0, 0, 0, 1);
            }
            else if (PlayerPrefs.GetInt("SaveLevel") == 2)
            {
                textOriColor = new Color(0.6666667f, 0.6666667f, 1, 1);
                textChangeColor = new Color(0.4666667f, 0.4666667f, 1, 1);
                imageOriColor = new Color(1, 1, 1, 1);
                imageChangeColor = new Color(0.6509804f, 0.4156863f, 0.007843138f, 1);
            }
        }
        colorOn = true;
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
            if (text.color == textChangeColor)
            {
                text.color = textOriColor;
            }
            // else if (text.color == Color.white || text.color == Color.black)
            // {
            //     text.color += textChangeColor;
            // }
        }
        foreach (Image image in buttonImages)
        {
            if (image.color == textChangeColor)
            {
                image.color = textOriColor;
                Debug.Log(image.name);
            }
            // else if (image.color == Color.white || image.color == Color.black)
            // {
            //     image.color += textChangeColor;
            // }
        }
    }
    public void PointerUp()
    {
        buttonTexts = GetComponentsInChildren<TMP_Text>(false);
        buttonImages = GetComponentsInChildren<Image>(false);
        foreach (TMP_Text text in buttonTexts)
        {
            text.color = textChangeColor;
        }
        foreach (Image image in buttonImages)
        {
            image.color = textChangeColor;
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
                UIManager.instance.mode = FullScreenMode.FullScreenWindow;
                return;
            }
            if (selected.text == collections[1])
            {
                UIManager.instance.mode = FullScreenMode.ExclusiveFullScreen;
                return;
            }
            if (selected.text == collections[2])
            {
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
            if (Input.anyKeyDown && IsExceptionKey() && !once)
            {
                once = true;
                Debug.Log(1);
                keyPressed = GetKeyPressed().ToString();
                Debug.Log(keyPressed);
                Sprite[] sprites = Resources.LoadAll<Sprite>("Images");
                foreach (Sprite sprite in sprites)
                {
                    if (sprite.name == keyPressed)
                    {
                        List<Button> keyPads = UIManager.instance.keyPads;
                        for (int i = 0; i < keyPads.Count; ++i)
                        {
                            if (keyPads[i].transform.Find("Key").GetComponent<Image>().sprite == sprite)
                            {
                                keyPads[i].transform.Find("Key").GetComponent<Image>().sprite = image.sprite;
                                Sprite keySprite = keyPads[i].transform.Find("Key").GetComponent<Image>().sprite;
                                keyRect = keyPads[i].transform.Find("Key").GetComponent<RectTransform>();
                                spriteWidth = keySprite.bounds.size.x;
                                spriteHeight = keySprite.bounds.size.y;
                                spriteRatio = spriteWidth / spriteHeight;
                                height = keyRect.rect.height;
                                width = height * spriteRatio;
                                keyRect.sizeDelta = new Vector2(width, height);
                                PlayerController.instance.keys[i] = image.sprite.name;
                                Debug.Log("changekey");
                                UIManager.instance.eventButton = button;
                                break;
                            }
                        }
                        keyRect = transform.Find("Key").GetComponent<RectTransform>();
                        image.sprite = sprite;
                        spriteWidth = sprite.bounds.size.x;
                        spriteHeight = sprite.bounds.size.y;
                        spriteRatio = spriteWidth / spriteHeight;
                        height = keyRect.rect.height;
                        width = height * spriteRatio;
                        keyRect.sizeDelta = new Vector2(width, height);
                        image.sprite = sprite;
                        PlayerController.instance.keys[UIManager.instance.count - 12] = keyPressed;
                        break;
                    }
                }
                keyPanel.SetActive(false);
                StartCoroutine(ChangeWaiting());
            }
        }
    }

    IEnumerator ChangeWaiting()
    {
        float c = 0;
        while (c < 1f)
        {
            c += Time.unscaledDeltaTime;
            yield return null;
        }
        once = false;
        changeKey = false;
    }
    bool IsExceptionKey()
    {

        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {

            if (Input.GetKeyDown(keyCode))
            {
                KeyCode pressedKey = keyCode;
                Debug.Log(pressedKey);
                foreach (KeyCode exceptionKey in UIManager.instance.exceptionKeys)
                {
                    if (pressedKey == exceptionKey)
                    {
                        Debug.Log(true);
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public void ChangeKey()
    {
        keyPanel.SetActive(true);
        Debug.Log("on");
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
