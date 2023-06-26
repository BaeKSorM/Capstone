using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresentBoss : MonoBehaviour
{
    public static PresentBoss instance;
    public enum eSkills { 몹소환, 창찌르기, 회복, 돌진 };
    public eSkills skills;
    [Tooltip("체력 바")]
    [SerializeField] internal Slider hpbar;
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
    [SerializeField] internal Transform mouth;
    [SerializeField] internal GameObject agari;
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
        if (other.CompareTag("PlayerWeapon") && other.name.Contains("Z"))
        {
            hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
        }
        if (other.CompareTag("Player"))
        {
            if (other.transform.Find("Shield").gameObject.activeSelf)
            {
                if (Mathf.Abs(transform.position.x - other.transform.position.x) > Mathf.Abs(transform.position.x - other.transform.Find("Shield").position.x) ? true : false)
                {
                    PlayerController.instance.Reduce();
                }
            }
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
                switch (Random.Range(1, 1))
                {
                    case 0:
                        StartCoroutine(RollMap());
                        break;
                    case 1:
                        StartCoroutine(Agaripo());
                        break;
                    case 2:
                        break;
                    case 3:
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
        bossRB.bodyType = RigidbodyType2D.Kinematic;
        int LR = transform.position.x > player.position.x ? 1 : -1;
        transform.localScale = new Vector2(LR, 1);
        RaycastHit2D hit;
        Vector2 curPos = transform.position;
        hit = Physics2D.Raycast(transform.position, Vector2.left * LR, dis, wgMask);
        while (!hit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.left * LR, dis, wgMask);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left * LR, 0.01f);
            yield return null;
        }
        Debug.Log(Vector2.up);
        hit = Physics2D.Raycast(transform.position, Vector2.up, dis, wgMask);
        while (!hit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.up, dis, wgMask);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.up, 0.01f);
            yield return null;
        }
        Debug.Log(Vector2.up);
        hit = Physics2D.Raycast(transform.position, Vector2.left * -LR, dis, wgMask);
        while (!hit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.left * -LR, dis, wgMask);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left * -LR, 0.01f);
            yield return null;
            Debug.Log(Vector2.up);
        }
        hit = Physics2D.Raycast(transform.position, Vector2.down, dis, wgMask);
        while (!hit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.down, dis, wgMask);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.down, 0.01f);
            yield return null;
        }
        while (transform.position.x != curPos.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, curPos, 0.01f);
            yield return null;
        }
        bossRB.bodyType = RigidbodyType2D.Dynamic;
        skillEnd = true;
    }
    IEnumerator Agaripo()
    {
        Vector3 playerPos = player.position;
        GameObject agaripo = Instantiate(agari, mouth.position, Quaternion.identity);
        Vector3 direction = (playerPos - agaripo.transform.position).normalized;
        while (agaripo != null)
        {
            agaripo.transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }
    }
}
