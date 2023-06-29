using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [Tooltip("최대 점프 횟수")]
    [SerializeField] private int maxJumpCount;
    [Tooltip("무기")]
    [SerializeField] internal string[] weaponNames;
    [SerializeField] internal GameObject[] getWeapons;
    [SerializeField] internal GameObject dropedWeapons;
    [SerializeField] internal string weaponAnimName;
    [Tooltip("무기별 공격 시간")]
    [SerializeField] internal float time;
    [SerializeField] internal bool isCinematic = true;
    [SerializeField] internal bool bossCanMove;
    [SerializeField] internal Animator anim;
    Rigidbody2D playerRB;
    [SerializeField] internal int weaponCount;
    [SerializeField] internal bool isTouching;
    [SerializeField] internal bool dyspnoea;
    [SerializeField] internal bool oscillation;
    [SerializeField] internal float reduceDamage;
    [SerializeField] internal GameObject fadeCan;
    [SerializeField] internal GameObject bossHpbar;
    [SerializeField] internal Camera mainCam;
    [SerializeField] internal Vector3 bossReadyPos;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float dis;
    [SerializeField] internal float reduce;
    [SerializeField] internal string[] keys;
    FadeInOut fadeInOut;
    CameraManager cameraManager;
    void Awake()
    {
        instance = this;
    }
    private IEnumerator Start()
    {
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
                ViewDirection();
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), keys[5])))
                {
                    StartCoroutine(Attack());
                }
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), keys[7])) && weaponNames[1] != "")
                {
                    swapWeapons();
                }
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), keys[4])))
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
                    else if (GameManager.instance.bossDie)
                    {
                        StartCoroutine(NextStage());
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
        playerRB.velocity = Vector2.zero;
        anim.SetBool("isAttack", true);

        reduce = transform.Find(weaponNames[0]).gameObject.GetComponent<PlayerWeapons>().damage = Random.Range(getWeapons[0].GetComponent<DropedWeapons>().mindamage, getWeapons[0].GetComponent<DropedWeapons>().maxdamage);

        transform.Find(weaponNames[0]).gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        transform.Find(weaponNames[0]).gameObject.SetActive(false);
        transform.Find(weaponNames[0]).gameObject.GetComponent<PlayerWeapons>().damage = 0;
        anim.SetBool("isAttack", false);
    }
    IEnumerator BossStageOn()
    {
        //fadeinout하고 보스스테이지위치로
        isCinematic = true;
        anim.SetBool("isMove", false);
        anim.SetBool("isAttack", false);
        fadeInOut.inOrOut = FadeInOut.InOrOut.Out;
        yield return new WaitForSeconds(fadeInOut.fadeTime * 2);
        bossHpbar.SetActive(true);
        // -1을 보스 바닥 y 위치로 바꾸기
        transform.localScale = new Vector3(-1, 1);
        transform.GetChild(1).localScale = new Vector2(40, 20);
        transform.position = new Vector2(cameraManager.bossGroundCenter.x, cameraManager.bossGroundCenter.y);
        GameManager.instance.bossAppear = true;
        fadeInOut.inOrOut = FadeInOut.InOrOut.In;
        // 도망 이동 시키기
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("isMove", true);
        while (transform.position != bossReadyPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, bossReadyPos, 0.1f);
            yield return null;
        }
        transform.localScale = new Vector3(1, 1);
        bossCanMove = true;
        //fightReady 끝나는 시간
        isCinematic = false;
    }
    private void ViewDirection()
    {
        if (playerRB.velocity.x > 0.04f)
        {
            transform.localScale = new Vector2(1, 1);
        }
        if (playerRB.velocity.x < -0.04f)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }
    private void Move()
    {
        float hori = Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), keys[2])) ? 1 : Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), keys[3])) ? -1 : 0;
        playerRB.velocity = new Vector2(hori * moveSpeed, playerRB.velocity.y);
        if (playerRB.velocity.x != 0)
        {
            anim.SetBool("isMove", true);
        }
        else
        {
            anim.SetBool("isMove", false);
        }
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
        Debug.Log(transform.Find(weaponNames[0]).gameObject.name);
    }
    private void Jump()
    {
        if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), keys[6])) && jumpCount < maxJumpCount)
        {
            ++jumpCount;
            anim.SetBool("isJump", true);
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
        }
    }
    internal void Reduce()
    {
        reduceDamage = reduce;
    }
    void checkGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, dis, mask);

        if (hit)
        {
            cameraManager.camPos = transform.position.y + 1.5f - dis;//원래2.3
            isJumped = false;
            jumpCount = 0;
            anim.SetBool("isJump", false);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BottomGround"))
        {
            checkGround();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyWeapon"))
        {
            Damaged(other);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Contains("Gas"))
        {
            if (dyspnoea)
            {
                dyspnoea = false;
            }
        }
        if (other.name.Contains("Agari"))
        {
            if (dyspnoea)
            {
                dyspnoea = false;
            }
        }
        if (other.name.Contains("Wave"))
        {
            if (oscillation)
            {
                oscillation = false;
            }
        }
    }
    IEnumerator NextStage()
    {
        Debug.Log("clear");
        anim.SetTrigger("isClear");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
        GameManager.instance.GameClear();
    }
    void Damaged(Collider2D other)
    {
        if (GameManager.instance.age == GameManager.eAge.로마)
        {
            if (other.name.Contains("Arrow"))//무기이름
            {
                float aDamage = other.GetComponent<EnemyProjectile>().projectileDamage;
                hpbar.value -= (aDamage - reduceDamage) > 0 ? (aDamage - reduceDamage) : 0;
            }
            else if (other.name.Contains("Shield"))
            {
                if (other.GetComponentInParent<ShieldEnemy>().damageOn)
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
                // Debug.Log(other.name);
                float rDamage = other.GetComponentInParent<RestEnemy>().attackDamage;
                hpbar.value -= (rDamage - reduceDamage) > 0 ? (rDamage - reduceDamage) : 0;
            }
            // Debug.Log(other.name);
        }
        else if (GameManager.instance.age == GameManager.eAge.현대)
        {
            if (other.name.Contains("Shotgun"))
            {
                float shotDamage = other.GetComponentInParent<RestEnemy>().attackDamage;
                Debug.Log(shotDamage);
                hpbar.value -= (shotDamage - reduceDamage) > 0 ? (shotDamage - reduceDamage) : 0;
            }
            else if (other.name.Contains("Boss"))
            {
                float bDamage = other.GetComponentInParent<RomeBoss>().attackDamage;
                hpbar.value -= (bDamage - reduceDamage) > 0 ? (bDamage - reduceDamage) : 0;
            }
            else if (other.name.Contains("Bullet"))
            {
                float bDamage = other.GetComponentInParent<EnemyProjectile>().projectileDamage;
                hpbar.value -= (bDamage - reduceDamage) > 0 ? (bDamage - reduceDamage) : 0;
            }
            else if (other.name.Contains("Gas"))
            {
                if (other.GetComponent<Gas>().isGas && !dyspnoea)
                {
                    StartCoroutine(GDamagePerSecond(other));
                }
            }
            else if (other.name.Contains("Button"))
            {
                float bDamage = other.GetComponentInParent<LandMine>().damage;
                hpbar.value -= (bDamage - reduceDamage) > 0 ? (bDamage - reduceDamage) : 0;
            }
            else if (other.name.Contains("Aerial"))
            {
                float aDamage = other.GetComponent<AerialBomb>().damage;
                hpbar.value -= (aDamage - reduceDamage) > 0 ? (aDamage - reduceDamage) : 0;
            }
        }
        else if (GameManager.instance.age == GameManager.eAge.미래)
        {
            if (other.name.Contains("Beam"))
            {
                Laser laser = other.GetComponent<Laser>();
                float lDamage = laser.damage;
                hpbar.value -= lDamage;
            }
            else if (other.name.Contains("Drone"))
            {
                Debug.Log(1);
                if (other.GetComponent<DroneEnemy>().bombing)
                {
                    float dDamage = other.GetComponent<DroneEnemy>().damage;
                    Debug.Log(dDamage);
                    hpbar.value -= dDamage;
                }
            }
            else if (other.name.Contains("Agari"))
            {
                StartCoroutine(AgariDam(other));
            }
            else if (other.name.Contains("AIM120B"))
            {
                float aDamage = other.GetComponent<AIM120B>().damage;
                hpbar.value -= aDamage;
            }
            else if (other.name.Contains("Boss"))
            {
                Debug.Log("boss");
                if (other.GetComponent<PresentBoss>().skills == PresentBoss.eSkills.맵회전)
                {
                    Debug.Log("roll");
                    float bDamage = other.GetComponent<PresentBoss>().damage;
                    hpbar.value -= bDamage;
                }
            }
            else if (other.name.Contains("Wave"))
            {
                if (other.GetComponent<Wave>().isWave && !oscillation)
                {
                    StartCoroutine(WaveDam(other));
                }
            }
        }
        if (hpbar.value <= 0)
        {
            Debug.Log(0);
            PlayerPrefs.SetInt("SaveLevel", 0);
            StartCoroutine(UIManager.instance.loading());
        }
        PlayerPrefs.SetFloat("PlayerHp", hpbar.value);
    }
    IEnumerator AgariDam(Collider2D other)
    {
        Agaripo agaripo = other.GetComponent<Agaripo>();
        float aDamage = agaripo.damage;
        dyspnoea = true;
        while (dyspnoea)
        {
            hpbar.value -= aDamage;
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator WaveDam(Collider2D other)
    {
        Wave wave = other.GetComponent<Wave>();
        float wDamage = wave.damage;
        oscillation = true;
        while (oscillation)
        {
            hpbar.value -= wDamage;
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator GDamagePerSecond(Collider2D other)
    {
        Gas gas = other.GetComponent<Gas>();
        dyspnoea = true;
        float gDamage = gas.damage;
        while (gas.isGas && dyspnoea)
        {
            hpbar.value -= gDamage;
            yield return new WaitForSeconds(1f);
        }
    }
}