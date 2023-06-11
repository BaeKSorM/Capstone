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
    [SerializeField] internal bool isSummoning;
    [SerializeField] internal int summonEnemiesCount = 5;
    [SerializeField] internal int maxSpawnMobsCount = 5;
    [SerializeField] internal int arriveSpawnedMobs;
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
                switch (Random.Range(0, 0))
                {
                    case 0:
                        if (spawnedMobs.transform.childCount < maxSpawnMobsCount)
                        {
                            StartCoroutine(SpawnMobs());
                        }
                        else
                        {
                            skillEnd = true;
                        }
                        Debug.Log(1);
                        break;
                    case 1:
                        StartCoroutine(SpearPoking(LR));
                        // Debug.Log(2);
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
                        // Debug.Log(3);
                        break;
                    case 3:
                        StartCoroutine(Crushing(LR));
                        // Debug.Log(4);
                        break;
                }
                yield return new WaitUntil(() => skillEnd);
            }
            yield return null;
        }
        GameManager.instance.bossDie = true;
        anim.SetTrigger("isDead");
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
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
        anim.SetBool("isSummoning", true);
        isSummoning = true;
        yield return new WaitForSeconds(0.416f);
        arriveSpawnedMobs = 0;
        for (int i = 0; i < summonEnemiesCount; ++i)
        {
            int enemy = Random.Range(0, summonEnemies.Length);
            GameObject spawnedMob = Instantiate(summonEnemies[enemy], summonPos, Quaternion.identity);
            spawnedMob.transform.parent = spawnedMobs.transform;
            StartCoroutine(SpawnMobMove(spawnedMob));
            yield return new WaitForSeconds(0.5f);
        }
        while (arriveSpawnedMobs != summonEnemiesCount)
        {
            yield return null;
        }
        anim.SetBool("isSummoning", false);
        isSummoning = false;
        skillEnd = true;
    }
    IEnumerator SpawnMobMove(GameObject notInGameMob)
    {
        float spawnTime = 0;
        while (notInGameMob.transform.position != new Vector3(cameraManager.bossGroundCenter.x + 5, notInGameMob.transform.position.y))
        {
            Debug.Log("mobmove");
            notInGameMob.transform.position = Vector2.MoveTowards(notInGameMob.transform.position, cameraManager.bossDoorFornt, 0.01f);
            // yield return new WaitForSeconds(0.5f);
            yield return new WaitForSeconds(0.01f);
            spawnTime += 0.02f;
            if (spawnTime >= 0.9)
            {
                break;
            }
        }
        ++arriveSpawnedMobs;
        // Debug.Log("end");
    }
    IEnumerator SpearPoking(int LR)
    {
        transform.localScale = new Vector2(LR, 1);
        anim.SetBool("isRunning", true);
        while (Mathf.Abs(transform.position.x - player.position.x) > range)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), speed * Time.deltaTime);
            yield return null;
        }
        anim.SetBool("isRunning", false);
        //공격하고 다시 false로 바뀜
        isAttack = true;
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(1.85f);
        Debug.Log(anim.GetCurrentAnimatorClipInfo(0).Length);
        anim.SetBool("isAttack", false);
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
    public Vector3 arrivePos;
    IEnumerator Crushing(int LR)
    {
        Vector3 curPos = transform.position;
        arrivePos = new Vector2(cameraManager.bossGroundCenter.x - LR * 10, transform.position.y);
        // Debug.Log(curPos);
        transform.localScale = new Vector2(LR, 1);
        weapon.SetActive(true);
        anim.SetBool("isCrushing", true);
        do
        {
            transform.position = Vector2.MoveTowards(transform.position, arrivePos, 0.02f);
            // 왼쪽으로 이동할때 왼쪽벽 위치보다 왼쪽으로 가면 오른쪽으로 이동
            // 왼쪽으로 가려면 -1 오른쪽에 있으면 1
            if (cameraManager.bossGroundCenter.x + -LR * 10 == transform.position.x)
            {
                //수정
                // Debug.Log("fixed");
                transform.position = new Vector2(cameraManager.bossGroundCenter.x + LR * 10, transform.position.y);
                arrivePos = curPos;
            }
            yield return null;
        } while (transform.position.x != curPos.x);
        anim.SetBool("isCrushing", false);
        weapon.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        skillEnd = true;
    }
}
