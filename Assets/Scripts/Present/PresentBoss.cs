using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresentBoss : MonoBehaviour
{
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
    Rigidbody2D bossRB;
    BoxCollider2D bossBC;
    void Start()
    {
        bossBC = GetComponent<BoxCollider2D>();
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
        if (other.CompareTag("Player"))
        {
            if (other.transform.Find("Shield") != null && other.transform.Find("Shield").gameObject.activeSelf)
            {
                if (Mathf.Abs(transform.position.x - other.transform.position.x) > Mathf.Abs(transform.position.x - other.transform.Find("Shield").position.x) ? true : false)
                {
                    PlayerController.instance.Reduce();
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BottomGround"))
        {
            onGround = true;
        }
        Debug.Log(other.gameObject.tag);
        Debug.Log(other.gameObject.layer);
    }
    IEnumerator UseSkills()
    {
        while (hpbar.value > 0)
        {
            if (!PlayerController.instance.isCinematic)
            {
                LR = transform.position.x > player.position.x ? 1 : -1;
                skillEnd = false;
                // 확률도 조정해야함
                switch (Random.Range(0, 0))
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
        bossBC.isTrigger = true;
        int LR = transform.position.x > player.position.x ? 1 : -1;
        transform.localScale = new Vector2(LR, 1);
        anim.SetBool("Roll", true);
        RaycastHit2D hit;
        Vector2 curPos = transform.position;
        hit = Physics2D.Raycast(transform.position, Vector2.left * LR, dis, wgMask);
        while (!hit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.left * LR, dis, wgMask);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left * LR, 0.1f);
            yield return null;
        }
        hit = Physics2D.Raycast(transform.position, Vector2.up, dis, wgMask);
        while (!hit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.up, dis, wgMask);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.up, 0.1f);
            yield return null;
        }
        hit = Physics2D.Raycast(transform.position, Vector2.left * -LR, dis, wgMask);
        while (!hit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.left * -LR, dis, wgMask);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left * -LR, 0.1f);
            yield return null;
        }
        hit = Physics2D.Raycast(transform.position, Vector2.down, dis, wgMask);
        while (!hit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.down, dis, wgMask);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.down, 0.1f);
            yield return null;
        }
        while (transform.position.x != curPos.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, curPos, 0.1f);
            yield return null;
        }
        layerNumber = LayerMask.NameToLayer("Enemy");
        gameObject.layer = layerNumber;
        gameObject.tag = "Enemy";
        bossRB.bodyType = RigidbodyType2D.Dynamic;
        bossBC.isTrigger = false;
        skillEnd = true;
    }
    IEnumerator Agaripo()
    {
        skills = eSkills.아가리포;
        Vector3 playerPos = player.position;
        GameObject agaripo = Instantiate(agari, mouth.position, Quaternion.identity);
        Vector3 direction = (playerPos - agaripo.transform.position).normalized;
        while (agaripo != null)
        {
            agaripo.transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator AIM120B()
    {
        skills = eSkills.미사일;
        // LR = player.transform.position.x > transform.position.x ? -1 : 1;
        // transform.localScale = new Vector2(LR, 1);
        foreach (Transform shotPos in shotPsoes)
        {
            GameObject aim = Instantiate(aim120b, shotPos.position, Quaternion.identity);
            aim.GetComponent<AIM120B>().LR = player.transform.position.x > transform.position.x ? -1 : 1;
            aim.GetComponent<AIM120B>().targetPos = shotPos;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1f);
        skillEnd = true;
    }
    IEnumerator SoaringSlam()
    {
        skills = eSkills.폭격;
        onGround = false;
        float radianAngle = ((LR * launchAngle) + 90) * Mathf.Deg2Rad;
        Vector2 launchVelocity = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)) * launchSpeed;
        bossRB.velocity = launchVelocity;
        while (!onGround)
        {
            yield return null;
        }
        GameObject shockWave = Instantiate(wave, source.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        skillEnd = true;
    }
}
