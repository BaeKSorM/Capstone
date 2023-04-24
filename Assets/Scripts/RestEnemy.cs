using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestEnemy : MonoBehaviour
{
    public static RestEnemy Instance;
    [Tooltip("이동속도")]
    [SerializeField] internal float speed = 5.0f;
    [Tooltip("공격 거리")]
    [SerializeField] internal float range = 2.0f;

    [Tooltip("공격 시간")]
    [SerializeField] internal float time = 1.0f;
    [Tooltip("공격 대기 시간")]
    [SerializeField] internal float delayTime = 1.0f;
    [Tooltip("특정 행동 거리")]
    [SerializeField] internal float action = 5.0f;

    [Tooltip("공격 데미지")]
    [SerializeField] internal float attackDamage = 2.5f;
    [Tooltip("공격 하는 중인지")]
    [SerializeField] internal bool isAttack;
    [SerializeField] internal GameObject weapon;
    [SerializeField] internal Animator anim;
    [SerializeField] internal Slider hpbar;
    [SerializeField] internal Transform enemyHpBar;

    void Start()
    {
        anim = GetComponent<Animator>();
        weapon = transform.GetChild(1).gameObject;
        Instance = this;
        enemyHpBar = transform.GetChild(0).GetChild(0);
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
        if (other.gameObject.CompareTag("AttackSight"))
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
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
            {
                //공격하고 다시 false로 바뀜
                isAttack = true;
                weapon.SetActive(true);
                anim.SetBool("isAttack", true);
                yield return new WaitForSeconds(time);
                anim.SetBool("isAttack", false);
                weapon.SetActive(false);
                yield return new WaitForSeconds(delayTime);
                isAttack = false;
            }
        }
    }
}
