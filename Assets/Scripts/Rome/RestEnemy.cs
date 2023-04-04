using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestEnemy : MonoBehaviour
{
    [Tooltip("이동속도")]
    [SerializeField] internal float speed;
    [Tooltip("공격 거리")]
    [SerializeField] internal float range;

    [Tooltip("공격 시간")]
    [SerializeField] internal float time;
    [Tooltip("특정 행동 거리")]
    [SerializeField] internal float action;
    [Tooltip("화살 출발 위치")]
    [SerializeField] internal Transform arrowPos;
    [Tooltip("공격 하는 중인지")]
    [SerializeField] internal bool isAttack;
    [SerializeField] internal Animator anim;

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
                yield return new WaitForSeconds(time * 2);
                isAttack = false;
            }
        }
    }
}
