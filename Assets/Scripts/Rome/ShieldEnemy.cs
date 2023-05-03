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
    Rigidbody2D rb;
    Canvas canvas;
    void Start()
    {
        speed = 5.0f;
        range = 3.0f;
        time = 1.0f;
        delayTime = 1.0f;
        action = 5.0f;
        attackDamage = 2.5f;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyHpBar = transform.parent.GetChild(0).GetChild(0);
        canvas = transform.parent.GetChild(0).GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        Instance = this;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            if (transform.localScale.x != other.transform.parent.localScale.x)
            {
                defend = false;
            }
            else
            {
                defend = true;
            }
            if (!isDefending)
            {
                hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
            }
            else if (isDefending)
            {
                Debug.Log(0);
                if (!defend)
                {
                    Debug.Log(1);
                    hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
                }
                else if (defend)
                {
                    Debug.Log(2);
                }
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("AttackSight"))
        {
            // 공격범위에 들어옴;
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) > range && !isAttack)
            {
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
    IEnumerator Attack(Collider2D other)
    {
        while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
        {
            //공격
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
            {
                //공격하고 다시 false로 바뀜
                isAttack = true;
                if (!holding)
                {
                    isDefending = true;
                    yield return new WaitForSeconds(time);
                    // 플레이어쪽으로 돌진할 방향
                    float pl = (transform.position.x > other.transform.position.x) ? -action : action;
                    // 돌진 대기
                    holding = true;
                    // 막는 중 아님
                    isDefending = false;
                    //addforce 사용해서 돌진
                    rb.AddForce(Vector2.right * pl * speed);
                    yield return new WaitForSeconds(0.2f);
                    weapon.SetActive(true);
                    holding = false;
                    yield return new WaitForSeconds(delayTime);
                }
                isAttack = false;
            }
        }
    }

}
