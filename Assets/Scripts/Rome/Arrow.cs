using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private int rotateSpeed;
    [SerializeField] private GameObject target;
    [SerializeField] internal float arrowDamage, damage;
    [Tooltip("왼쪽을 바라보면 0")]
    float lookLeftRange;
    Vector3 targetPos;
    void Awake()
    {
        target = FindObjectOfType<PlayerController>().gameObject;
        damage = CrossbowEnemy.Instance.attackDamage;
    }

    void Start()
    {
        Vector2 direction = new Vector2(transform.position.x - target.transform.position.x, transform.position.y - target.transform.position.y);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - lookLeftRange, Vector3.forward);
        transform.rotation = angleAxis;
        targetPos = target.transform.position - transform.position;
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos + transform.position, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            arrowDamage = damage;
        }
        else
        {
            arrowDamage = 0;
        }
    }
}
