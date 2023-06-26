using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
    [SerializeField] internal float damage;
    [SerializeField] internal bool isGas;
    [SerializeField] internal Transform target;
    [SerializeField] internal Animator anim;
    Rigidbody2D gasRB;
    void Awake()
    {
        gasRB = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BottomGround"))
        {
            StartCoroutine(Release());
        }
    }
    void OnDestroy()
    {
        isGas = false;
    }
    IEnumerator Release()
    {
        gasRB.bodyType = RigidbodyType2D.Kinematic;
        yield return new WaitForSeconds(0.2f);
        isGas = true;
        anim.SetTrigger("gas");
        Destroy(gameObject, 5.0f);
    }
}

