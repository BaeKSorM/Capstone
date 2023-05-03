using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : PlayerWeapons
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyWeapon") && PlayerController.instance.weaponNames[0] == "Shield")
        {
            PlayerController.instance.reduceDamage = Random.Range(PlayerController.instance.getWeapons[0].GetComponent<DropedWeapons>().mindamage, PlayerController.instance.getWeapons[0].GetComponent<DropedWeapons>().maxdamage);

            if (other.name.Contains("Arrow"))
            {
                Destroy(other.gameObject);
            }
            else if (other.name.Contains("shield") && other.gameObject.GetComponentInParent<ShieldEnemy>().holding)
            {
                other.gameObject.SetActive(false);
            }
            else if (!other.name.Contains("shield"))
            {
                // 방패병 방패 사라짐 안사라지게 하기
                other.gameObject.SetActive(false);
            }
        }
    }
}
