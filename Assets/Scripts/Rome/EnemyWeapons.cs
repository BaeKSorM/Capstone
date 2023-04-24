using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapons : MonoBehaviour
{
    [SerializeField] internal float damage;
    [SerializeField] internal float attackDamage;
    RestEnemy restEnemy;
    void Start()
    {
        restEnemy = transform.parent.GetComponent<RestEnemy>();
        attackDamage = restEnemy.attackDamage;
    }
}
