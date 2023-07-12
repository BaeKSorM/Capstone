using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
    [Tooltip("이동속도")]
    [SerializeField] internal float speed;
    [Tooltip("공격 거리")]
    [SerializeField] internal float range;
    [Tooltip("공격 시간")]
    [SerializeField] internal float time;
    [Tooltip("공격 대기 시간")]
    [SerializeField] internal float delayTime;
    [Tooltip("특정 행동 거리")]
    [SerializeField] internal float action;
    [Tooltip("공격 데미지")]
    [SerializeField] internal float attackDamage;
    [Tooltip("저장 공격 데미지")]
    [SerializeField] internal float saveDamage;
    public float damage;
    [Tooltip("공격 하는 중인지")]
    [SerializeField] internal bool isAttack;
    [SerializeField] internal Animator anim;
    [SerializeField] internal Slider hpbar;
}
