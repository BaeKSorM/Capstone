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

    Rigidbody2D EnemyRB;
    Canvas canvas;
    void Start()
    {
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
            if (other.transform.Find("Shield").gameObject.activeSelf)
            {
                if (Mathf.Abs(transform.position.x - other.transform.position.x) > Mathf.Abs(transform.position.x - other.transform.Find("Shield").position.x) ? true : false)
                {
                    PlayerController.instance.Reduce();
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
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(other.transform.position.x, transform.position.y), speed * Time.deltaTime);
            }
            StartCoroutine(Attack(other));
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon") && other.name.Contains("h"))
        {
            PlayerController.instance.reduceDamage = 0;
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
                    anim.SetBool("isCrush", true);
                    anim.CrossFade("Shield_Crush", 0f);
                    isDefending = true;
                    holding = true;
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
                    anim.SetBool("isCrush", false);
                }
                isAttack = false;
            }
        }
    }
}
