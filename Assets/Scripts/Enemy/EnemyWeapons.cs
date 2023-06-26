using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapons : MonoBehaviour
{
    RestEnemy restEnemy;
    void Start()
    {
        restEnemy = GetComponent<RestEnemy>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // RestEnemy.Instance.attackDamage = RestEnemy.Instance.saveDamage;
        }
    }
}
