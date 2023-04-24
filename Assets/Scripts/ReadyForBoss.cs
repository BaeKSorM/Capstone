using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyForBoss : MonoBehaviour
{
    public static ReadyForBoss instance;
    [SerializeField] internal bool ready;
    void Awake()
    {
        instance = this;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ready = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ready = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
