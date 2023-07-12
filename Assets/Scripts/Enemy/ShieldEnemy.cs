using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldEnemy : Creature
{
    public static ShieldEnemy Instance;
    [SerializeField] internal GameObject weapon;
    [Tooltip("돌진 대기중")]
    [SerializeField] internal bool holding;
    [Tooltip("막는 중인지")]
    [SerializeField] internal bool isDefending;
    [Tooltip("막는 중일때 방패에 닿였는지")]
    [SerializeField] internal bool defend;
    [SerializeField] internal Transform enemyHpBar;
    [SerializeField] internal bool damageOn;
    [SerializeField] internal bool isDamaged;
    [SerializeField] internal bool isDoing;
    [SerializeField] internal int LR;
    [SerializeField] GameObject shield;
    Rigidbody2D EnemyRB;
    Canvas canvas;
    void Start()
    {
        shield = GameObject.Find("Shield");
        // speed = 5.0f;
        range = 3.0f;
        // time = 1.0f;
        delayTime = 1.0f;
        action = 5.0f;
        attackDamage = 2.5f;
        anim = GetComponent<Animator>();
        EnemyRB = GetComponent<Rigidbody2D>();
        enemyHpBar = transform.parent.GetChild(0).GetChild(0);
        canvas = transform.parent.GetChild(0).GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        Instance = this;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon") && other.name.Contains("Z"))
        {
            // Debug.Log(other.transform.parent.name);
            if (transform.localScale.x == other.transform.parent.localScale.x)
            {
                defend = true;
            }
            else
            {
                defend = false;
            }
            if (!isDefending)
            {
                LR = transform.position.x > other.transform.parent.position.x ? 1 : -1;
                Damaged();
                hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
            }
            else if (isDefending)
            {
                Debug.Log("Def");
                if (!defend)
                {
                    Debug.Log("dam");
                    LR = transform.position.x > other.transform.parent.position.x ? 1 : -1;
                    Damaged();
                    hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
                }
                else if (defend)
                {
                    Debug.Log("shie");
                }
            }
        }
        if (other.CompareTag("Player"))
        {
            attackDamage = damage;
            if (other.CompareTag("Player"))
            {
                if (Vector2.Distance(shield.transform.position, transform.position) < Vector2.Distance(other.transform.position, transform.position))
                {
                    attackDamage = saveDamage;
                    attackDamage -= attackDamage - PlayerController.instance.reduce > 0 ? PlayerController.instance.reduce : attackDamage;
                }
                else if (Vector2.Distance(shield.transform.position, transform.position) < Vector2.Distance(other.transform.position, transform.position))
                {
                    attackDamage = saveDamage;
                }
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackSight") && !isDamaged && !GameManager.instance.pause && !isDoing)
        {
            isDoing = true;
            // 공격범위에 들어옴;
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) > range && !isAttack)
            {
                anim.SetBool("isAttack", false);
                anim.SetBool("isWalk", true);
                float LR = ((other.transform.position.x > transform.position.x) ? -1f : 1f);
                transform.localScale = new Vector2(LR, 1f);
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(other.transform.position.x, transform.position.y), speed * Time.deltaTime);
            }
            StartCoroutine(Attack(other));
        }
    }
    void Update()
    {
        EnemyHp(enemyHpBar);
    }
    void EnemyHp(Transform _enemyHpBar)
    {
        _enemyHpBar.position = new Vector2(transform.position.x, transform.position.y + transform.localScale.y / 2 + 0.5f);
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
        while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
        {
            //공격
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
            {
                //공격하고 다시 false로 바뀜
                if (!holding)
                {
                    isAttack = true;
                    anim.CrossFade("Shield_Crush", 0f);
                    anim.SetBool("isAttack", true);
                    isDefending = true;
                    holding = true;
                    anim.SetBool("isWalk", false);
                    float pl = (transform.position.x > other.transform.position.x) ? -action : action;
                    yield return new WaitForSeconds(time);
                    damageOn = true;
                    // 플레이어쪽으로 돌진할 방향
                    // 돌진 대기
                    // 막는 중 아님
                    isDefending = false;
                    //addforce 사용해서 돌진
                    EnemyRB.AddForce(Vector2.right * pl * speed, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(1f);
                    isDefending = true;
                    damageOn = false;
                    holding = false;
                    yield return new WaitForSeconds(delayTime);
                    LR = transform.position.x > other.transform.parent.position.x ? 1 : -1;
                    transform.localScale = new Vector3(LR, 1);
                    anim.SetBool("isAttack", false);
                }
                isAttack = false;
            }
        }
    }
}
