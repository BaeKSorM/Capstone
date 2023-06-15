using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestEnemy : Creature
{
    public static RestEnemy Instance;
    [SerializeField] internal float saveDamage;
    [SerializeField] internal GameObject weapon;
    [SerializeField] internal Transform enemyHpBar;
    [SerializeField] internal bool isDamaged;
    [SerializeField] internal int LR;
    [SerializeField] internal bool isSpawned;

    Canvas canvas;
    Rigidbody2D EnemyRB;
    void Start()
    {
        // speed = 5.0f;
        // range = 2.0f;
        // time = 1.0f;
        // delayTime = 1.0f;
        // action = 5.0f;
        // attackDamage = 2.5f;
        saveDamage = attackDamage;
        anim = GetComponent<Animator>();
        EnemyRB = GetComponent<Rigidbody2D>();
        enemyHpBar = transform.parent.GetChild(0).GetChild(0);
        canvas = transform.parent.GetChild(0).GetComponent<Canvas>();
        weapon = transform.GetChild(0).gameObject;
        Instance = this;
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
        if (other.CompareTag("PlayerWeapon") && other.name == "Shield")
        {
            PlayerController.instance.Reduce();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackSight") && !isDamaged)
        {
            // 공격범위에 들어옴;
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) > range && !isAttack)
            {
                anim.SetBool("isAttack", false);
                anim.SetBool("isMove", true);
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
    void Damaged()
    {
        anim.SetTrigger("isDamaged");
        EnemyRB.AddForce(new Vector2(LR * 4, 0), ForceMode2D.Impulse);
        isDamaged = false;
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
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
            {
                //공격하고 다시 false로 바뀜]
                isAttack = true;
                anim.SetBool("isAttack", true);
                weapon.SetActive(true);
                yield return new WaitForSeconds(time);
                anim.SetBool("isMove", false);
                anim.SetBool("isAttack", false);
                weapon.SetActive(false);
                yield return new WaitForSeconds(delayTime);
                isAttack = false;
                LR = transform.position.x > other.transform.parent.position.x ? 1 : -1;
                transform.localScale = new Vector3(LR, 1);
            }
        }
    }
}
