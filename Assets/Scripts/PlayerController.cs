using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Tooltip("체력 바")]
    [SerializeField] internal Slider hpbar;
    [Tooltip("이동 속도")]
    [SerializeField] private float moveSpeed = 1.0f;
    [Tooltip("점프 힘")]
    [SerializeField] private float jumpForce = 1.0f;
    [Tooltip("감속된 점프 힘")]
    [SerializeField] private float deceleratedJumpForce = 1.0f;
    [Tooltip("더블 점프 체크")]
    [SerializeField] private bool isJumped;
    [Tooltip("점프 횟수")]
    [SerializeField] private int jumpCount;
    [Tooltip("무기")]
    [SerializeField] internal string[] weaponNames;
    [SerializeField] internal string weaponAnimName;
    [Tooltip("무기별 공격 시간")]
    [SerializeField] internal List<float> time;
    [SerializeField] private bool isCinematic = true;
    internal Animator anim;
    Rigidbody2D playerRB;
    void Awake()
    {
        instance = this;
    }
    private IEnumerator Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (isCinematic)
        {
            // FadeInOut fadeInOut = GameObject.Find("FadeIn Canvas").GetComponent<FadeInOut>();
            yield return new WaitForSeconds(1);
            isCinematic = false;
        }
    }
    private void Update()
    {
        if (!isCinematic)
        {
            Jump();
            if (Input.GetButtonDown("Fire1"))
            {
                StartCoroutine(Attack());
            }
            if (Input.GetKeyDown(KeyCode.S) && weaponNames[1] != "")
            {
                weaponNames[2] = weaponNames[0];
                weaponNames[0] = weaponNames[1];
                weaponNames[1] = weaponNames[2];
                weaponAnimName = "use" + weaponNames[1];
                anim.SetBool(weaponAnimName, false);
                weaponAnimName = "use" + weaponNames[0];
                anim.SetBool(weaponAnimName, true);
            }
        }
        ExplosionDamage(transform.position, 5f);
    }
    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            hitColliders[i].SendMessage("AddDamage");
            i++;
        }
    }
    IEnumerator Attack()
    {
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(time[0]);
        anim.SetBool("isAttack", false);
    }
    void FixedUpdate()
    {
        if (!isCinematic)
        {
            Move();
        }
    }
    private void Move()
    {
        playerRB.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, playerRB.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            if (!isJumped)
            {
                //playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                //두번 빨리 누르면 높이 점프하고 천천히 누르면 낮게 점프한다
                playerRB.velocity = Vector2.up * jumpForce;
                //두번 빨리 누르면 낮게 점프하고 천천히 누르면 높게 점프한다 like skul
                isJumped = true;
            }
            else
            {
                //playerRB.AddForce(Vector2.up * deceleratedJumpForce, ForceMode2D.Impulse);
                playerRB.velocity = Vector2.up * deceleratedJumpForce;
            }
            ++jumpCount;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumped = false;
            jumpCount = 0;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // 무기 얻기 , 이름에 넣지 않기 -> Clone(), 오브젝트 풀링으로 해결 가능
                if (weaponNames[1] == "")
                {
                    if (weaponNames[0] == "")
                    {

                    }
                    else if (weaponNames[1] == "")
                    {

                    }
                }
                else
                {

                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            if (other.name.Contains("Arrow"))
            {
                hpbar.value -= other.GetComponent<Arrow>().arrowDamage;
            }
            else if (other.transform.parent.name.Contains("Shield"))
            {
                if (other.GetComponentInParent<ShieldEnemy>().holding)
                {
                    hpbar.value -= other.GetComponentInParent<ShieldEnemy>().attackDamage;
                }
            }
            else
            {
                hpbar.value -= other.GetComponent<EnemyWeapons>().attackDamage;
            }
        }
    }
}
