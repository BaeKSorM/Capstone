using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldEnemy : MonoBehaviour
{
    public static ShieldEnemy Instance;
    [Tooltip("이동속도")]
    [SerializeField] internal float speed = 5.0f;
    [Tooltip("공격 거리")]
    [SerializeField] internal float range = 3.0f;

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
    [Tooltip("돌진 대기중")]
    [SerializeField] internal bool holding;
    [SerializeField] internal Slider hpbar;
    [Tooltip("막는 중인지")]
    [SerializeField] internal bool isDefending;
    [Tooltip("막는 중일때 방패에 닿였는지")]
    [SerializeField] internal bool defend;
    [SerializeField] internal Transform enemyHpBar;
    Rigidbody2D rb;

    [SerializeField] internal Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyHpBar = transform.GetChild(0).GetChild(0);
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
