using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrossbow : PlayerWeapons
{
    public static PlayerCrossbow instance;
    [SerializeField] internal GameObject arrow;
    [SerializeField] internal GameObject arrowClone;
    [SerializeField] internal float fireAngle;
    [SerializeField] internal float firePower;
    [SerializeField] internal float destroyTime;
    Vector3 point;
    void Awake()
    {
        instance = this;
    }
    void OnEnable()
    {
        arrowClone = Instantiate(arrow, transform.position, Quaternion.Euler(new Vector3(0, 0, fireAngle * transform.parent.localScale.x)));
        arrowClone.GetComponent<Rigidbody2D>().velocity = arrowClone.transform.right * firePower * transform.parent.localScale.x;
        arrowClone.transform.parent = gameObject.transform.parent;
    }
}
