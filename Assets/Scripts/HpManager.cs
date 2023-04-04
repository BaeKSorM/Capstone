using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpManager : MonoBehaviour
{
    [SerializeField] private Slider[] hpBar;
    [SerializeField] private string targetName;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject player;
    [SerializeField] private enum eMyRole { 적, 플레이어 };
    [SerializeField] private eMyRole myRole;

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; ++i)
        {
            StartCoroutine(EnemyHp(enemies[i], enemies[i].transform.GetChild(0).GetChild(0)));
            Debug.Log(enemies[i].transform.GetChild(0).GetChild(0).name);
        }
    }
    IEnumerator EnemyHp(GameObject enemy, Transform enemyHpBar)
    {
        while (true)
        {
            enemyHpBar.position = new Vector2(enemy.transform.position.x, enemy.transform.position.y + enemy.transform.localScale.y / 2 + 0.5f);
            yield return new WaitForSeconds(0);
        }
    }
    IEnumerator PlayerHp()
    {
        while (true)
        {

            yield return new WaitForSeconds(0.01f);
        }
    }
}
