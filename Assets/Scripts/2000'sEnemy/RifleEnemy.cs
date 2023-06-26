using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleEnemy : Creature
{
    public static RifleEnemy Instance;
    [Tooltip("공격 거리")]
    [SerializeField] internal GameObject arrow;
    [SerializeField] internal Transform player;
    [SerializeField] internal Transform enemyHpBar;
    [SerializeField] internal Transform shootPos;
    [SerializeField] internal float wallDistance = 0.5f;
    [SerializeField] internal bool isAvoiding;
    [SerializeField] internal bool isDamaged;
    [SerializeField] bool isWall;
    [SerializeField] internal int LR;
    Canvas canvas;
    Rigidbody2D EnemyRB;
    void Start()
    {
        // speed = 5.0f;
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
        if (other.CompareTag("PlayerWeapon") && other.name.Contains("h"))
        {
            PlayerController.instance.reduceDamage = PlayerController.instance.reduce;
        }
    }
    IEnumerator OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackSight") && !isDamaged)
        {
            // 공격범위에 들어옴;
            while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) > range && !isAttack && !isAvoiding)
            {
                // Debug.Log("run");
                anim.SetBool("isAttack", false);
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(other.transform.position.x, transform.position.y), 0.001f);
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
        while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack && !isWall)
        {
            float LR = ((other.transform.position.x > transform.position.x) ? -1f : 1f);
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
            yield return new WaitForSeconds(0.1f);
            GameObject arrowClone2 = Instantiate(arrow, shootPos.position, Quaternion.identity);
            arrowClone2.transform.parent = transform.parent;
            EnemyProjectile enemyProjectile2 = arrowClone2.GetComponent<EnemyProjectile>();
            enemyProjectile2.LR = LR;
            yield return new WaitForSeconds(0.1f);
            GameObject arrowClone3 = Instantiate(arrow, shootPos.position, Quaternion.identity);
            arrowClone3.transform.parent = transform.parent;
            EnemyProjectile enemyProjectile3 = arrowClone3.GetComponent<EnemyProjectile>();
            enemyProjectile3.LR = LR;
            yield return new WaitForSeconds(time);
            isAttack = false;
            isAvoiding = false;
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
