using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RomeBoss : MonoBehaviour
{
    public enum eSkills { 말_소환, 창_찌르기, 도약_찍기, 막기 };
    public eSkills skills;
    [Tooltip("체력 바")]
    [SerializeField] internal Slider hpbar;
    void Start()
    {
        StartCoroutine(Boss());
    }
    IEnumerator Boss()
    {
        while (hpbar.value > 0)
        {
            yield return new WaitForSeconds(0);
            switch (Random.Range(0, 4))
            {
                case 0:
                    SpawnHorses();
                    break;
                case 1:
                    Spear_Poking();
                    break;
                case 2:
                    Leaping_Attack();
                    break;
                case 3:
                    Defending();
                    break;
                    // case 4:
                    // break;
            }
            // 말은 하단이나 상단 전체
            // 창찌르기는 보스 앞 거리 일부 강한 공격
            // 도약 찍기는 도약후 떨어질위치는 특정 시간 플레이어위 떨어질 자리 표시
            // 막기는 몇초간의 막기 막을때마다 소리
        }
        yield return null;
    }

    void SpawnHorses()
    {

    }
    void Spear_Poking()
    {

    }
    void Leaping_Attack()
    {

    }
    void Defending()
    {

    }
}
