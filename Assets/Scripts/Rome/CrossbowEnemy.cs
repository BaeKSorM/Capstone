using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowEnemy : MonoBehaviour
{
    public static CrossbowEnemy Instance;
    [Tooltip("이동속도")]
    [SerializeField] internal float speed = 5.0f;
    [Tooltip("공격 거리")]
    [SerializeField] internal float range = 7.0f;

    [Tooltip("공격 시간")]
    [SerializeField] internal float time = 1.0f;
    [Tooltip("특정 행동 거리")]
    [SerializeField] internal float action = 5.0f;
    [Tooltip("공격 데미지")]
    [SerializeField] internal float attackDamage = 2.5f;
    [Tooltip("공격 하는 중인지")]
    [SerializeField] internal bool isAttack;
    [SerializeField] internal bool isAvoid;
    [SerializeField] internal GameObject arrow;
    [SerializeField] internal Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        Instance = this;
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
            // 화살 쏘는 애는 너무 플레이어와 너무 가까우면 거리두기기
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range / 2 && !isAvoid)
            {
                Debug.Log(new Vector2((transform.position.x > other.transform.position.x) ? transform.position.x + action : transform.position.x - action, transform.position.y));
                transform.position = Vector2.MoveTowards(transform.position, new Vector2((transform.position.x > other.transform.position.x) ? transform.position.x + action : transform.position.x - action, transform.position.y), speed * 2 * Time.deltaTime);
                yield return new WaitForSeconds(speed * 2);
                isAvoid = true;
            }
            // 공격
            else if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
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
        }
    }
}
