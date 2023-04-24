using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropWeapons : MonoBehaviour
{
    public static DropWeapons instance;

    [Tooltip("드랍할것인가?")]
    [SerializeField] internal bool isDrop;
    [Tooltip("떨어뜨릴 무기")]
    [SerializeField] internal GameObject dropWeapon;
    [Tooltip("떨어뜨린 무기")]
    [SerializeField] internal GameObject dropedWeapon;
    [Tooltip("떨어뜨린 무기들 담을 곳")]
    [SerializeField] internal GameObject dropedWeapons;
    [SerializeField] internal int num;
    [SerializeField] internal Slider hpbar;
    void Start()
    {

    }
    void Update()
    {
        if (hpbar.value <= 0)
        {
            if (isDrop)
            {
                dropedWeapon = Instantiate(dropWeapon, transform.position, Quaternion.identity);
                dropedWeapon.GetComponent<DropedWeapons>().num = GameManager.instance.dropedDeadCount++;
                dropedWeapon.transform.SetParent(dropedWeapons.transform);
            }
            ++GameManager.instance.deadCount;
            Destroy(gameObject);
        }
    }

}