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
    float LR;
    Quaternion q;
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
        if (isFollowing && !GameManager.instance.pause)
        {
            FollowingLaser();
            LR = p.transform.position.x > transform.position.x ? -1 : 1;
            transform.localScale = new Vector2(LR, 1);
        }
    }
    void FollowingLaser()
    {
        if (p.transform.position.y > transform.position.y)
        {
            deg = -GetAngleBetweenVectors(p.transform.position, gameObject.transform.position);
        }
        else
        if (p.transform.position.y < transform.position.y)
        {
            deg = GetAngleBetweenVectors(p.transform.position, gameObject.transform.position);
        }
        if (LR == 1)
        {
            q = Quaternion.Euler(new Vector3(0, 0, deg % 90));
        }
        else
        {
            q = Quaternion.Euler(new Vector3(0, 0, (deg + 180) % 90));
        }
        turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, q, 0.01f);
    }
    IEnumerator FollowOrDamage()
    {
        while (true)
        {
            isFollowing = true;
            yield return new WaitForSeconds(followTime);
            laser.anim.SetTrigger("red");
            isFollowing = false;
            yield return new WaitForSeconds(0.5f);
            laser.anim.SetTrigger("skyBlue");
            yield return new WaitForSeconds(damagingTime);
        }
    }
}