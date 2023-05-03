using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] Transform player;
    [SerializeField] Vector3 bossDoorFornt;
    Vector3 targetPos;
    Vector3 ss;
    [Tooltip("보스 스테이지 중앙 중앙")]
    [SerializeField] internal Vector3 bossGroundCenter;

    private void Update()
    {
        if (player.position.x >= bossDoorFornt.x && !GameManager.instance.bossAppear)
        {
            if (player.position.y <= 0)
            {
                targetPos = new Vector3(bossDoorFornt.x, this.transform.position.y - Mathf.Abs(transform.position.y), this.transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
            }
            else
            {
                targetPos = new Vector3(bossDoorFornt.x, player.position.y, this.transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
            }
        }
        else
        if (!GameManager.instance.bossAppear)
        {
            if (player.position.y <= 0)
            {
                if (player.position.x > 0)
                {
                    targetPos = new Vector3(player.position.x, this.transform.position.y - Mathf.Abs(transform.position.y), this.transform.position.z);
                }
                else
                {
                    targetPos = new Vector3(0, this.transform.position.y - Mathf.Abs(transform.position.y), this.transform.position.z);
                }
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
            }
            else
            {
                if (player.position.x > 0)
                {
                    targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);
                }
                else
                {
                    targetPos = new Vector3(0, player.position.y, this.transform.position.z);
                }
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
            }
        }
        else if (GameManager.instance.bossAppear)
        {
            targetPos = bossGroundCenter;
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
        }
    }
}
