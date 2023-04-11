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
        damage = restEnemy.attackDamage;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            attackDamage = damage;
            Debug.Log(1);
        }
        else
        {
            attackDamage = 0;
        }
        //yield return new WaitForSeconds(restEnemy.time);
    }
}
