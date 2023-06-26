using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialBomb : MonoBehaviour
{
    [SerializeField] internal Transform target;
    [SerializeField] internal float damage;
    [SerializeField] internal float saveDamage;
    [SerializeField] internal float destroyTime;
    [SerializeField] internal Animator anim;
    [SerializeField] internal Rigidbody2D aeriaRB;
    [SerializeField] internal CapsuleCollider2D landMineCC;
    void Awake()
    {
        saveDamage = damage;
        anim = GetComponent<Animator>();
        landMineCC = GetComponent<CapsuleCollider2D>();
        aeriaRB = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BottomGround"))
        {
            aeriaRB.bodyType = RigidbodyType2D.Kinematic;
            landMineCC.enabled = false;
            anim.SetTrigger("explosion");
            Destroy(gameObject, destroyTime);
        }
        if (other.CompareTag("Player"))
        {
            damage = saveDamage;
            aeriaRB.bodyType = RigidbodyType2D.Kinematic;
            landMineCC.enabled = false;
            anim.SetTrigger("explosion");
            Destroy(gameObject, destroyTime);
        }
        else
        {
            damage = 0;
        }
        // Debug.Log(other.GetComponent<Collider2D>());
    }
}
