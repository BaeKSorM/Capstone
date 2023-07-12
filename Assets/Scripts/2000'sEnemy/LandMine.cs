using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{
    [SerializeField] internal float damage;
    [SerializeField] internal float saveDamage;
    [SerializeField] internal float explosionTime = 0.5f;
    [SerializeField] internal float destroyTime;
    [SerializeField] internal Transform target;
    [SerializeField] internal Animator anim;
    [SerializeField] internal Rigidbody2D landMineRB;
    // [SerializeField] internal CapsuleCollider2D landMineCC;
    [SerializeField] internal GameObject button;
    [SerializeField] GameObject shield;
    void Awake()
    {
        shield = GameObject.Find("Shield");
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
            if (Vector2.Distance(shield.transform.position, transform.position) < Vector2.Distance(other.transform.position, transform.position))
            {
                damage = saveDamage;
                damage -= damage - PlayerController.instance.reduce > 0 ? PlayerController.instance.reduce : damage;
            }
            else if (Vector2.Distance(shield.transform.position, transform.position) < Vector2.Distance(other.transform.position, transform.position))
            {
                damage = saveDamage;
            }
            Destroy(gameObject, explosionTime);
        }
    }
}
