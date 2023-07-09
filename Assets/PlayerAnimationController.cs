using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public GameObject swordAnimation;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwordAnimation()
    {
        swordAnimation.SetActive(true);
        swordAnimation.GetComponent<Animator>().Play("AttackEffect");
    }

    public void DisableEffect()
    {
        swordAnimation.SetActive(false);
    }
}
