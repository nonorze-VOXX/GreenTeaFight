using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Player
{
    public enum PlayerState
    {
        Idle = 0,
        Walk = 1,
        Run = 2,
        Dash = 3,
        DashAttack,
        FallingDownAttack,
        Attack,
        Jumping,
        Falling,
    }

    public class PlayerManager : MonoBehaviour
    {
        private PlayerState state;
        public PlayerData playerData;
        private Rigidbody2D _rigidbody2D;
        private bool onGround;
        private Vector2 deltaDash;
        private Vector2 dashTarget;


        public GameObject PastPlayer;

        private float nowSpeed;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            //tmp
            nowSpeed = 10;
            playerData.dashData.CdCounter = 0;
            playerData.jumpData.CdCounter = 0;
            //todo newgame init
        }


        private void Update()
        {
            switch (state)
            {
                case PlayerState.Idle:
                    DashTrigger();
                    JumpManager();
                    _rigidbody2D.velocity = new Vector2(
                        GetSpeedByRLKey(),
                        _rigidbody2D.velocity.y
                    );

                    break;
                case PlayerState.Walk:
                    break;
                case PlayerState.Run:
                    break;
                case PlayerState.Dash:
                    Dash();
                    if (playerData.dashData.keepCounter > playerData.dashData.maxKeepTime)
                    {
                        state = PlayerState.Idle;
                        _rigidbody2D.gravityScale = 1;
                    }

                    break;
                case PlayerState.DashAttack:

                    break;
                case PlayerState.FallingDownAttack:
                    break;
                case PlayerState.Attack:
                    break;
                case PlayerState.Jumping:
                    break;
                case PlayerState.Falling:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Dash()
        {
            playerData.dashData.keepCounter += Time.deltaTime;
            if (playerData.dashData.keepCounter > playerData.dashData.maxKeepTime)
            {
                transform.position = dashTarget;
            }
            else
            {
                transform.position += (Vector3)(deltaDash * Time.deltaTime);
            }
        }

        private void DashTrigger()
        {
            if (GetDashKey() && playerData.stateCheck.Dash)
            {
                dashTarget = PastPlayer.transform.position;
                deltaDash = (dashTarget - (Vector2)transform.position) / playerData.dashData.maxKeepTime;
                state = PlayerState.Dash;
                playerData.stateCheck.Dash = false;
                _rigidbody2D.gravityScale = 0;
                _rigidbody2D.velocity = Vector2.zero;
            }
            else
            {
                if (playerData.dashData.CdCounter > playerData.dashData.Cd)
                {
                    playerData.dashData.keepCounter = 0;
                    playerData.dashData.CdCounter = 0;
                    playerData.stateCheck.Dash = true;
                }

                if (!playerData.stateCheck.Dash)
                {
                    playerData.dashData.CdCounter += Time.deltaTime;
                }
            }
        }

        private bool GetDashKey()
        {
            return Input.GetKey(KeyCode.K);
        }

        private void JumpManager()
        {
            var jumpData = playerData.jumpData;
            switch (playerData.stateCheck.Jump)
            {
                case true when (GetJumptKey() && onGround) && (jumpData.nowForceTime > jumpData.maxForceTime):
                {
                    JumpEnd();
                    break;
                }
                case true when (GetJumptKey() && onGround):
                {
                    playerData.jumpData.nowForceTime += Time.deltaTime;
                    _rigidbody2D.velocity += Vector2.up;

                    break;
                }
                case true when (!GetJumptKey() && playerData.jumpData.nowForceTime != 0):
                {
                    JumpEnd();
                    break;
                }
                case false when playerData.jumpData.CdCounter > playerData.jumpData.Cd:
                    playerData.stateCheck.Jump = true;
                    playerData.jumpData.CdCounter = 0;
                    break;
                case false:
                    playerData.jumpData.CdCounter += Time.deltaTime;
                    break;
            }
        }

        private void JumpEnd()
        {
            playerData.jumpData.nowForceTime = 0;
            playerData.stateCheck.Jump = false;
            onGround = false;
        }

        private bool GetJumptKey()
        {
            return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W);
        }

        private float GetSpeedByRLKey()
        {
            if (GetRightKey() && GetLeftKey())
            {
                return 0;
            }
            else if (GetRightKey())
            {
                return nowSpeed;
            }
            else if (GetLeftKey())
            {
                return -nowSpeed;
            }

            return 0;
        }

        private bool GetLeftKey()
        {
            return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        }

        private bool GetRightKey()
        {
            return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            onGround = other.transform.CompareTag("ground") || onGround;
        }
    }
}