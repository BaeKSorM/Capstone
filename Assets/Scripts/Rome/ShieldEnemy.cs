using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Tooltip("돌진 대기중")]
    public bool holding;
    Rigidbody2D rb;

    [SerializeField] internal Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
            //공격
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
            {
                Debug.Log(0);
                //공격하고 다시 false로 바뀜
                isAttack = true;
                if (!holding)
                {
                    yield return new WaitForSeconds(time);
                    // 플레이어쪽으로 돌진할 방향
                    float pl = (transform.position.x > other.transform.position.x) ? -action : action;
                    //addforce 사용해서 돌진
                    holding = true;
                    rb.AddForce(Vector2.right * pl * speed);
                    yield return new WaitForSeconds(delayTime);
                    holding = false;
                }
                isAttack = false;
            }
        }
    }

}
