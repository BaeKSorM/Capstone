using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    [SerializeField] private int rotateSpeed;
    [SerializeField] private GameObject target;
    [SerializeField] internal float arrowDamage, damage;
    [Tooltip("왼쪽을 바라보면 0")]
    [SerializeField] internal float LR;
    [Tooltip("화살 사라지는 시간")]
    [SerializeField] internal float arrowDestroyTime = 1.0f;
    Vector3 targetPos;
    void Awake()
    {
        damage = CrossbowEnemy.Instance.attackDamage;
    }
    void Start()
    {
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        targetPos = new Vector3(LR * 10, transform.position.y);
        while (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.2f);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.name);
            arrowDamage = damage;
            Destroy(gameObject);
        }
        else
        {
            arrowDamage = 0;
        }
    }
}
