using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region variable
    public static Enemy instance;
    [Tooltip("몹 종류 선택")]
    [SerializeField] internal enum Mobs { 석궁병, 창병, 방패병, 단검병, 대검병 };
    [SerializeField] internal Mobs mobs;
    [Tooltip("이동속도")]
    [SerializeField] internal float speed;
    [Tooltip("공격 거리")]
    [SerializeField] internal float range;

    [Tooltip("공격 시간")]
    [SerializeField] internal float time;
    [Tooltip("도망가는 거리")]
    [SerializeField] internal float Crossbow_Soldier_MoveXPos;
    [Tooltip("화살 출발 위치")]
    [SerializeField] internal Transform arrowPos;
    [Tooltip("화살")]
    [SerializeField] internal GameObject arrow;
    [Tooltip("공격 하는 중인지")]
    [SerializeField] internal bool isAttack;
    [SerializeField] internal Animator anim;
    RomeEnemyManager romeEnemyManager;
    #endregion

    void Awake()
    {
        romeEnemyManager = FindObjectOfType<RomeEnemyManager>();
    }

    /// <summary>
    /// 각 몹마다 스탯 배정
    /// </summary>
    void Start()
    {
        switch (mobs)
        {
            case Mobs.석궁병:
                {
                    speed = romeEnemyManager.moveSpeed[0];
                    range = romeEnemyManager.attackRange[0];
                    time = romeEnemyManager.attackTime[0];
                }
                break;
            case Mobs.창병:
                {
                    speed = romeEnemyManager.moveSpeed[1];
                    range = romeEnemyManager.attackRange[1];
                    time = romeEnemyManager.attackTime[1];
                }
                break;
            case Mobs.방패병:
                {
                    speed = romeEnemyManager.moveSpeed[2];
                    range = romeEnemyManager.attackRange[2];
                    time = romeEnemyManager.attackTime[2];
                }
                break;
            case Mobs.단검병:
                {
                    speed = romeEnemyManager.moveSpeed[3];
                    range = romeEnemyManager.attackRange[3];
                    time = romeEnemyManager.attackTime[3];
                }
                break;
            case Mobs.대검병:
                {
                    speed = romeEnemyManager.moveSpeed[4];
                    range = romeEnemyManager.attackRange[4];
                    time = romeEnemyManager.attackTime[4];
                }
                break;
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
    IEnumerator Attack(Collider2D other)
    {
        while (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
        {
            // 화살 쏘는 애는 너무 플레이어와 너무 가까우면 거리두기기
            // if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range / range && mobs == Mobs.석궁병)
            // {
            //     transform.position = Vector2.MoveTowards(transform.position, new Vector2((transform.position.x > other.transform.position.x) ? transform.position.x + Crossbow_Soldier_MoveXPos : transform.position.x - Crossbow_Soldier_MoveXPos, transform.position.y), speed * 2 * Time.deltaTime);
            //     yield return new WaitForSeconds(time*이동 시간);
            // }
            // 공격
            // else
            if (Mathf.Abs(transform.position.x - other.transform.parent.position.x) <= range && !isAttack)
            {
                Debug.Log(0);
                //공격하고 다시 false로 바뀜
                isAttack = true;
                // anim.setBool("isAttack", true);
                if (mobs == Mobs.석궁병)
                {
                    GameObject arrowClone = Instantiate(arrow, gameObject.transform.position + ((other.transform.position.x > transform.position.x) ? Vector3.right : Vector3.left), Quaternion.identity);
                }
                yield return new WaitForSeconds(time);
                // anim.setBool("isAttack", false);
                yield return new WaitForSeconds(time);
                isAttack = false;
            }
        }
    }
}
