using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
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
    RectTransform keyRect;
    float spriteWidth;
    float spriteHeight;
    float spriteRatio;
    float height;
    float width;
    [SerializeField] bool once;
    void Awake()
    {
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
            text.color = new Color(0.3f, 0.5f, 0.8f, 1);
        }
        foreach (Image image in buttonImages)
        {
            image.color = new Color(0.3f, 0.5f, 0.8f, 1);
        }
    }
    public void PointerUp()
    {
        buttonTexts = GetComponentsInChildren<TMP_Text>(false);
        buttonImages = GetComponentsInChildren<Image>(false);
        foreach (TMP_Text text in buttonTexts)
        {
            text.color = new Color(1, 1, 1, 1);
        }
        foreach (Image image in buttonImages)
        {
            image.color = new Color(1, 1, 1, 1);
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
