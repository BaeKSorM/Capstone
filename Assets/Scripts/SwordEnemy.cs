using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordEnemy : Creature
{
    public static SwordEnemy Instance;
    [SerializeField] internal GameObject weapon;
    [SerializeField] internal Transform enemyHpBar;
    [SerializeField] internal bool isDamaged;
    [SerializeField] internal int LR;
    [SerializeField] internal bool isDoing;
    [SerializeField] internal bool isSpawned;
    [SerializeField] LayerMask layerNumber;

    [SerializeField] GameObject shield;
    Canvas canvas;
    Rigidbody2D EnemyRB;
    void Start()
    {
        shield = GameObject.Find("Shield");
        speed = 2.0f;
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
    public bool collisionEnabled = true;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon") && other.name.Contains("Z"))
        {
            hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
            LR = transform.position.x > other.transform.parent.position.x ? 1 : -1;
            Damaged();
        }
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
    IEnumerator OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackSight") && !isDamaged && !GameManager.instance.pause && !isDoing)
        {
            isDoing = true;
            // 공격범위에 들어옴;
            while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) > range && !isAttack)
            {
                float LR = ((other.transform.position.x > transform.position.x) ? -1f : 1f);
                transform.localScale = new Vector2(LR, 1f);
                anim.SetBool("isAttack", false);
                anim.SetBool("isWalk", true);
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(other.transform.position.x, transform.position.y), speed * Time.deltaTime);
                yield return null;
            }
            StartCoroutine(Attack(other));
            anim.SetBool("isWalk", false);
        }
    }

    void Damaged()
    {
        anim.SetTrigger("isDamaged");
        EnemyRB.AddForce(new Vector2(LR / 2, 0), ForceMode2D.Impulse);
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
        isDoing = false;
        while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
        {
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
            {
                //공격하고 다시 false로 바뀜
                isAttack = true;
                anim.SetBool("isAttack", true);
                yield return new WaitForSeconds(time / 3);
                weapon.SetActive(true);
                yield return new WaitForSeconds(time / 3 * 2);
                weapon.SetActive(false);
                anim.SetBool("isWalk", false);
                anim.SetBool("isAttack", false);
                yield return new WaitForSeconds(delayTime);
                isAttack = false;
                LR = transform.position.x > other.transform.parent.position.x ? 1 : -1;
                transform.localScale = new Vector3(LR, 1);
            }
        }
    }
}
