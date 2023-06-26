using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private int rotateSpeed;
    [SerializeField] private Vector3 shootPos;
    [SerializeField] private GameObject target;
    [SerializeField] internal float projectileDamage, damage;
    [Tooltip("왼쪽을 바라보면 1")]
    [SerializeField] internal float LR;
    [Tooltip("화살 사라지는 거리")]
    [SerializeField] internal float destroyRange = 5.0f;
    [SerializeField] internal Vector3 targetPos;
    [SerializeField] internal int repeat;
    void Start()
    {
        if (name.Contains("Arrow"))//투사체 이름
        {
            damage = CrossbowEnemy.Instance.attackDamage;
        }
        // else if (name.Contains("Rifle"))
        // {
        //     damage = RifleEnemy.Instance.attackDamage;
        // }
        // else 
        // if (name.Contains("Turret"))
        // {
        //     damage = TurretEnemy.Instance.attackDamage;
        // }
        if (name.Contains("Century"))
        {
            damage = Century21Boss.instance.attackDamage;
        }
        if (name.Contains("Present"))
        {
            damage = 10;
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
            projectileDamage = damage;
            if (other.transform.Find("Shield").gameObject.activeSelf)
            {
                if (Mathf.Abs(transform.position.x - other.transform.position.x) > Mathf.Abs(transform.position.x - other.transform.Find("Shield").position.x) ? true : false)
                {
                    PlayerController.instance.Reduce();
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
        Debug.Log(other.tag);
    }
}
