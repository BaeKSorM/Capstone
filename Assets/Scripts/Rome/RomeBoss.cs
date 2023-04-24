using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RomeBoss : MonoBehaviour
{
    public enum eSkills { 말_소환, 창_찌르기, 도약_찍기, 막기 };
    public eSkills skills;
    [Tooltip("체력 바")]
    [SerializeField] internal Slider hpbar;
    [SerializeField] internal Animator anim;
    [SerializeField] internal GameObject[] summonEnemies;
    [SerializeField] internal Vector2[] summonPosLR;
    [SerializeField] internal float curHp;
    [SerializeField] internal float exhaustionHp;
    [SerializeField] internal Transform player;


    void Start()
    {
        StartCoroutine(Boss());
    }
    IEnumerator Boss()
    {
        while (hpbar.value > 0)
        {
            anim.SetBool("isAttack", true);
            yield return new WaitForSeconds(0);
            switch (Random.Range(0, 4))
            {
                case 0:
                    StartCoroutine(SpawnMobs());
                    Debug.Log(0);
                    break;
                case 1:
                    StartCoroutine(Spear_Poking());
                    Debug.Log(1);
                    break;
                case 2:
                    StartCoroutine(Healing());
                    Debug.Log(2);
                    break;
                case 3:
                    StartCoroutine(Crushing());
                    Debug.Log(3);
                    break;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator SpawnMobs()
    {
        anim.SetBool("spawnMobs", true);
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 5; ++i)
        {
            Instantiate(summonEnemies[Random.Range(0, 3)], summonPosLR[Random.Range(0, 2)], Quaternion.identity);
        }
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("spawnMobs", false);
    }
    IEnumerator Spear_Poking()
    {
        anim.SetBool("poking", true);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("poking", false);
    }
    IEnumerator Healing()
    {
        exhaustionHp = hpbar.value - 10;
        anim.SetBool("heal", true);
        while (hpbar.value > exhaustionHp) { }
        anim.SetBool("heal", false);
        anim.SetBool("lostHeal", true);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("lostHeal", false);

    }
    IEnumerator Crushing()
    {
        transform.localScale = transform.position.x > player.position.x ? new Vector2(1, 1) : new Vector2(-1, 1);
        anim.SetBool("crush", true);
        yield return null;
    }
}
