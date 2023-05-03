using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RomeBoss : MonoBehaviour
{
    public static RomeBoss instance;
    public enum eSkills { 몹소환, 창찌르기, 회복, 돌진 };
    public eSkills skills;
    [Tooltip("체력 바")]
    [SerializeField] internal Slider hpbar;
    [SerializeField] internal Animator anim;
    [SerializeField] internal GameObject[] summonEnemies;
    [SerializeField] internal Vector3[] summonPosLR;
    [SerializeField] internal float exhaustionHp;
    [SerializeField] internal Transform player;

    [Tooltip("이동속도")]
    [SerializeField] internal float speed = 5.0f;
    [Tooltip("공격 거리")]
    [SerializeField] internal float range = 2.0f;

    [Tooltip("공격 시간")]
    [SerializeField] internal float time = 1.0f;
    [Tooltip("공격 대기 시간")]
    [SerializeField] internal float pokingDelayTime = 1.0f;

    [Tooltip("공격 데미지")]
    [SerializeField] internal float attackDamage = 2.5f;
    [SerializeField] internal bool skillEnd;
    [SerializeField] internal float healingTime;
    [SerializeField] internal float healAmountPerSecond = 1.0f;
    [SerializeField] internal float healDelayTime = 0.5f;
    [Tooltip("공격 하는 중인지")]
    [SerializeField] internal bool isAttack;
    [SerializeField] internal GameObject weapon;
    [SerializeField] internal GameObject spawnedMobs;
    [SerializeField] internal bool enterance;
    [SerializeField] internal bool enemyEnter;
    Rigidbody2D bossRB;
    void Start()
    {
        bossRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // weapon = transform.GetChild(1).gameObject;
        instance = this;
    }
    void Update()
    {
        if (GameManager.instance.bossAppear && !enterance)
        {
            StartCoroutine(BossAppear());
            enterance = true;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
        }
    }
    IEnumerator UseSkills()
    {
        while (hpbar.value > 0)
        {
            if (!PlayerController.instance.isCinematic)
            {
                skillEnd = false;
                switch (Random.Range(1, 2))
                {
                    case 0:
                        StartCoroutine(SpawnMobs());
                        Debug.Log(0);
                        break;
                    case 1:
                        StartCoroutine(SpearPoking());
                        Debug.Log(1);
                        break;
                    case 2:
                        if (hpbar.value <= 80)
                        {
                            StartCoroutine(Healing());
                        }
                        skillEnd = true;
                        Debug.Log(2);
                        break;
                    case 3:
                        StartCoroutine(Crushing());
                        Debug.Log(3);
                        break;
                }
                yield return new WaitUntil(() => skillEnd);
                yield return new WaitForSeconds(1.0f);
            }
            yield return null;
        }
    }
    internal IEnumerator BossAppear()
    {
        yield return new WaitForSeconds(0.2f);
        Vector3 bossAppear = new Vector2(23, -1.99f);
        while (transform.position != bossAppear)
        {
            transform.position = Vector2.MoveTowards(transform.position, bossAppear, 0.05f);
            yield return null;
        }
        StartCoroutine(UseSkills());
    }
    IEnumerator SpawnMobs()
    {
        anim.SetBool("isSpawning", true);
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 5; ++i)
        {
            int LR = Random.Range(0, 2);
            int enemy = Random.Range(0, 2);
            GameObject spawnedMob = Instantiate(summonEnemies[enemy], summonPosLR[LR], Quaternion.identity);

            StartCoroutine(SpawnMobMove(spawnedMob, LR));
            spawnedMob.transform.parent = spawnedMobs.transform;
        }
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("isSpawning", false);
    }
    IEnumerator SpawnMobMove(GameObject notInGameMob, int LR)
    {
        enemyEnter = true;
        float cur = 0;
        float enterTime = 1f;
        while (cur < enterTime)
        {
            cur += Time.deltaTime;
            // Debug.Log("mobmove");
            notInGameMob.transform.position = Vector2.MoveTowards(notInGameMob.transform.position, summonPosLR[LR] + ((LR == 1) ? -new Vector3(10, 0) : new Vector3(10, 0)), 0.01f);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(1.0f);
        skillEnd = true;
        enemyEnter = false;
    }
    IEnumerator SpearPoking()
    {
        while (Mathf.Abs(transform.position.x - player.position.x) > range)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), speed * Time.deltaTime);
            yield return null;
        }
        //공격하고 다시 false로 바뀜
        isAttack = true;
        weapon.SetActive(true);
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(time);
        anim.SetBool("isAttack", false);
        weapon.SetActive(false);
        yield return new WaitForSeconds(pokingDelayTime);
        isAttack = false;
        skillEnd = true;
    }
    IEnumerator Healing()
    {
        exhaustionHp = hpbar.value - 10;
        anim.SetBool("isHealing", true);
        float curTime = 0;
        while (hpbar.value > exhaustionHp)
        {
            curTime += healDelayTime;
            hpbar.value += healAmountPerSecond;
            yield return new WaitForSeconds(healDelayTime);
            if (curTime > healingTime || hpbar.value < exhaustionHp)
            {
                break;
            }
        }
        anim.SetBool("isHealing", false);
        yield return new WaitForSeconds(1.0f);
        skillEnd = true;
    }
    IEnumerator Crushing()
    {
        transform.localScale = transform.position.x > player.position.x ? new Vector2(1, 1) : new Vector2(-1, 1);
        anim.SetBool("crush", true);
        yield return null;
    }
}
