using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIM120B : MonoBehaviour
{
    [SerializeField] internal float damage;
    [SerializeField] internal GameObject p;
    [SerializeField] internal Transform targetPos;
    [SerializeField] internal float rise;
    [SerializeField] internal float deg;
    [SerializeField] internal int LR;
    [SerializeField] internal Animator anim;
    [SerializeField] internal bool bombing;
    [SerializeField] internal Quaternion q;
    [SerializeField] internal Vector3 direction;
    [SerializeField] internal Quaternion q2;

    void Start()
    {
        StartCoroutine(Rise());
        p = GameObject.Find("Player");

        Vector2 direction = p.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        deg = GetAngleBetweenVectors(p.transform.position, transform.position);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // anim.SetTrigger("bomb");
            bombing = true;
            Destroy(gameObject);
        }
    }
    IEnumerator Rise()
    {
        Vector3 risePos = transform.position + new Vector3(0, rise, 0);
        while (transform.position != risePos)
        {
            transform.position = Vector3.MoveTowards(transform.position, risePos, 0.01f);
            yield return null;
        }
        StartCoroutine(Launch());
    }
    float GetAngleBetweenVectors(Vector3 player, Vector3 missile)
    {
        Vector3 direction = player - missile;
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        float angle = rotation.eulerAngles.z; // = Vector3.Angle(Vector3.up, direction);
        return angle;
    }
    IEnumerator Launch()
    {
        direction = new Vector3(0, 0, (GetAngleBetweenVectors(p.transform.position, transform.position) - 90));
        q = Quaternion.Euler(direction);
        q2 = Quaternion.Euler(0, 0, 360);//- q;
        q2 = Quaternion.Inverse(q2) * q;
        if (LR == 1)
        {
            do
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, q, 0.01f);
                yield return null;
            } while (transform.rotation != q);
        }
        else
        {
            do
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, q, 0.01f);
                yield return null;
            } while (transform.rotation != q2);
        }
        Vector2 forward = transform.up;
        while (gameObject != null || !bombing)
        {
            transform.position += (Vector3)forward * Time.deltaTime;
            yield return null;
        }
    }
}
