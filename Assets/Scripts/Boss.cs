using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float speed;
    [Tooltip("왼쪽 끝 달릴 위치")]
    [SerializeField] Vector2 rushPos;
    [Tooltip("오른쪽 끝 달릴 위치")]
    [SerializeField] Vector2 nextRushPos;
    [Tooltip("위치 바꿀 변수")]
    [SerializeField] Vector2 swap;
    [Tooltip("끝으로의 이동 시간")]
    [SerializeField] private float moveTime;
    [Tooltip("공격 시간")]
    [SerializeField] private float attackTime;
    [SerializeField] Animator anim;
    Rigidbody2D bossRB;
    private void Start()
    {
        bossRB = GetComponent<Rigidbody2D>();
        StartCoroutine(BossMove());
    }
    IEnumerator BossMove()
    {
        anim.SetTrigger("Move");
        transform.position = Vector2.MoveTowards(transform.position, rushPos, speed * Time.deltaTime);
        swap = rushPos;
        rushPos = nextRushPos;
        nextRushPos = swap;
        yield return new WaitForSeconds(moveTime);
        StartCoroutine(BossmachineGun());
    }
    IEnumerator BossmachineGun()
    {
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(attackTime);
        anim.SetBool("Attack", false);
        StartCoroutine(BossMove());
    }

}
