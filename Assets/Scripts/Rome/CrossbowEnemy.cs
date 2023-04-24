using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CrossbowEnemy : MonoBehaviour
{
    public static CrossbowEnemy Instance;
    [Tooltip("이동속도")]
    [SerializeField] internal float speed = 5.0f;
    [Tooltip("공격 거리")]
    [SerializeField] internal float range = 7.0f;
    [Tooltip("공격 거리")]
    [SerializeField] internal float dangerRange = 2.0f;

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
    [SerializeField] internal bool isAvoid;
    [SerializeField] internal bool avoiding;
    [SerializeField] internal GameObject arrow;
    [SerializeField] internal Animator anim;
    [SerializeField] internal Slider hpbar;
    [SerializeField] internal Transform enemyHpBar;


    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Instance = this;
        enemyHpBar = transform.GetChild(0).GetChild(0);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            Debug.Log(other.GetComponent<PlayerWeapons>().damage);
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
            // 화살 쏘는 애는 너무 플레이어와 너무 가까우면 거리두기기
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= dangerRange && !isAvoid && !avoiding)
            {
                avoiding = true;
                // Debug.Log(new Vector2((transform.position.x > other.transform.position.x) ? transform.position.x + action : transform.position.x - action, transform.position.y));
                float pl = (transform.position.x > other.transform.position.x) ? action : -action;
                StartCoroutine(Avoidance(new Vector2(transform.position.x + pl, transform.position.y)));
                //rb.AddForce(new Vector2(pl, 0), ForceMode2D.Impulse);
                yield return new WaitForSeconds(delayTime);
                // Debug.Log(pl);
                // Debug.Log(isAvoid);
            }
            // 공격
            else if (!avoiding)
            {
                //공격하고 다시 false로 바뀜
                GameObject arrowClone = Instantiate(arrow, gameObject.transform.position + ((other.transform.position.x > transform.position.x) ? Vector3.right : Vector3.left), Quaternion.identity);
                isAttack = true;
                anim.SetBool("isAttack", true);
                yield return new WaitForSeconds(time);
                anim.SetBool("isAttack", false);
                isAttack = false;
                isAvoid = false;
            }
            else
            {
                yield return null;
            }
        }
    }
    IEnumerator Avoidance(Vector2 arrivePos)
    {
        // arrivePos에 도착할때까지 이동
        while (Mathf.Abs(transform.position.x - arrivePos.x) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, arrivePos, 0.01f);
            yield return new WaitForSeconds(0.001f);
        }
        avoiding = false;
        isAvoid = true;
    }
}
