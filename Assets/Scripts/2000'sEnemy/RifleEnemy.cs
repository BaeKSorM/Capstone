using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleEnemy : Creature
{
    public static RifleEnemy Instance;
    [Tooltip("공격 거리")]
    [SerializeField] internal GameObject bullet;
    [SerializeField] internal Transform player;
    [SerializeField] internal Transform enemyHpBar;
    [SerializeField] internal Transform shootPos;
    [SerializeField] internal float wallDistance = 0.5f;
    [SerializeField] internal bool isAvoiding;
    [SerializeField] internal bool isDamaged;
    [SerializeField] bool isWall;
    [SerializeField] internal int LR;
    [SerializeField] internal bool isDoing;
    Canvas canvas;
    Rigidbody2D EnemyRB;
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
    }
    IEnumerator OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackSight") && !isDamaged && !GameManager.instance.pause && !isDoing)
        {
            isDoing = true;
            // 공격범위에 들어옴;
            while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) > range && !isAttack)
            {
                // Debug.Log("run");
                float LR = ((other.transform.position.x > transform.position.x) ? -1f : 1f);
                transform.localScale = new Vector2(LR, 1f);
                anim.SetBool("isAttack", false);
                anim.SetBool("isWalk", true);
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(other.transform.parent.position.x, transform.position.y), speed * Time.deltaTime);
                yield return null;
            }
            StartCoroutine(Attack(other));
        }
    }
    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("PlayerWeapon") && other.name == "Shield")
    //     {
    //         PlayerController.instance.reduceDamage = 0;
    //     }
    // }
    void Damaged()
    {
        anim.SetTrigger("isDamaged");
        EnemyRB.AddForce(new Vector2(LR * 4, 0), ForceMode2D.Impulse);
        isDamaged = false;
    }
    IEnumerator Attack(Collider2D other)
    {
        isDoing = false;
        while (!isAttack && Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range)
        {
            isAttack = true;
            // Debug.Log(isAttack);
            yield return new WaitUntil(() => !GameManager.instance.pause);
            float LR = ((other.transform.position.x > transform.position.x) ? -1f : 1f);
            transform.localScale = new Vector2(LR, 1f);
            // Debug.Log("attack");
            anim.SetBool("isWalk", false);
            //공격하고 다시 false로 바뀜
            yield return new WaitForSeconds(delayTime);
            anim.SetBool("isAttack", true);
            for (int i = 0; i < 3; ++i)
            {
                GameObject bulletClone = Instantiate(bullet, shootPos.position, Quaternion.identity);
                bulletClone.transform.parent = transform.parent;
                EnemyProjectile enemyProjectile = bulletClone.GetComponent<EnemyProjectile>();
                enemyProjectile.LR = LR;
                yield return new WaitForSeconds(0.1f);
            }
            anim.SetBool("isAttack", false);
            yield return new WaitForSeconds(time);
            isAttack = false;
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
}
