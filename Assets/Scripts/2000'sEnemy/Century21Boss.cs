using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Century21Boss : MonoBehaviour
{
    public static Century21Boss instance;
    public enum eSkills { 포탄발사, 독가스투척, 지뢰투적, 항공기탑재폭탄 };
    public eSkills skills;
    [Tooltip("체력 바")]
    [SerializeField] internal Slider hpbar;
    [SerializeField] internal Animator anim;

    [Tooltip("공격 데미지")]
    [SerializeField] internal float attackDamage = 2.5f;
    [SerializeField] internal bool skillEnd;
    [Tooltip("공격 하는 중인지")]
    [SerializeField] internal bool isAttack;
    [SerializeField] internal GameObject weapon;
    [SerializeField] internal GameObject cannonBall;
    [SerializeField] internal GameObject gas;
    [SerializeField] internal GameObject landMind;
    [SerializeField] internal GameObject aeriaBomb;
    [SerializeField] internal Transform[] throwPoses;
    [SerializeField] internal bool enterance;
    [SerializeField] internal Transform shootPos;
    [SerializeField] internal Transform gasMinePos;
    [SerializeField] internal Transform aeriaPos;
    [SerializeField] Vector3 bossAppear = new Vector2(23, -1.99f);
    [SerializeField] internal CameraManager cameraManager;
    [SerializeField] internal float shootDelay;
    [SerializeField] internal float damage;
    [SerializeField] internal float throwPower;
    [SerializeField] internal float waitingTime;
    [SerializeField] internal int throwCount = 5;
    Rigidbody2D bossRB;
    void Start()
    {
        bossRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // weapon = transform.GetChild(1).gameObject;
        instance = this;
    }
    void Update()
    {
        if (GameManager.instance.bossAppear && !enterance)
        {
            StartCoroutine(BossAppear());
            enterance = true;
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon") && other.name.Contains("Z"))
        {
            hpbar.value -= other.GetComponent<PlayerWeapons>().damage;
        }
        if (other.CompareTag("PlayerWeapon") && other.name == "Shield")
        {
            PlayerController.instance.Reduce();
        }
    }
    IEnumerator UseSkills()
    {
        while (hpbar.value > 0)
        {
            if (!PlayerController.instance.isCinematic)
            {
                skillEnd = false;
                // 확률도 조정해야함
                switch (Random.Range(1, 1))
                {
                    case 0:
                        StartCoroutine(ShootCannonball());
                        Debug.Log(0);
                        break;
                    case 1:
                        StartCoroutine(ThrowGas());
                        break;
                    case 2:
                        StartCoroutine(AerialBomb());
                        break;
                    case 3:
                        StartCoroutine(LandMine());
                        break;
                }
                yield return new WaitUntil(() => skillEnd);
            }
            yield return null;
        }
        GameManager.instance.bossDie = true;
        anim.SetTrigger("isDead");
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }
    internal IEnumerator BossAppear()
    {
        Physics2D.IgnoreLayerCollision(15, 8, true);
        yield return new WaitForSeconds(0.2f);
        // while (transform.position != bossAppear)
        // {
        //     transform.position = Vector2.MoveTowards(transform.position, bossAppear, 0.05f);
        //     yield return null;
        // }
        StartCoroutine(UseSkills());
    }
    IEnumerator ShootCannonball()
    {
        GameObject cannonBallClone = Instantiate(cannonBall, shootPos.position, Quaternion.identity);
        cannonBallClone.transform.parent = transform.parent;
        yield return new WaitForSeconds(shootDelay);
        skillEnd = true;
    }
    IEnumerator ThrowGas()
    {
        for (int i = 1; i <= throwCount; ++i)
        {
            int randThrow = Random.Range((i - 1) * throwPoses.Length / throwCount, i * throwPoses.Length / throwCount);
            GameObject gasClone = Instantiate(gas, gasMinePos);
            Gas gasS = gasClone.GetComponent<Gas>();
            gasS.target = throwPoses[throwPoses.Length - randThrow - 1];
            gasClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-(randThrow + 1) - (randThrow + 7) * 0.5f - (randThrow + 1) * 0.126f, 1.125f);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(waitingTime);
        skillEnd = true;
    }
    IEnumerator AerialBomb()
    {
        for (int i = 1; i <= throwCount; ++i)
        {
            int randThrow = Random.Range((i - 1) * throwPoses.Length / throwCount, i * throwPoses.Length / throwCount);
            GameObject aerialClone = Instantiate(aeriaBomb, aeriaPos);
            AerialBomb aerialBombS = aerialClone.GetComponent<AerialBomb>();
            aerialBombS.target = throwPoses[throwPoses.Length - randThrow - 1];
            aerialClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-randThrow * 0.64f - (randThrow + 1) * 0.017f, 1.125f);
            yield return new WaitForSeconds(0.7f);
        }
        yield return new WaitForSeconds(waitingTime);
        skillEnd = true;
    }
    IEnumerator LandMine()
    {
        for (int i = 1; i <= throwCount; ++i)
        {
            int randThrow = Random.Range((i - 1) * throwPoses.Length / throwCount, i * throwPoses.Length / throwCount);
            GameObject mineClone = Instantiate(landMind, gasMinePos);
            LandMine landMineS = mineClone.GetComponent<LandMine>();
            landMineS.target = throwPoses[throwPoses.Length - randThrow - 1];
            mineClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-(randThrow + 1) - (randThrow + 7) * 0.5f - (randThrow + 1) * 0.036f, 1.125f);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(waitingTime);
        skillEnd = true;
    }
}
