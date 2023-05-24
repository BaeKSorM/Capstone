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
    [SerializeField] internal Vector3 summonPos;
    [SerializeField] internal float exhaustionHp;
    [SerializeField] internal Transform player;

    [Tooltip("이동속도")]
    [SerializeField] internal float speed = 5.0f;
    [Tooltip("공격 거리")]
    [SerializeField] internal float range = 2.0f;

    [Tooltip("공격 시간")]
    [SerializeField] internal float time = 1.0f;

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
    [SerializeField] internal bool isSpawning;
    [SerializeField] internal int summonEnemiesCount = 5;
    [SerializeField] internal int maxSpawnMobsCount = 5;
    [SerializeField] Vector3 bossAppear = new Vector2(23, -1.99f);
    [SerializeField] internal CameraManager cameraManager;
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
                int LR = transform.position.x > player.position.x ? 1 : -1;
                skillEnd = false;
                // 확률도 조정해야함
                switch (Random.Range(0, 4))
                {
                    case 0:
                        if (spawnedMobs.transform.childCount < maxSpawnMobsCount)
                        {
                            StartCoroutine(SpawnMobs());
                        }
                        Debug.Log(0);
                        break;
                    case 1:
                        StartCoroutine(SpearPoking(LR));
                        Debug.Log(1);
                        break;
                    case 2:
                        if (hpbar.value <= 80)
                        {
                            StartCoroutine(Healing());
                        }
                        else
                        {
                            skillEnd = true;
                        }
                        Debug.Log(2);
                        break;
                    case 3:
                        StartCoroutine(Crushing(LR));
                        Debug.Log(3);
                        break;
                }
                yield return new WaitUntil(() => skillEnd);
            }
            yield return null;
        }
    }
    internal IEnumerator BossAppear()
    {
        Physics2D.IgnoreLayerCollision(15, 8, true);
        yield return new WaitForSeconds(0.2f);
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
        isSpawning = true;
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < summonEnemiesCount; ++i)
        {
            int enemy = Random.Range(0, summonEnemies.Length);
            GameObject spawnedMob = Instantiate(summonEnemies[enemy], summonPos, Quaternion.identity);
            StartCoroutine(SpawnMobMove(spawnedMob));
            spawnedMob.transform.parent = spawnedMobs.transform;
            yield return new WaitForSeconds(0.5f);
            Debug.Log(i);
        }

    }
    IEnumerator SpawnMobMove(GameObject notInGameMob)
    {
        while (notInGameMob.transform.position != new Vector3(cameraManager.bossGroundCenter.x, notInGameMob.transform.position.y))
        {
            // Debug.Log("mobmove");
            notInGameMob.transform.position = Vector2.MoveTowards(notInGameMob.transform.position, cameraManager.bossGroundCenter, 0.01f);
            // yield return new WaitForSeconds(0.5f);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        Debug.Log("end");
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("isSpawning", false);
        isSpawning = false;
        skillEnd = true;
    }
    IEnumerator SpearPoking(int LR)
    {
        transform.localScale = new Vector2(LR, 1);
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
        yield return new WaitForSeconds(1.0f);
        skillEnd = true;
        isAttack = false;
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
            if (curTime > healingTime || hpbar.value < exhaustionHp || hpbar.value >= 100)
            {
                anim.SetBool("isHealing", false);
                yield return new WaitForSeconds(1.0f);
                break;
            }
        }
        skillEnd = true;
    }
    IEnumerator Crushing(int LR)
    {
        Vector3 curPos = transform.position;
        Vector3 arrivePos = new Vector2(cameraManager.bossGroundCenter.x - LR * 10, transform.position.y);
        // Debug.Log(curPos);
        transform.localScale = new Vector2(LR, 1);

        anim.SetBool("isCrushing", true);
        weapon.SetActive(true);
        do
        {
            transform.position = Vector2.MoveTowards(transform.position, arrivePos, 0.1f);
            // 왼쪽으로 이동할때 왼쪽벽 위치보다 왼쪽으로 가면 오른쪽으로 이동
            // 왼쪽으로 가려면 -1 오른쪽에 있으면 1
            if (cameraManager.bossGroundCenter.x + -LR * 10 == transform.position.x)
            {
                //수정
                transform.position = new Vector2(cameraManager.bossGroundCenter.x + LR * 10, transform.position.y);
                arrivePos = curPos;
            }
            yield return null;
        } while (transform.position.x != curPos.x);
        weapon.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("isCrushing", false);
        skillEnd = true;
    }
}
