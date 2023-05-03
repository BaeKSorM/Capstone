using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [Tooltip("데미지")]
    [SerializeField] internal int damage;
    [Tooltip("공격 시간")]
    [SerializeField] internal float time;
    [SerializeField] internal float saveDamage;
}
