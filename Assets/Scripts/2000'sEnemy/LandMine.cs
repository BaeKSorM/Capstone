using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{
    [SerializeField] internal float damage;
    [SerializeField] internal float explosionTime = 0.5f;
    [SerializeField] internal float destroyTime;
    [SerializeField] internal Transform target;
    [SerializeField] internal Animator anim;
    [SerializeField] internal Rigidbody2D landMineRB;
    // [SerializeField] internal CapsuleCollider2D landMineCC;
    [SerializeField] internal GameObject button;
    void Awake()
    {
        anim = GetComponent<Animator>();
        // landMineCC = GetComponent<CapsuleCollider2D>();
        landMineRB = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BottomGround"))
        {
            landMineRB.bodyType = RigidbodyType2D.Kinematic;
            button.SetActive(true);
            Destroy(gameObject, destroyTime);
        }
        if (other.CompareTag("Player"))
        {
            Destroy(button);
            anim.SetTrigger("explosion");
            Destroy(gameObject, explosionTime);
        }
    }
}
