using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CrossbowEnemy : Creature
{
    public static CrossbowEnemy Instance;
    [Tooltip("공격 거리")]
    [SerializeField] internal float dangerRange = 2.0f;
    [SerializeField] internal GameObject arrow;
    [SerializeField] internal Transform player;
    [SerializeField] internal Transform enemyHpBar;
    [SerializeField] internal Transform shootPos;
    [SerializeField] internal float wallDistance = 0.5f;
    [SerializeField] internal float avoidSpeed = 5.0f;
    [SerializeField] internal bool isAvoiding;
    [SerializeField] internal bool isDamaged;
    [SerializeField] internal bool isMoveOpposite;
    [SerializeField] bool isWall;
    [SerializeField] internal float avoidingTime = 1.0f;
    [SerializeField] internal int LR;
    [SerializeField] internal bool isSpawned;
    [SerializeField] internal bool isDoing;
    Canvas canvas;
    Rigidbody2D EnemyRB;
    RaycastHit2D[] hit;
    void Start()
    {
        speed = 2.0f;
        // range = 7.0f;
        // time = 1.0f;
        // delayTime = 1.0f;
        // action = 5.0f;
        attackDamage = 2.5f;
        // dangerRange = 2.0f;
        player = GameObject.Find("Player").transform;
        wallDistance = 1.0f;
        EnemyRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Instance = this;
        enemyHpBar = transform.parent.GetChild(0).GetChild(0);
        canvas = transform.parent.GetChild(0).GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon") && other.name.Contains("Z"))
        {
            hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
            LR = transform.position.x > other.transform.parent.position.x ? 1 : -1;
            Damaged();
        }
        else if (other.CompareTag("PlayerWeapon") && other.name.Contains("h"))
        {
            PlayerController.instance.reduceDamage = PlayerController.instance.reduce;
        }
    }
    IEnumerator OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackSight") && !isDamaged && !GameManager.instance.pause && !isDoing)
        {
            isDoing = true;
            // 공격범위에 들어옴;
            while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) > range && !isAttack && !isAvoiding)
            {
                // Debug.Log("run");
                float LR = ((other.transform.position.x > transform.position.x) ? -1f : 1f);
                transform.localScale = new Vector2(LR, 1f);
                anim.SetBool("isAttack", false);
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(other.transform.position.x, transform.position.y), speed * Time.deltaTime);
                yield return null;
            }
            StartCoroutine(Attack(other));
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon") && other.name == "Shield")
        {
            PlayerController.instance.reduceDamage = 0;
        }
    }
    void Damaged()
    {
        anim.SetTrigger("isDamaged");
        EnemyRB.AddForce(new Vector2(LR * 4, 0), ForceMode2D.Impulse);
        isDamaged = false;
    }
    IEnumerator Attack(Collider2D other)
    {
        isDoing = false;
        while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack && !isWall)
        {
            Debug.Log(isAttack);
            yield return new WaitUntil(() => !GameManager.instance.pause);
            float LR = ((other.transform.position.x > transform.position.x) ? -1f : 1f);
            // 화살 쏘는 애는 너무 플레이어와 너무 가까우면 거리두기기
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= dangerRange && !isAvoiding && !isWall)
            {
                yield return new WaitUntil(() => !GameManager.instance.pause);
                anim.CrossFade("Crossbow_Run", 0f);
                transform.localScale = new Vector2(-LR, 1f);
                isAvoiding = true;
                float pl = (transform.position.x > other.transform.position.x) ? action : -action;
                StartCoroutine(Avoidance(new Vector2(transform.position.x + pl, transform.position.y), other));
                yield return new WaitForSeconds(avoidingTime);
            }
            // 공격
            else if (!isAvoiding && !isWall)
            {
                // Debug.Log("attack");
                isAttack = true;
                isAvoiding = true;
                anim.SetBool("isAttack", true);
                //공격하고 다시 false로 바뀜
                transform.localScale = new Vector2(LR, 1f);
                yield return new WaitForSeconds(delayTime);
                GameObject arrowClone = Instantiate(arrow, shootPos.position, Quaternion.identity);
                arrowClone.transform.parent = transform.parent;
                EnemyProjectile enemyProjectile = arrowClone.GetComponent<EnemyProjectile>();
                enemyProjectile.LR = LR;
                yield return new WaitForSeconds(time);
                isAttack = false;
                isAvoiding = false;
            }
            else
            {
                yield return null;
            }
        }
    }
    void Update()
    {
        EnemyHp(enemyHpBar);
    }
    void FixedUpdate()
    {
        CheckWall();
    }
    [SerializeField] LayerMask mask;
    void CheckWall()
    {
        hit = Physics2D.RaycastAll(transform.position, Vector2.left * transform.localScale.x, wallDistance);
        for (int i = 0; i < hit.Length; ++i)
        {
            // Debug.Log(hit[i].collider.tag);
            // Debug.Log(isWall);
            // Debug.Log(hit[i].collider.tag == "Wall" && !isWall);
            if (hit[i].collider.tag == "Wall" && !isWall)
            {
                if (!RomeBoss.instance.isSummoning)
                {
                    isWall = true;
                    StopAllCoroutines();
                    StartCoroutine(oppositeTheWall());
                }
            }
        }
    }
    IEnumerator oppositeTheWall()
    {
        anim.CrossFade("Crossbow_Run", 0f);
        transform.localScale = new Vector2(-LR, 1);
        float arrivePos = transform.position.x + transform.localScale.x * -action;
        while (Mathf.Abs(transform.position.x - arrivePos) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(arrivePos, transform.position.y), avoidSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }
        isWall = false;
        isAvoiding = false;
    }
    void EnemyHp(Transform _enemyHpBar)
    {
        _enemyHpBar.position = new Vector2(transform.position.x, transform.position.y + transform.localScale.y / 2 + 0.5f);
    }
    IEnumerator Avoidance(Vector2 arrivePos, Collider2D other)
    {
        anim.SetBool("isAttack", false);
        anim.CrossFade("Crossbow_Run", 0f);
        // Debug.Log("Avoidance");
        LR = other.transform.position.x > transform.position.x ? 1 : -1;

        // arrivePos에 도착할때까지 이동
        transform.localScale = new Vector2(LR, 1f);
        while (Mathf.Abs(transform.position.x - arrivePos.x) > 0.1f && !isWall)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(arrivePos.x, transform.position.y), avoidSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }
        transform.localScale = new Vector2(-LR, 1f);
        yield return new WaitForSeconds(avoidingTime);
        anim.SetBool("isAttack", true);
        if (!isWall)
        {
            isAvoiding = false;
        }
    }
}
