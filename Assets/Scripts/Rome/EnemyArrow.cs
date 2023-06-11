using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    [SerializeField] private int rotateSpeed;
    [SerializeField] private Vector3 shootPos;
    [SerializeField] private GameObject target;
    [SerializeField] internal float arrowDamage, damage;
    [Tooltip("왼쪽을 바라보면 1")]
    [SerializeField] internal float LR;
    [Tooltip("화살 사라지는 시간")]
    [SerializeField] internal float arrowDestroyTime = 1.0f;
    public Vector3 targetPos;
    void Awake()
    {
        damage = CrossbowEnemy.Instance.attackDamage;
    }
    void Start()
    {
        StartCoroutine(Shoot());
        // shootPos =
    }
    IEnumerator Shoot()
    {
        targetPos = transform.position * Vector2.left * 10 * transform.parent.GetChild(1).localScale.x;
        while (transform.position != targetPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log(targetPos);
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            arrowDamage = damage;
            Destroy(gameObject);
        }
        else
        {
            arrowDamage = 0;
        }
        if (other.CompareTag("BottomGround") || other.CompareTag("MidGround"))
        {
            Destroy(gameObject);
        }
    }
}
