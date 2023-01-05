using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
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
    private Vector2 UnitDash;
    public GameObject PastPlayer;
    private bool _touchGround;

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
        if (data.canMove)
        {
            GetMove();
            if (data.canDash)
            {
                GetDash();
            }

            if (data.canAttack)
            {
                GetAttack();
            }
        }

        CounterUpdate();
    }

    private void CounterUpdate()
    {
        if (!data.canAttack)
        {
            data.attack.stateCdCounter += Time.deltaTime;
            if (data.attack.stateCdCounter > data.attack.stateCd)
            {
                data.canAttack = true;
            }
        }

        if (!data.canDash)
        {
            data.dash.stateCdCounter += Time.deltaTime;
            if (data.dash.stateCdCounter > data.dash.stateCd)
            {
                data.canDash = true;
            }
        }
    }

    private void GetAttack()
    {
    }

    private void GetDash()
    {
        if (Input.GetKey("k"))
        {
            UnitDash = (data.pastLocal.Dequeue() - (Vector2)transform.position) / data.dashFrameMoveTimes;
        }
    }

    private void GetMove()
    {
        if (Input.GetKey("d") && Input.GetKey("a"))
        {
            Move(0.0f, Rigidbody.velocity.y);
        }
        else if (Input.GetKey("d") && transform.position.x < data.xMax)
        {
            Move(data.moveSpeed, Rigidbody.velocity.y);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (Input.GetKey("a") && transform.position.x > -data.xMax)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Move(-data.moveSpeed, Rigidbody.velocity.y);
        }
        else
        {
            Move(0.0f, Rigidbody.velocity.y);
        }

        // will do keep a time and  no work
        if (Input.GetKey("w") && _touchGround)
        {
            Move(Rigidbody.velocity.x, data.jumpSpeed);
            _touchGround = false;
        }
    }

    private void Move(Vector2 unitMove)
    {
        transform.position = (Vector2)transform.position + unitMove;
    }

    private void Move(float x, float y)
    {
        Rigidbody.velocity = new Vector2(x, y);
        //
    }

    public void NewGame()
    {
        gameObject.transform.Translate(0.0f, 0.0f, 0.0f);
        data.pastLocal.Clear();
        data.queueTime = 0;
        _touchGround = false;
        nowPlayerState = PlayerState.Idle;
        attacked = false;
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
            }
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            _touchGround = true;
        }
    }
}