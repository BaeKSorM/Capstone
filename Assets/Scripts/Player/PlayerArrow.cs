using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : PlayerWeapons
{
    void Awake()
    {
        damage = 10;
        transform.localScale = new Vector2(-1.0f, 0.2f);
    }

    void Update()
    {
        transform.right = GetComponent<Rigidbody2D>().velocity;
    }
    void Start()
    {
        Destroy(gameObject, PlayerCrossbow.instance.destroyTime);
    }

}
