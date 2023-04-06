using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestEnemy : MonoBehaviour
{
    public static RestEnemy Instance;
    [Tooltip("이동속도")]
    [SerializeField] internal float speed = 5.0f;
    [Tooltip("공격 거리")]
    [SerializeField] internal float range = 2.0f;

    [Tooltip("공격 시간")]
    [SerializeField] internal float time = 1.0f;
    [Tooltip("특정 행동 거리")]
    [SerializeField] internal float action = 5.0f;

    [Tooltip("공격 데미지")]
    [SerializeField] internal float attackDamage = 2.5f;
    [SerializeField] internal float damage;
    [Tooltip("공격 하는 중인지")]
    [SerializeField] internal bool isAttack;
    [SerializeField] internal Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        Instance = this;
        damage = attackDamage;
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
    IEnumerator Attack(Collider2D other)
    {
        while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
        {
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
            {
                Debug.Log(0);
                //공격하고 다시 false로 바뀜
                isAttack = true;
                anim.SetBool("isAttack", true);
                yield return new WaitForSeconds(time);
                anim.SetBool("isAttack", false);
                yield return new WaitForSeconds(time);
                isAttack = false;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            attackDamage = damage;
        }
        else
        {
            attackDamage = 0;
        }
    }
}
