using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : PlayerWeapons
{
    [SerializeField] internal Animator anim;
    [SerializeField] internal BoxCollider2D laserBC;
    [SerializeField] internal float damagingTime;
    void OnEnable()
    {
        StartCoroutine(LaserBeam());
    }
    IEnumerator LaserBeam()
    {
        anim.SetTrigger("red");
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("skyBlue");
        yield return new WaitForSeconds(damagingTime);
    }
}
