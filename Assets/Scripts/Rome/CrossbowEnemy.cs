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
    [SerializeField] internal Transform enemyHpBar;
    [SerializeField] internal float wallDistance = 0.5f;
    [SerializeField] internal float avoidSpeed = 5.0f;
    [SerializeField] internal bool isAvoidingAttack;
    [SerializeField] internal bool isAvoiding;
    [SerializeField] internal bool isDamaged;
    [SerializeField] bool isWall;
    [SerializeField] internal float avoidingTime = 1.0f;

    Canvas canvas;
    Rigidbody2D rb;
    RaycastHit2D[] hit;
    void Start()
    {
        speed = 5.0f;
        // range = 7.0f;
        time = 1.0f;
        delayTime = 1.0f;
        action = 5.0f;
        attackDamage = 2.5f;
        dangerRange = 2.0f;
        wallDistance = 1.0f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Instance = this;
        enemyHpBar = transform.parent.GetChild(0).GetChild(0);
        canvas = transform.parent.GetChild(0).GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
            Damaged();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackSight") && !isWall && !isDamaged)
        {
            // 공격범위에 들어옴;
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) > range && !isAttack && !isAvoiding)
            {
                // Debug.Log("run");
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(other.transform.position.x, transform.position.y), speed * Time.deltaTime);
            }
            StartCoroutine(Attack(other));
        }
    }
    void Damaged()
    {
        anim.SetBool("isDamaged", true);
        Debug.Log(rb.velocity.y);
        rb.AddForce(new Vector2(transform.localScale.x * 4, 2), ForceMode2D.Impulse);
        Debug.Log(rb.velocity.y);
        isDamaged = false;
    }
    IEnumerator Attack(Collider2D other)
    {
        while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
        {
            float LR = ((other.transform.position.x > transform.position.x) ? -0.5f : 0.5f);
            // 화살 쏘는 애는 너무 플레이어와 너무 가까우면 거리두기기
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= dangerRange && !isAvoidingAttack && !isAvoiding && !isWall)
            {
                transform.localScale = new Vector2(-LR, 0.5f);
                isAvoiding = true;
                float pl = (transform.position.x > other.transform.position.x) ? action : -action;
                StartCoroutine(Avoidance(new Vector2(transform.position.x + pl, transform.position.y), other));
                yield return new WaitForSeconds(avoidingTime);
            }
            // 공격
            else if (!isAvoiding)
            {
                //공격하고 다시 false로 바뀜
                transform.localScale = new Vector2(LR, 0.5f);
                GameObject arrowClone = Instantiate(arrow, gameObject.transform.position + new Vector3(-LR, 0, 0), Quaternion.identity);
                EnemyArrow enemyArrow = arrowClone.GetComponent<EnemyArrow>();
                enemyArrow.LR = -LR;
                isAttack = true;
                anim.SetBool("isAttack", true);
                yield return new WaitForSeconds(enemyArrow.arrowDestroyTime);
                anim.SetBool("isAttack", false);
                yield return new WaitForSeconds(time);
                isAttack = false;
                isAvoidingAttack = false;
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
        CheckWall();
    }
    void CheckWall()
    {
        hit = Physics2D.RaycastAll(transform.position, Vector2.left * transform.localScale.x, wallDistance);
        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].collider.tag == "Wall" && !isWall)
            {
                if (!RomeBoss.instance.isSpawning)
                {
                    isWall = true;
                    StartCoroutine(oppositeTheWall());
                }
            }
        }
    }
    IEnumerator oppositeTheWall()
    {
        Debug.Log("oppositeTheWall");

        float arrivePos = transform.position.x + transform.localScale.x * action;
        while (Mathf.Abs(transform.position.x - arrivePos) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(arrivePos, transform.position.y), avoidSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
            // Debug.Log(transform.position);
            // Debug.Log(new Vector2(arrivePos, transform.position.y));
            // Debug.Log(Mathf.Abs(transform.position.x - arrivePos));
            // Debug.Log((Mathf.Abs(transform.position.x - arrivePos) > 0.1f));
        }
        isWall = false;
    }
    void EnemyHp(Transform _enemyHpBar)
    {
        _enemyHpBar.position = new Vector2(transform.position.x, transform.position.y + transform.localScale.y / 2 + 0.5f);
    }
    IEnumerator Avoidance(Vector2 arrivePos, Collider2D other)
    {
        Debug.Log("Avoidance");
        float LR = ((other.transform.position.x > transform.position.x) ? 0.5f : -0.5f);

        // arrivePos에 도착할때까지 이동
        transform.localScale = new Vector2(LR, 0.5f);
        while (Mathf.Abs(transform.position.x - arrivePos.x) > 0.1f && !isWall)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(arrivePos.x, transform.position.y), avoidSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }
        isAvoiding = false;
        isAvoidingAttack = true;
    }
}
