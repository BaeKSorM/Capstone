using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyShieldEnemy : Creature
{
    public static ArmyShieldEnemy Instance;
    [SerializeField] internal GameObject weapon;
    [Tooltip("돌진 대기중")]
    [SerializeField] internal bool holding;
    [Tooltip("막는 중인지")]
    [SerializeField] internal bool isDefending;
    [Tooltip("막는 중일때 방패에 닿였는지")]
    [SerializeField] internal bool defend;
    [SerializeField] internal Transform enemyHpBar;
    [SerializeField] internal bool damageOn;
    [SerializeField] internal bool isDamaged;
    [SerializeField] internal int LR;

    Rigidbody2D EnemyRB;
    Canvas canvas;
    void Start()
    {
        // speed = 5.0f;
        // range = 3.0f;
        // time = 1.0f;
        // delayTime = 1.0f;
        // action = 5.0f;
        attackDamage = 2.5f;
        anim = GetComponent<Animator>();
        EnemyRB = GetComponent<Rigidbody2D>();
        enemyHpBar = transform.parent.GetChild(0).GetChild(0);
        canvas = transform.parent.GetChild(0).GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        Instance = this;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon") && other.name.Contains("Z"))
        {
            // Debug.Log(other.transform.parent.name);
            if (transform.localScale.x == other.transform.parent.localScale.x)
            {
                defend = true;
            }
            else
            {
                defend = false;
            }
            Debug.Log("Def");
            if (!defend)
            {
                Debug.Log("dam");
                LR = transform.position.x > other.transform.parent.position.x ? 1 : -1;
                Damaged();
                hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
            }
            else if (defend)
            {
                Debug.Log("shie");
            }
        }
        if (other.CompareTag("PlayerWeapon") && other.name == "Shield")
        {
            PlayerController.instance.reduceDamage = PlayerController.instance.reduce;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon") && other.name.Contains("h"))
        {
            PlayerController.instance.reduceDamage = 0;
        }
    }
    void Update()
    {
        EnemyHp(enemyHpBar);
    }
    void EnemyHp(Transform _enemyHpBar)
    {
        _enemyHpBar.position = new Vector2(transform.position.x, transform.position.y + transform.localScale.y / 2 + 0.5f);
    }
    void Damaged()
    {
        anim.SetTrigger("isDamaged");
        // EnemyRB.AddForce(new Vector2(LR * 4, 0), ForceMode2D.Impulse);
        isDamaged = false;
    }
}
