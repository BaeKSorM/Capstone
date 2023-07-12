using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : PlayerWeapons
{
    public static PlayerShoot instance;
    [SerializeField] internal GameObject projectile;
    [SerializeField] internal GameObject projectileClone;
    [SerializeField] internal float fireAngle;
    [SerializeField] internal float firePower;
    [SerializeField] internal float destroyTime;
    [SerializeField] internal Transform shootPos;
    Vector3 point;
    void Awake()
    {
        instance = this;
    }
    void OnEnable()
    {
        if (GameManager.instance.age == GameManager.eAge.로마)
        {
            Bow();
        }
        else if (gameObject.name.Contains("Rifle"))
        {
            StartCoroutine(Rifle());
        }
        else if (GameManager.instance.age == GameManager.eAge.미래)
        {

        }
    }
    void Bow()
    {
        projectileClone = Instantiate(projectile, shootPos.position, Quaternion.Euler(new Vector3(0, 0, fireAngle * transform.parent.localScale.x)));
        projectileClone.GetComponent<PlayerProjectile>().damage = damage;
        projectileClone.GetComponent<Rigidbody2D>().velocity = projectileClone.transform.right * firePower * transform.parent.localScale.x;
        projectileClone.transform.parent = shootPos;
    }
    IEnumerator Rifle()
    {
        projectileClone = Instantiate(projectile, shootPos);
        yield return new WaitForSeconds(0.1f);
        projectileClone = Instantiate(projectile, shootPos);
        yield return new WaitForSeconds(0.1f);
        projectileClone = Instantiate(projectile, shootPos);
    }
}
