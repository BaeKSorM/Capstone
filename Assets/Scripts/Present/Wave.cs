using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] internal float damage;
    [SerializeField] internal float holdingTime;
    [SerializeField] internal bool isWave;
    [SerializeField] internal Animator anim;
    void Start()
    {
        Destroy(gameObject, holdingTime);
        // anim.SetTrigger("wave");
        isWave = true;
    }
    void OnDestroy()
    {
        isWave = false;
    }
}
