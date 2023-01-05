using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManagerold : MonoBehaviour
{
    enum PlayerState
    {
        Idle = 0,
        Walk = 1,
        Run = 2,
        Attack = 3,
        Dash = 4
    }

    PlayerState nowPlayerState = PlayerState.Idle;

    //public PlayerAttack attack;
    private Rigidbody2D Rigidbody;
    private bool TouchGround;
    private bool Dashing;
    private int DashCounter;
    private Vector2 DashTarget;
    public GameObject PastPlayer;

    public PlayerData data;

    //public GameObject attack;
    public bool attacked;

    public Animator playerAction;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
        playerAction = gameObject.GetComponent<Animator>();
        NewGame();
        data.pastLocal.Enqueue(new Vector2(
            transform.position.x,
            transform.position.y
        ));
    }

    // Update is called once per frame
    void Update()
    {
        // old
        if (data.queueTime < data.dashBackTime * 2) //- data.dashCd)
        {
            data.queueTime += Time.deltaTime;
            data.pastLocal.Enqueue(new Vector2(
                transform.position.x,
                transform.position.y
            ));
        }
        else
        {
            data.pastLocal.Enqueue(new Vector2(
                transform.position.x,
                transform.position.y
            ));
            PastPlayer.transform.position = data.pastLocal.Dequeue();
        }

        playerAction.SetInteger("State", (int)nowPlayerState);
        switch (nowPlayerState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Walk:
                break;
            case PlayerState.Run:
                break;
            case PlayerState.Attack:
                Attack();
                nowPlayerState = PlayerState.Idle;
                /*
                if ( == playerAction.GetCurrentAnimatorStateInfo(0))
                {
                    nowPlayerState = PlayerState.Idle;

                }
                */
                break;
            case PlayerState.Dash:
                Rigidbody.gravityScale = 0;
                //Debug.Log(DashTarget);
                if (DashCounter >= data.dashFrameMoveTimes)
                {
                    Rigidbody.gravityScale = 1;
                    Dashing = false;
                    nowPlayerState = PlayerState.Idle;
                    DashCounter = 0;
                }
                else
                {
                    Dash(DashTarget);
                    DashCounter++;
                }

                break;
        }

        AllInput();
    }

    private void Dash(Vector2 DashTarget)
    {
        transform.position = (Vector2)transform.position + DashTarget;
    }

    private void AllInput()
    {
        if (!Dashing)
        {
            if (data.canMove)
            {
                if (Input.GetKey("d") && Input.GetKey("a"))
                {
                    Move(0.0f, Rigidbody.velocity.y);
                    //Rigidbody.velocity = new Vector2(0.0f, Rigidbody.velocity.y);
                }
                else if (Input.GetKey("d") && transform.position.x < data.xMax)
                {
                    Move(data.moveSpeed, Rigidbody.velocity.y);
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    //Rigidbody.velocity = new Vector2(data.MoveSpeed, Rigidbody.velocity.y);
                }
                else if (Input.GetKey("a") && transform.position.x > -data.xMax)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    Move(-data.moveSpeed, Rigidbody.velocity.y);
                    //Rigidbody.velocity = new Vector2(-data.MoveSpeed, Rigidbody.velocity.y);
                }
                else
                {
                    Move(0.0f, Rigidbody.velocity.y);
                    //Rigidbody.velocity = new Vector2(0.0f, Rigidbody.velocity.y);
                }

                if (Input.GetKey("w") && TouchGround)
                {
                    Move(Rigidbody.velocity.x, data.jumpSpeed);
                    //Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, data.JumpSpeed);
                    TouchGround = false;
                }

                if (Input.GetKey("j"))
                {
                    nowPlayerState = PlayerState.Attack;
                    attacked = false;
                    //attack.SetBool("Attack", true);
                }

                if (Input.GetKey("k") && Dashing == false)
                {
                    DashTarget = (data.pastLocal.Dequeue() - (Vector2)transform.position) / data.dashFrameMoveTimes;
                    Dashing = true;
                    nowPlayerState = PlayerState.Dash;
                    //attack.SetBool("Attack", true);
                }
            }
        }
        else
        {
        }
    }

    private void Move(float x, float y)
    {
        Rigidbody.velocity = new Vector2(x, y);
        //
    }

    public void Attack()
    {
    }

    public void NewGame()
    {
        gameObject.transform.Translate(0.0f, 0.0f, 0.0f);
        Dashing = false;
        data.pastLocal.Clear();
        data.queueTime = 0;
        TouchGround = false;
        nowPlayerState = PlayerState.Idle;
        DashCounter = 0;
        attacked = false;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            TouchGround = true;
        }
    }

    public void gameover()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (attacked == false
            && collision.gameObject.CompareTag("enemy")
            && collision.gameObject.transform.rotation == this.transform.rotation)
        {
            data.enemyHp -= data.playerDamege;
            attacked = true;
            if (data.enemyHp <= 0)
            {
                gameover();
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        Debug.Log(collision.gameObject.tag);
        if (attacked == false && collision.gameObject.CompareTag("enemy"))
        {
            data.EnemyHP -= data.PlayerDamege;
            attacked = true;
            if (data.EnemyHP <= 0)
            {
                gameover();
            }
        }
        */
    }
}