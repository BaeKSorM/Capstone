using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropedWeapons : MonoBehaviour
{
    public static DropedWeapons instance;
    [SerializeField] internal int num;
    [SerializeField] internal enum eTypeOfWeapon { 검, 석궁, 방패 };
    [SerializeField] internal eTypeOfWeapon typeOfWeapon;
    [SerializeField] internal string weaponName;
    [SerializeField] internal int mindamage, maxdamage;
    [SerializeField] internal TMP_Text weaponNameText;
    [SerializeField] internal TMP_Text damText;
    Canvas canvas;
    void Awake()
    {
        instance = this;
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        switch (typeOfWeapon)
        {
            case eTypeOfWeapon.검:
                mindamage = Random.Range(8, 13);
                maxdamage = Random.Range(24, 29);
                break;
            case eTypeOfWeapon.석궁:
                mindamage = Random.Range(8, 13);
                maxdamage = Random.Range(24, 29);
                break;
            case eTypeOfWeapon.방패:
                mindamage = Random.Range(8, 13);
                maxdamage = Random.Range(24, 29);
                break;
        }
        weaponName = gameObject.name;
        weaponName = weaponName.Replace("Z", "");
        weaponNameText.text = weaponName;
        damText.text = "데미지\n" + mindamage + " ~ " + maxdamage;
    }
    void Update()
    {
        transform.GetChild(0).GetChild(0).position = new Vector2(transform.position.x, transform.position.y + 1);
    }
    void Start()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GetItem"))
        {
            PlayerController.instance.isTouching = true;
            //마지막에 다인지 확인
            GameManager.instance.show_ItemInfo(num);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GetItem"))
        {
            GameManager.instance.close_ItemInfo(num);
        }
    }
}