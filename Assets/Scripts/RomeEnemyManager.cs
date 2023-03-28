using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RomeEnemyManager : MonoBehaviour
{
    [Tooltip("enum에 따른 이동속도")]
    [SerializeField] internal List<float> moveSpeed;
    [Tooltip("enum에 따른 공격 거리")]
    [SerializeField] internal List<int> attackRange;
    [Tooltip("enum에 따른 공격 시간")]
    [SerializeField] internal List<float> attackTime;
}