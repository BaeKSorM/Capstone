using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapons : MonoBehaviour
{
    SwordEnemy swordEnemy;
    void Start()
    {
        swordEnemy = GetComponent<SwordEnemy>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // RestEnemy.Instance.attackDamage = RestEnemy.Instance.saveDamage;
        }
    }
}
