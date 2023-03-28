using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [Tooltip("벽에 닿았는지")]
    public bool isStop;
    Vector3 targetPos;

    void Update()
    {

    }
    private void FixedUpdate()
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
}
