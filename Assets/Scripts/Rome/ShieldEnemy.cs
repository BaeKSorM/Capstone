using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : MonoBehaviour
{
    [Tooltip("이동속도")]
    [SerializeField] internal float speed = 5.0f;
    [Tooltip("공격 거리")]
    [SerializeField] internal float range = 3.0f;

    [Tooltip("공격 시간")]
    [SerializeField] internal float time = 1.0f;
    [Tooltip("특정 행동 거리")]
    [SerializeField] internal float action = 5.0f;
    [Tooltip("공격 하는 중인지")]
    [SerializeField] internal bool isAttack;
    [Tooltip("돌진 대기중")]
    public bool holding;
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
                    holding = true;
                    yield return new WaitForSeconds(time);
                    // 플레이어쪽으로 돌진할 방향
                    Vector2 pl = new Vector2((transform.position.x > other.transform.position.x) ? transform.position.x - action : transform.position.x + action, transform.position.y);
                    while (transform.position.x != pl.x)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, pl, 0.1f);
                        yield return new WaitForSeconds(0.01f);
                    }
                    holding = false;
                    yield return new WaitForSeconds(time);
                }
                isAttack = false;
            }
        }
    }

}
