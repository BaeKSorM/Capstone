using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSniperEnemy : MonoBehaviour
{
    [SerializeField] internal GameObject p;
    [SerializeField] internal float followTime;
    [SerializeField] internal float damagingTime;
    [SerializeField] internal float damage;
    [SerializeField] internal float deg;
    [SerializeField] internal GameObject turret;
    [SerializeField] internal bool isFollowing;
    Laser laser;
    void Start()
    {
        laser = GetComponentInChildren<Laser>();
        p = GameObject.Find("Player");
        StartCoroutine(FollowOrDamage());
    }
    float GetAngleBetweenVectors(Vector3 sniper, Vector3 player)
    {
        Vector3 direction = player - sniper;
        float angle = Vector3.Angle(Vector3.right, direction);
        return angle;
    }
    void Update()
    {
        if (isFollowing)
        {
            FollowingLaser();
        }
    }
    void FollowingLaser()
    {
        if (p.transform.position.y > transform.position.y)
        {
            deg = -GetAngleBetweenVectors(p.transform.position, gameObject.transform.position);
            float rad = deg * Mathf.Deg2Rad;
            turret.transform.localPosition = new Vector2(Mathf.Cos(rad) * -0.5f, Mathf.Sin(rad) * -0.5f);
        }
        else if (p.transform.position.y < transform.position.y)
        {
            deg = GetAngleBetweenVectors(p.transform.position, gameObject.transform.position);
            float rad = deg * Mathf.Deg2Rad;
            turret.transform.localPosition = new Vector2(Mathf.Cos(rad) * -0.5f, Mathf.Sin(rad) * -0.5f);
        }
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, deg));
        turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, q, 0.01f);
    }
    IEnumerator FollowOrDamage()
    {
        while (true)
        {
            isFollowing = true;
            yield return new WaitForSeconds(followTime);
            laser.anim.SetTrigger("red");
            yield return new WaitForSeconds(0.5f);
            isFollowing = false;
            laser.anim.SetTrigger("skyBlue");
            yield return new WaitForSeconds(damagingTime);
        }
    }
}
