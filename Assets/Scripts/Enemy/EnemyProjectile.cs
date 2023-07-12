using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private int rotateSpeed;
    [SerializeField] private Vector3 shootPos;
    [SerializeField] private GameObject target;
    [SerializeField] internal float projectileDamage, saveDamage;
    [Tooltip("왼쪽을 바라보면 1")]
    [SerializeField] internal float LR;
    [Tooltip("화살 사라지는 거리")]
    [SerializeField] internal float destroyRange = 5.0f;
    [SerializeField] internal Vector3 targetPos;
    [SerializeField] internal int repeat;
    [SerializeField] GameObject shield;
    void Start()
    {
        shield = GameObject.Find("Shield");
        if (name.Contains("Arrow"))//투사체 이름
        {
            saveDamage = CrossbowEnemy.Instance.attackDamage;
        }
        else if (name.Contains("Turret"))
        {
            saveDamage = TurretEnemy.Instance.attackDamage;
        }
        else if (name.Contains("Bullet"))
        {
            saveDamage = RifleEnemy.Instance.attackDamage;
        }
        else if (name.Contains("Turret"))
        {
            saveDamage = TurretEnemy.Instance.attackDamage;
        }
        if (name.Contains("Century"))
        {
            saveDamage = Century21Boss.instance.attackDamage;
        }
        if (name.Contains("Rifle"))
        {
            saveDamage = RifleEnemy.Instance.attackDamage;
        }
        StartCoroutine(Shoot());
        // shootPos =
    }
    IEnumerator Shoot()
    {
        targetPos = transform.position + Vector3.left * destroyRange * transform.parent.GetChild(1).localScale.x;
        while (transform.position != targetPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        PlayerController.instance.reduceDamage = 0;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.CompareTag("Player"))
            {
                if (Vector2.Distance(shield.transform.position, transform.position) < Vector2.Distance(other.transform.position, transform.position))
                {
                    projectileDamage = saveDamage;
                    projectileDamage -= projectileDamage - PlayerController.instance.reduce > 0 ? PlayerController.instance.reduce : projectileDamage;
                }
                else if (Vector2.Distance(shield.transform.position, transform.position) < Vector2.Distance(other.transform.position, transform.position))
                {
                    projectileDamage = saveDamage;
                }
            }
            Destroy(gameObject);
        }
        else
        {
            projectileDamage = 0;
        }
        if (other.CompareTag("BottomGround") || other.CompareTag("MidGround") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        // Debug.Log(other.tag);
    }
}
