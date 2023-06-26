using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : PlayerWeapons
{
    [SerializeField] internal float destroyRange;
    [SerializeField] internal Vector3 targetPos;
    void Awake()
    {
        // transform.localScale = new Vector2(-1.0f, 0.2f);
    }
    IEnumerator Start()
    {
        if (GameManager.instance.age == GameManager.eAge.로마)
        {
            // Destroy(gameObject, PlayerShoot.instance.destroyTime);
        }
        if (GameManager.instance.age == GameManager.eAge.현대)
        {
            targetPos = transform.position + Vector3.right * destroyRange * transform.parent.localScale.x;
            while (gameObject != null && gameObject.transform.position != targetPos)
            {
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, targetPos, 0.1f);
                yield return new WaitForSeconds(0.01f);
            }
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (GameManager.instance.age == GameManager.eAge.로마)
        {
            transform.right = GetComponent<Rigidbody2D>().velocity;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("MidGround") || other.CompareTag("BottomGround") || other.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}