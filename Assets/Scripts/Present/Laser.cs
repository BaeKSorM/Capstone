using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] internal float damage;
    [SerializeField] internal Animator anim;
    [SerializeField] internal BoxCollider2D laserRB;
    void Awake()
    {
        laserRB = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }
}
