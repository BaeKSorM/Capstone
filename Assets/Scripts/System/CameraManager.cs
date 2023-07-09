using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] Transform player;
    [SerializeField] internal Vector3 bossDoorFornt;
    Vector3 targetPos;
    [Tooltip("보스 스테이지 중앙 중앙")]
    [SerializeField] internal Vector3 bossGroundCenter;
    [SerializeField] internal enum eGround { under, mid };
    [SerializeField] internal eGround ground;
    [SerializeField] internal float camPos;
    [SerializeField] internal float groundPos;
    [SerializeField] internal float startX;
    [SerializeField] internal float pPos;
    [SerializeField] internal float os;
    private void FixedUpdate()
    {
        os = player.transform.position.y;
        if (player.position.x >= bossDoorFornt.x && player.position.y > bossDoorFornt.y && player.position.y < bossDoorFornt.y + 3 && !GameManager.instance.bossAppear)
        {
            if (player.position.y <= 0)
            {
                targetPos = new Vector3(bossDoorFornt.x, 0, this.transform.position.z);
            }
            else
            {
                targetPos = new Vector3(bossDoorFornt.x, player.position.y, this.transform.position.z);
            }
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.15f);
        }
        else
        if (!GameManager.instance.bossAppear)
        {
            if (ground == eGround.under)
            {
                //화면 중간보다 위일때
                if (player.position.y > camPos)
                {
                    if (player.position.x >= bossDoorFornt.x)
                    {
                        targetPos = new Vector3(bossDoorFornt.x, player.position.y, this.transform.position.z);
                    }
                    else if (player.position.x > startX)
                    {
                        targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);
                    }
                    else
                    {
                        targetPos = new Vector3(startX, player.position.y, this.transform.position.z);
                    }
                    transform.position = Vector3.Lerp(transform.position, targetPos, 0.15f);
                }
                //떨어질때
                else
                if (player.position.y < groundPos)
                {
                    Debug.Log("1234567890");
                    if (player.position.x >= bossDoorFornt.x)
                    {
                        targetPos = new Vector3(bossDoorFornt.x, player.position.y, this.transform.position.z);
                    }
                    else if (player.position.x > startX)
                    {
                        targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);
                    }
                    else
                    {
                        targetPos = new Vector3(startX, player.position.y, this.transform.position.z);
                    }
                    transform.position = Vector3.Lerp(transform.position, targetPos, 0.15f);
                }
                //아닐때
                else
                {
                    if (player.position.x >= bossDoorFornt.x)
                    {
                        targetPos = new Vector3(bossDoorFornt.x, camPos, this.transform.position.z);
                    }
                    else if (player.position.x > startX)
                    {
                        targetPos = new Vector3(player.position.x, camPos, this.transform.position.z);
                    }
                    else
                    {
                        targetPos = new Vector3(startX, camPos, this.transform.position.z);
                    }
                    transform.position = Vector3.Lerp(transform.position, targetPos, 0.15f);
                }
            }
            else if (ground == eGround.mid)
            {
                if (player.position.x >= bossDoorFornt.x)
                {
                    targetPos = new Vector3(bossDoorFornt.x, player.position.y, this.transform.position.z);
                }
                else if (player.position.x > startX)
                {
                    targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);
                }
                else
                {
                    targetPos = new Vector3(startX, player.position.y, this.transform.position.z);
                }
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.15f);
            }
        }
        else
        if (GameManager.instance.bossAppear)
        {
            targetPos = bossGroundCenter;
            Camera.main.orthographicSize = 5.0f;
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.15f);
        }
    }
}
