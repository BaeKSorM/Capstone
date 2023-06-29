using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneEnemy : Creature
{
    [SerializeField] internal bool isDamaged;
    [SerializeField] internal bool bombing;
    [SerializeField] internal float damage;
    [SerializeField] internal float saveDamage;
    [SerializeField] bool inside;
    void Awake()
    {
        anim = GetComponent<Animator>();
        saveDamage = damage;
    }
    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AttackSight") && !isDamaged)
        {
            inside = true;
            anim.SetBool("detected", true);
            // 공격범위에 들어옴;
            while (Vector2.Distance(transform.position, other.transform.parent.position) > range && !isAttack && inside)
            {
                transform.localScale = new Vector3(transform.position.x > other.transform.position.x ? 1 : -1, 1, 1);
                transform.position = Vector2.MoveTowards(transform.position, other.transform.position, 0.01f);
                yield return null;
            }
            if (Vector2.Distance(transform.position, other.transform.parent.position) < range)
            {
                StartCoroutine(SuicideBombing());
            }
        }
        Debug.Log(other.tag);
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
        isAttack = true;
        anim.SetTrigger("bombReady");
        yield return new WaitForSeconds(time);
        int layerNumber = LayerMask.NameToLayer("EnemyWeapon");
        gameObject.layer = layerNumber;
        damage = saveDamage;
        bombing = true;
        anim.SetTrigger("bomb");
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(false);
    }
}
