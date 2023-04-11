using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapons : MonoBehaviour
{
    string weaponAnimName;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (PlayerController.instance.weaponNames[1] == "")
                {
                    if (PlayerController.instance.weaponNames[0] == "")
                    {
                        PlayerController.instance.weaponNames[0] = gameObject.name;
                        weaponAnimName = "use" + PlayerController.instance.weaponNames[0];
                        PlayerController.instance.anim.SetBool(weaponAnimName, true);
                        Debug.Log(weaponAnimName);
                    }
                    else if (PlayerController.instance.weaponNames[1] == "")
                    {
                        PlayerController.instance.weaponNames[1] = PlayerController.instance.weaponNames[0];
                        weaponAnimName = "use" + PlayerController.instance.weaponNames[1];
                        Debug.Log(weaponAnimName);
                        PlayerController.instance.anim.SetBool(weaponAnimName, false);
                        PlayerController.instance.weaponNames[0] = gameObject.name;
                        weaponAnimName = "use" + PlayerController.instance.weaponNames[0];
                        Debug.Log(weaponAnimName);
                        PlayerController.instance.anim.SetBool(weaponAnimName, true);
                    }
                }
                else
                {
                    weaponAnimName = "use" + PlayerController.instance.weaponNames[0];
                    Debug.Log(weaponAnimName);
                    PlayerController.instance.anim.SetBool(weaponAnimName, false);
                    PlayerController.instance.weaponNames[0] = gameObject.name;
                    weaponAnimName = "use" + PlayerController.instance.weaponNames[0];
                    Debug.Log(weaponAnimName);
                    PlayerController.instance.anim.SetBool(weaponAnimName, true);
                }
                Destroy(gameObject);
            }
        }
    }
}