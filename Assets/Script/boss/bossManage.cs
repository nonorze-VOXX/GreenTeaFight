using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bossManage : MonoBehaviour
{
    public float speed;
    public float distance;
    public GameObject player;

    private Animator bossAnimator;
    private Transform playerT;
    private Transform bossT;
    private bool TouchGround;
    private bool leftDir;
    public PlayerData data;

    enum bossState
    {
        Idle = 0,
        Walk = 1,
        Slash = 2
    }

    bossState nowBossState;

    // Start is called before the first frame update
    void Start()
    {
        NewGame();
        bossAnimator = gameObject.GetComponent<Animator>();
        playerT = player.GetComponent<Transform>();
        bossT = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Debug.Log(bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("bossActiveEnd"));
        Debug.Log(nowBossState);
        
        */
        bossAnimator.SetInteger("state", (int)nowBossState);
        switch (nowBossState)
        {
            case bossState.Idle:
                if (TouchGround)
                    nowBossState = bossState.Walk;
                break;
            case bossState.Walk:
                if (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("bossWalk"))
                    Move();
                break;
            case bossState.Slash:
                if (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("bossActiveEnd"))
                    nowBossState = bossState.Walk;
                break;
        }
    }

    private void NewGame()
    {
        nowBossState = bossState.Idle;
        leftDir = true;
        TouchGround = false;
        data.enemyHp = 100;
    }

    private void Move()
    {
        if (bossT.position.x > playerT.position.x)
        {
            if (leftDir == false)
            {
                leftDir = true;
                bossT.Rotate(0, 180, 0);
            }

            if (bossT.position.x - playerT.position.x > distance)
                bossT.Translate(new Vector2(-speed, 0));
            else
                nowBossState = bossState.Slash;
        }
        else
        {
            if (leftDir == true)
            {
                leftDir = false;
                bossT.Rotate(0, 180, 0);
            }

            if (playerT.position.x - bossT.position.x > distance)
                bossT.Translate(new Vector2(-speed, 0));
            else
                nowBossState = bossState.Slash;
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            TouchGround = true;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
    }
}