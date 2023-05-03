using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("스테이지")]
    [Tooltip("스테이지 이름")]
    [SerializeField] internal List<string> stages;
    [Tooltip("현재 스테이지")]
    [SerializeField] internal int saveStageLevel;
    [Tooltip("아이템 드랍할 적 최대수")]
    [SerializeField] internal int dropEnemiesMaxCount;
    [Tooltip("적들넣어주기")]
    [SerializeField] internal List<GameObject> enemies;
    // [Tooltip("드랍된 아이템에 닿였는지")]
    // [SerializeField] internal List<bool> isTouching;
    [Tooltip("전에 밟은 아이템")]
    [SerializeField] internal int beforeSteped;
    [Tooltip("적이 떨어뜨린 무기들")]
    [SerializeField] internal Transform enemiesDropedWeapons;
    [Tooltip("아이템 떨어뜨리는 적들 죽은 순서")]
    [SerializeField] internal int dropedDeadCount;
    [Tooltip("죽은 적 수")]
    [SerializeField] internal int deadCount;

    [SerializeField] internal enum eAge { 로마, 현대, 미래 };
    [SerializeField] internal eAge age;


    [Tooltip("보스 등장했는지")]
    [SerializeField] internal bool bossAppear;
    private void Awake()
    {
        instance = this;
        // PlayerPrefs.SetInt("SaveLevel", 0);
    }
    void Start()
    {
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
    public void stageStart(int _stageLevel)
    {
        saveStageLevel = _stageLevel;
        SceneManager.LoadScene(stages[_stageLevel]);
    }
    public void GameClear()
    {
        PlayerPrefs.SetInt("SaveLevel", saveStageLevel + 1);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        StartCoroutine(UIManager.instance.loading());
#endif
    }
}
