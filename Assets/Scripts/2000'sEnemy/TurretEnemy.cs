using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : Creature
{
    public static TurretEnemy Instance;
    [Tooltip("공격 거리")]
    [SerializeField] internal GameObject arrow;
    [SerializeField] internal Transform player;
    [SerializeField] internal Transform enemyHpBar;
    [SerializeField] internal Transform shootPos;
    [SerializeField] internal float wallDistance = 0.5f;
    [SerializeField] internal bool isAvoiding;
    [SerializeField] internal bool isDamaged;
    [SerializeField] bool isWall;
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
            Damaged();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackSight") && !isDamaged && !GameManager.instance.pause)
        {
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) > range)
            {
                anim.SetBool("isAttack", false);
            }
            else
            {
                StartCoroutine(Attack(other));
            }
        }
    }
    void Damaged()
    {
        anim.SetTrigger("isDamaged");
        isDamaged = false;
    }
    IEnumerator Attack(Collider2D other)
    {
        while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
        {
            yield return new WaitUntil(() => !GameManager.instance.pause);
            // Debug.Log("attack");
            isAttack = true;
            anim.SetBool("isAttack", true);
            //공격하고 다시 false로 바뀜
            yield return new WaitForSeconds(delayTime);
            GameObject arrowClone = Instantiate(arrow, shootPos.position, Quaternion.identity);
            arrowClone.transform.parent = transform.parent;
            EnemyProjectile enemyProjectile = arrowClone.GetComponent<EnemyProjectile>();
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
