using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("스테이지")]
    [Tooltip("스테이지 이름")]
    [SerializeField] internal List<string> stages;
    [Tooltip("아이템 드랍할 적 최대수")]
    [SerializeField] internal int dropEnemiesMaxCount;
    [Tooltip("적들넣어주기")]
    [SerializeField] internal List<GameObject> enemies;
    [Tooltip("전에 밟은 아이템")]
    [SerializeField] internal int beforeSteped;
    [Tooltip("적이 떨어뜨린 무기들")]
    [SerializeField] internal Transform enemiesDropedWeapons;
    [Tooltip("아이템 떨어뜨리는 적들 죽은 순서")]
    [SerializeField] internal int dropedDeadCount;
    [Tooltip("죽은 적 수")]
    [SerializeField] internal int deadCount;
    [Tooltip("보스 죽었나")]
    [SerializeField] internal bool bossDie;

    [SerializeField] internal enum eAge { 로마, 현대, 미래 };
    [SerializeField] internal eAge age;
    [SerializeField] public bool SM;

    [Tooltip("보스 등장했는지")]
    [SerializeField] internal bool bossAppear;
    [SerializeField] internal AudioSource audioSource;
    [SerializeField] internal AudioClip[] audioClips;

    private void Awake()
    {
        instance = this;
        // 첫스테이지
        //UIManager.instance.
        PlayerPrefs.SetFloat("PlayerHp", 100);
        // PlayerPrefs.SetInt("SaveLevel", saveStageLevel);
        //테스트용
        PlayerPrefs.SetInt("SaveLevel", 2);
        age = (eAge)PlayerPrefs.GetInt("SaveLevel");
        // Debug.Log(PlayerPrefs.GetInt("SaveLevel"));
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioClips = Resources.LoadAll<AudioClip>("AudioClips");
        Texture2D[] cursors = Resources.LoadAll<Texture2D>("Cursors");
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images");
        Debug.Log(sprites.Length);
        Debug.Log(cursors.Length);
        foreach (Texture2D cursor in cursors)
        {
            if (cursor.name == stages[PlayerPrefs.GetInt("SaveLevel")] + "Cursor")
            {
                Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
                Debug.Log("found");
                break;
            }
            Debug.Log(cursor.name);
        }
        if (SM)
        {
            Screen.SetResolution(PlayerPrefs.GetInt("ScreenWidth"), PlayerPrefs.GetInt("ScreenHeight"), (FullScreenMode)System.Enum.Parse(typeof(FullScreenMode), PlayerPrefs.GetString("mode"), true));
            {
                SoundManager.instance.audioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("Volume"));
                SoundManager.instance.audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("Music"));
                SoundManager.instance.audioMixer.SetFloat("SoundEffect", PlayerPrefs.GetFloat("SFX"));
            }
        }
        // PlayerController.instance.hpbar.value = PlayerPrefs.GetFloat("PlayerHp");
        for (int i = 0; i < dropEnemiesMaxCount; ++i)
        {
            int rand = Random.Range(0, enemies.Count);
            if (!enemies[rand].transform.GetComponentInChildren<DropWeapons>().isDrop)
            {
                enemies[rand].transform.GetComponentInChildren<DropWeapons>().isDrop = true;
            }
            else
            {
                --i;
            }
        }
        for (int i = 0; i < audioClips.Length; ++i)
        {
            if (audioClips[i].name == stages[PlayerPrefs.GetInt("SaveLevel")] + "Opening")
            {
                audioSource.clip = audioClips[i];
                break;
            }
        }
        Debug.Log(stages[PlayerPrefs.GetInt("SaveLevel")]);
        audioSource.Play();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SM)
        {
            UIManager.instance.Pause();
        }
    }

    internal void show_ItemInfo(int nearby_Item)
    {
        if (beforeSteped != -1 && nearby_Item != beforeSteped)
        {
            enemiesDropedWeapons.GetChild(beforeSteped).GetChild(0).gameObject.SetActive(false);
        }
        beforeSteped = nearby_Item;
        enemiesDropedWeapons.GetChild(nearby_Item).GetChild(0).gameObject.SetActive(true);
        PlayerController.instance.weaponCount = nearby_Item;
    }
    internal void close_ItemInfo(int nearby_Item)
    {
        enemiesDropedWeapons.GetChild(nearby_Item).GetChild(0).gameObject.SetActive(false);
        PlayerController.instance.isTouching = false;
    }
    /// <summary>
    /// 스테이지 불러오기
    /// </summary>
    /// <param name="_stageLevel">이동할 스테이지</param>
    public void stageStart()
    {
        SceneManager.LoadScene(stages[PlayerPrefs.GetInt("SaveLevel")]);
    }
    public void GameClear()
    {
        PlayerPrefs.SetInt("SaveLevel", PlayerPrefs.GetInt("SaveLevel") + 1);
        Debug.Log("GameClear");
#if UNITY_EDITOR
        // UnityEditor.EditorApplication.isPlaying = false;
#endif
        StartCoroutine(UIManager.instance.loading());
    }
}
