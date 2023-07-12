using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresentBoss : MonoBehaviour
{
    public int i;
    public static PresentBoss instance;
    public enum eSkills { 맵회전, 아가리포, 미사일, 폭격 };
    public eSkills skills;
    [Tooltip("체력 바")]
    [SerializeField] internal Slider hpbar;
    [SerializeField] internal float damage;
    [SerializeField] internal Animator anim;
    [SerializeField] internal Transform player;

    [Tooltip("이동속도")]
    [SerializeField] internal float speed = 5.0f;
    [Tooltip("공격 거리")]
    [SerializeField] internal float range = 2.0f;
    [Tooltip("공격 데미지")]
    [SerializeField] internal float attackDamage = 2.5f;
    [SerializeField] internal bool skillEnd;
    [Tooltip("공격 하는 중인지")]
    [SerializeField] internal bool isAttack;
    [SerializeField] internal bool enterance;
    [SerializeField] internal float dis;
    [SerializeField] internal float rollSpeed = 1f;
    [SerializeField] internal float rollTime = 1f;
    [SerializeField] internal float headUDTime = 1f;
    [SerializeField] internal float missiTime = 1f;
    [SerializeField] internal float mLaunchTime = 1f;
    [SerializeField] internal Transform[] shotPsoes;
    [SerializeField] internal GameObject aim120b;
    [SerializeField] internal Transform mouth;
    [SerializeField] internal Transform source;
    [SerializeField] internal GameObject agari;
    [SerializeField] internal GameObject wave;
    [SerializeField] internal int LR;
    [SerializeField] Vector3 bossAppear = new Vector2(23, -1.99f);
    [SerializeField] internal float launchAngle;
    [SerializeField] internal float launchSpeed;
    [SerializeField] internal bool onGround;
    [SerializeField] internal Vector3 agariAriv;
    Rigidbody2D bossRB;
    CapsuleCollider2D bossCC;
    void Start()
    {
        bossCC = GetComponent<CapsuleCollider2D>();
        bossRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
        if (other.CompareTag("PlayerWeapon") && other.name.Contains("Z"))
        {
            hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BottomGround"))
        {
            onGround = true;
        }
        // Debug.Log(other.gameObject.tag);
        // Debug.Log(other.gameObject.layer);
    }
    IEnumerator UseSkills()
    {
        while (hpbar.value > 0)
        {
            if (!PlayerController.instance.isCinematic)
            {
                LR = transform.position.x > player.position.x ? 1 : -1;
                skillEnd = false;
                anim.SetBool("end", false);
                // 확률도 조정해야함
                switch (Random.Range(i, i))
                {
                    case 0:
                        Debug.Log(0);
                        StartCoroutine(RollMap());
                        break;
                    case 1:
                        Debug.Log(1);
                        StartCoroutine(Agaripo());
                        break;
                    case 2:
                        Debug.Log(2);
                        StartCoroutine(AIM120B());
                        break;
                    case 3:
                        Debug.Log(3);
                        StartCoroutine(SoaringSlam());
                        break;
                }
                Debug.Log("attacked");
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
        // Physics2D.IgnoreLayerCollision(15, 8, true);
        yield return new WaitForSeconds(0.2f);
        // while (transform.position != bossAppear)
        // {
        //     transform.position = Vector2.MoveTowards(transform.position, bossAppear, 0.05f);
        //     yield return null;
        // }
        StartCoroutine(UseSkills());
    }
    [SerializeField] internal LayerMask wgMask;
    IEnumerator RollMap()
    {
        skills = eSkills.맵회전;
        int layerNumber = LayerMask.NameToLayer("EnemyWeapon");
        gameObject.layer = layerNumber;
        gameObject.tag = "EnemyWeapon";
        bossRB.bodyType = RigidbodyType2D.Static;
        bossCC.isTrigger = true;
        int LR = transform.position.x > player.position.x ? 1 : -1;
        transform.localScale = new Vector2(LR, 1);
        anim.SetBool("Roll", true);
        bossCC.size = new Vector2(0, 0);
        yield return new WaitForSeconds(rollTime);
        anim.SetBool("Rolled", true);
        // yield return new WaitForSeconds(0.01f);
        anim.SetBool("Spin", true);
        RaycastHit2D hit;
        Vector2 curPos = transform.position;
        hit = Physics2D.Raycast(transform.position, Vector2.left * LR, dis, wgMask);
        while (!hit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.left * LR, dis, wgMask);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left * LR, rollSpeed * Time.deltaTime);
            yield return null;
        }
        hit = Physics2D.Raycast(transform.position, Vector2.up, dis, wgMask);
        while (!hit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.up, dis, wgMask);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.up, rollSpeed * Time.deltaTime);
            yield return null;
        }
        hit = Physics2D.Raycast(transform.position, Vector2.left * -LR, dis, wgMask);
        while (!hit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.left * -LR, dis, wgMask);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left * -LR, rollSpeed * Time.deltaTime);
            yield return null;
        }
        hit = Physics2D.Raycast(transform.position, Vector2.down, dis, wgMask);
        while (!hit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.down, dis, wgMask);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.down, rollSpeed * Time.deltaTime);
            yield return null;
        }
        while (transform.position.x != curPos.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, curPos, rollSpeed * Time.deltaTime);
            yield return null;
        }
        anim.SetBool("Spin", false);
        anim.SetBool("Rolled", false);
        anim.SetBool("Roll", false);
        bossCC.isTrigger = false;
        bossRB.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(rollTime);
        anim.SetBool("end", true);
        layerNumber = LayerMask.NameToLayer("Enemy");
        gameObject.layer = layerNumber;
        gameObject.tag = "Enemy";
        yield return new WaitForSeconds(1f);
        skillEnd = true;
    }
    IEnumerator Agaripo()
    {
        skills = eSkills.아가리포;
        anim.SetBool("HeadUp", true);
        yield return new WaitForSeconds(headUDTime);
        Vector3 playerPos = player.position;
        GameObject agaripo = Instantiate(agari, mouth.position, Quaternion.identity);
        agariAriv = (playerPos - agaripo.transform.position).normalized;
        // yield return new WaitForSeconds(1f);
        anim.SetBool("HeadUp", false);
        yield return new WaitForSeconds(headUDTime);
        // yield return new WaitForSeconds(headUDTime);
        anim.SetBool("end", true);
        PresentBoss.instance.skillEnd = true;
    }
    IEnumerator AIM120B()
    {
        skills = eSkills.미사일;
        anim.SetBool("Open", true);
        yield return new WaitForSeconds(missiTime);
        foreach (Transform shotPos in shotPsoes)
        {
            GameObject aim = Instantiate(aim120b, shotPos.position, Quaternion.identity);
            aim.GetComponent<AIM120B>().LR = player.transform.position.x > transform.position.x ? -1 : 1;
            aim.GetComponent<AIM120B>().targetPos = shotPos;
            yield return new WaitForSeconds(mLaunchTime);
        }
        anim.SetBool("Open", false);
        yield return new WaitForSeconds(missiTime * 3);
        anim.SetBool("end", true);
        skillEnd = true;
    }
    IEnumerator SoaringSlam()
    {
        skills = eSkills.폭격;
        anim.SetBool("Roll", true);
        yield return new WaitForSeconds(rollTime);
        anim.SetBool("Rolled", true);
        anim.SetBool("Spin", true);
        onGround = false;
        float radianAngle = ((LR * launchAngle) + 90) * Mathf.Deg2Rad;
        Vector2 launchVelocity = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)) * launchSpeed;
        bossRB.velocity = launchVelocity;
        while (!onGround)
        {
            yield return null;
        }
        GameObject shockWave = Instantiate(wave, source.position, Quaternion.identity);
        anim.SetBool("Spin", false);
        anim.SetBool("Rolled", false);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("Roll", false);
        yield return new WaitForSeconds(rollTime);
        anim.SetBool("end", true);
        skillEnd = true;
    }
}