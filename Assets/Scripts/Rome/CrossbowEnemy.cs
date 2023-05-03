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
    [SerializeField] internal GameObject[] wallPos;
    [SerializeField] internal Vector2 direction;

    [SerializeField] internal bool isAvoidingAttack;
    [SerializeField] internal bool isAvoiding;
    [SerializeField] bool isWall = true;
    [SerializeField] internal float avoidingTime = 1.0f;

    Canvas canvas;
    Rigidbody2D rb;
    RaycastHit2D[] hit;
    void Start()
    {
        speed = 5.0f;
        range = 7.0f;
        time = 1.0f;
        delayTime = 1.0f;
        action = 5.0f;
        attackDamage = 2.5f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Instance = this;
        enemyHpBar = transform.parent.GetChild(0).GetChild(0);
        canvas = transform.parent.GetChild(0).GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
    internal IEnumerator isWallOn()
    {
        yield return new WaitForSeconds(10f);
        isWall = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackSight") && !isWall)
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
    IEnumerator Attack(Collider2D other)
    {
        while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
        {
            // 화살 쏘는 애는 너무 플레이어와 너무 가까우면 거리두기기
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= dangerRange && !isAvoidingAttack && !isAvoiding && !isWall)
            {
                isAvoiding = true;
                float pl = (transform.position.x > other.transform.position.x) ? action : -action;
                StartCoroutine(Avoidance(new Vector2(transform.position.x + pl, transform.position.y)));
                yield return new WaitForSeconds(avoidingTime);
            }
            // 공격
            else if (!isAvoiding)
            {
                //공격하고 다시 false로 바뀜
                float LR = ((other.transform.position.x > transform.position.x) ? 1 : -1);
                GameObject arrowClone = Instantiate(arrow, gameObject.transform.position + new Vector3(LR, 0, 0), Quaternion.identity);
                EnemyArrow enemyArrow = arrowClone.GetComponent<EnemyArrow>();
                enemyArrow.LR = LR;
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
        hit = Physics2D.RaycastAll(transform.position, -new Vector2(1, 0), wallDistance);
        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].collider.tag == "Wall" && !isWall)
            {
                isWall = true;
                StartCoroutine(oppositeTheWall());
            }
            Debug.Log(hit[i].collider.tag);
        }
    }
    IEnumerator oppositeTheWall()
    {
        // Debug.Log("oppositeTheWall");
        float arrivePos = transform.position.x + action;
        while (Mathf.Abs(transform.position.x - arrivePos) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + action, transform.position.y), 0.01f);
            yield return new WaitForSeconds(0.001f);
        }
        isWall = false;
    }
    void EnemyHp(Transform _enemyHpBar)
    {
        _enemyHpBar.position = new Vector2(transform.position.x, transform.position.y + transform.localScale.y / 2 + 0.5f);
    }
    IEnumerator Avoidance(Vector2 arrivePos)
    {
        // Debug.Log("Avoidance");
        // arrivePos에 도착할때까지 이동
        while (Mathf.Abs(transform.position.x - arrivePos.x) > 0.1f && !isWall)
        {
            transform.position = Vector2.MoveTowards(transform.position, arrivePos, 0.01f);
            yield return new WaitForSeconds(0.001f);
        }
        isAvoiding = false;
        isAvoidingAttack = true;
    }
}
