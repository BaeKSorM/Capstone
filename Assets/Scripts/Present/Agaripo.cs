using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agaripo : MonoBehaviour
{
    [SerializeField] internal float damage;
    [SerializeField] internal float speed;
    void Update()
    {
        transform.position += PresentBoss.instance.agariAriv * speed * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("BottomGround"))
        {
            Destroy(gameObject);
        }
    }
}
