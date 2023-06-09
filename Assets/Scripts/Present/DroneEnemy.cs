using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneEnemy : Creature
{
    [SerializeField] internal bool isDamaged;
    [SerializeField] internal bool bombing;
    [SerializeField] bool inside;
    [SerializeField] bool isDoing;
    [SerializeField] GameObject shield;
    void Awake()
    {
        shield = GameObject.Find("Shield");
        anim = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon") && other.name.Contains("Z"))
        {
            hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
            Damaged();
        }
        if (other.CompareTag("Player"))
        {
            if (Vector2.Distance(shield.transform.position, transform.position) < Vector2.Distance(other.transform.position, transform.position))
            {
                attackDamage = saveDamage;
                attackDamage -= attackDamage - PlayerController.instance.reduce > 0 ? PlayerController.instance.reduce : attackDamage;
            }
            else if (Vector2.Distance(shield.transform.position, transform.position) < Vector2.Distance(other.transform.position, transform.position))
            {
                attackDamage = saveDamage;
            }
        }
        Debug.Log(other.tag);
    }
    IEnumerator OntriggerStay(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackSight") && !isDamaged && !GameManager.instance.pause && !isDoing)
        {
            inside = true;
            isDoing = true;
            anim.SetBool("detected", true);
            // 공격범위에 들어옴;
            while (Vector2.Distance(transform.position, other.transform.parent.position) > range && !isAttack && inside)
            {
                transform.localScale = new Vector3(transform.position.x > other.transform.position.x ? 1 : -1, 1, 1);
                transform.position = Vector2.MoveTowards(transform.position, other.transform.position, 0.01f);
                yield return null;
            }
            isDoing = false;
            if (Vector2.Distance(transform.position, other.transform.parent.position) < range)
            {
                StartCoroutine(SuicideBombing());
            }
        }
    }
    void Damaged()
    {
        anim.SetTrigger("isDamaged");
        isDamaged = false;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackSight"))
        {
            inside = false;
            anim.SetBool("detected", false);
        }
    }
    IEnumerator SuicideBombing()
    {
        isDoing = false;
        isAttack = true;
        anim.SetTrigger("bombReady");
        yield return new WaitForSeconds(time);
        int layerNumber = LayerMask.NameToLayer("EnemyWeapon");
        gameObject.layer = layerNumber;
        attackDamage = saveDamage;
        bombing = true;
        anim.SetTrigger("bomb");
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(false);
    }
}
