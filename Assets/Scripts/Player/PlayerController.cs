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
    [SerializeField] internal GameObject[] getWeapons;
    [SerializeField] internal string weaponAnimName;
    [Tooltip("무기별 공격 시간")]
    [SerializeField] internal float time;
    [SerializeField] internal bool isCinematic = true;
    [SerializeField] internal bool bossCanMove;
    internal Animator anim;
    Rigidbody2D playerRB;
    [SerializeField] internal int weaponCount;
    [SerializeField] internal bool isTouching;
    [SerializeField] internal float reduceDamage;
    [SerializeField] internal GameObject fadeCan;
    [SerializeField] internal GameObject rome;
    [SerializeField] internal Camera mainCam;
    [SerializeField] internal Vector3 bossReadyPos;


    FadeInOut fadeInOut;
    CameraManager cameraManager;
    RomeBoss romeBoss;
    void Awake()
    {
        instance = this;
    }
    private IEnumerator Start()
    {
        romeBoss = rome.GetComponent<RomeBoss>();
        time = transform.Find(weaponNames[0]).gameObject.GetComponent<PlayerWeapons>().time;
        playerRB = GetComponent<Rigidbody2D>();
        fadeInOut = fadeCan.GetComponent<FadeInOut>();
        cameraManager = mainCam.GetComponent<CameraManager>();
        anim = GetComponent<Animator>();
        if (isCinematic)
        {
            yield return new WaitForSeconds(fadeInOut.fadeTime);
            isCinematic = false;
        }
    }
    private void Update()
    {
        if (!isCinematic)
        {
            Jump();
            if (!anim.GetBool("isAttack"))
            {
                if (playerRB.velocity.x > 0.04f)
                {
                    transform.localScale = new Vector2(1, 1);
                }
                if (playerRB.velocity.x < -0.04f)
                {
                    transform.localScale = new Vector2(-1, 1);
                }
                if (Input.GetButtonDown("Fire1"))
                {
                    StartCoroutine(Attack());
                }
                if (Input.GetKeyDown(KeyCode.S) && weaponNames[1] != "")
                {
                    swapWeapons();
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (ReadyForBoss.instance.ready && GameManager.instance.deadCount == GameManager.instance.enemies.Count)
                    {
                        playerRB.velocity = Vector2.zero;
                        StartCoroutine(BossStageOn());
                    }
                    //f 눌러 줍기 함수
                    else if (isTouching)
                    {
                        getWeapon();
                    }
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (!isCinematic && !anim.GetBool("isAttack"))
        {
            Move();
        }
    }
    IEnumerator Attack()
    {
        anim.SetBool("isAttack", true);
        if (weaponNames[0] != "Shield")
        {
            transform.Find(weaponNames[0]).gameObject.GetComponent<PlayerWeapons>().damage = Random.Range(getWeapons[0].GetComponent<DropedWeapons>().mindamage, getWeapons[0].GetComponent<DropedWeapons>().maxdamage);
        }
        transform.Find(weaponNames[0]).gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        transform.Find(weaponNames[0]).gameObject.SetActive(false);
        reduceDamage = 0;
        transform.Find(weaponNames[0]).gameObject.GetComponent<PlayerWeapons>().damage = 0;
        anim.SetBool("isAttack", false);
    }
    IEnumerator BossStageOn()
    {
        //fadeinout하고 보스스테이지위치로
        isCinematic = true;
        fadeInOut.inOrOut = FadeInOut.InOrOut.Out;
        yield return new WaitForSeconds(fadeInOut.fadeTime * 2);
        // -1을 보스 바닥 y 위치로 바꾸기
        transform.localScale = new Vector3(1, 1);
        transform.GetChild(1).localScale = new Vector2(40, 20);
        transform.position = new Vector2(cameraManager.bossGroundCenter.x, cameraManager.bossGroundCenter.y);
        GameManager.instance.bossAppear = true;
        fadeInOut.inOrOut = FadeInOut.InOrOut.In;
        // 도망 이동 시키기
        yield return new WaitForSeconds(1.0f);
        while (transform.position != bossReadyPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, bossReadyPos, 0.01f);
            yield return null;
        }
        bossCanMove = true;
        //fightReady 끝나는 시간
        isCinematic = false;
    }
    private void Move()
    {
        playerRB.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, playerRB.velocity.y);
    }
    private void swapWeapons()
    {
        getWeapons[2] = getWeapons[0];
        weaponNames[2] = weaponNames[0];
        getWeapons[0] = getWeapons[1];
        weaponNames[0] = weaponNames[1];
        weaponNames[1] = weaponNames[2];
        getWeapons[1] = getWeapons[2];
        weaponAnimName = "use" + weaponNames[1];
        anim.SetBool(weaponAnimName, false);
        weaponAnimName = "use" + weaponNames[0];
        anim.SetBool(weaponAnimName, true);
    }
    private void getWeapon()
    {
        // 무기 없이 시작할때 추가 및 보수 필요
        // if (weaponNames[0] == "")
        // {
        //     weaponNames[0] = GameManager.instance.enemiesDropedWeapons.GetChild(weaponCount).name.Replace("(Clone)", "");
        //     weaponAnimName = "use" + weaponNames[0];
        //     anim.SetBool(weaponAnimName, true);
        //     getWeapons[0] = GameManager.instance.enemiesDropedWeapons.GetChild(weaponCount).gameObject;
        //     getWeapons[0].SetActive(false);
        // }
        // else 
        if (weaponNames[1] == "")
        {
            GameManager.instance.enemiesDropedWeapons.GetChild(weaponCount).gameObject.SetActive(false);
            weaponNames[2] = weaponNames[0];
            getWeapons[2] = getWeapons[0];
            weaponNames[0] = GameManager.instance.enemiesDropedWeapons.GetChild(weaponCount).name.Replace("(Clone)", "");
            getWeapons[0] = GameManager.instance.enemiesDropedWeapons.GetChild(weaponCount).gameObject;
            weaponNames[1] = weaponNames[2];
            getWeapons[1] = getWeapons[2];
            weaponAnimName = "use" + weaponNames[1];
            anim.SetBool(weaponAnimName, false);
            weaponAnimName = "use" + weaponNames[0];
            anim.SetBool(weaponAnimName, true);
        }
        else
        {
            getWeapons[0].transform.position = transform.position;
            getWeapons[0].SetActive(true);
            getWeapons[0] = GameManager.instance.enemiesDropedWeapons.GetChild(weaponCount).gameObject;
            getWeapons[0].SetActive(false);
            weaponAnimName = "use" + weaponNames[0];
            anim.SetBool(weaponAnimName, false);
            weaponNames[0] = GameManager.instance.enemiesDropedWeapons.GetChild(weaponCount).name.Replace("(Clone)", "");
            weaponAnimName = "use" + weaponNames[0];
            anim.SetBool(weaponAnimName, true);
        }
        time = transform.Find(weaponNames[0]).gameObject.GetComponent<PlayerWeapons>().time;
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
        if (other.gameObject.CompareTag("MidGround") || other.gameObject.CompareTag("BottomGround"))
        {
            cameraManager.camPos = transform.position.y + 2.2f;
            isJumped = false;
            jumpCount = 0;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyWeapon"))
        {
            Damaged(other);
        }
    }
    void Damaged(Collider2D other)
    {
        if (GameManager.instance.age == GameManager.eAge.로마)
        {
            if (other.name.Contains("Arrow"))
            {
                float aDamage = other.GetComponent<EnemyArrow>().arrowDamage;
                hpbar.value -= (aDamage - reduceDamage) > 0 ? (aDamage - reduceDamage) : 0;
            }
            else if (other.name.Contains("shield"))
            {
                if (other.GetComponentInParent<ShieldEnemy>().holding)
                {
                    float sDamage = other.GetComponentInParent<ShieldEnemy>().attackDamage;
                    hpbar.value -= (sDamage - reduceDamage) > 0 ? (sDamage - reduceDamage) : 0;
                }
            }
            else if (other.name.Contains("Boss"))
            {
                float bDamage = other.GetComponentInParent<RomeBoss>().attackDamage;
                hpbar.value -= (bDamage - reduceDamage) > 0 ? (bDamage - reduceDamage) : 0;
            }
            else
            {
                float rDamage = other.GetComponentInParent<RestEnemy>().attackDamage;
                hpbar.value -= (rDamage - reduceDamage) > 0 ? (rDamage - reduceDamage) : 0;
            }
        }
    }
}
