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
        private Animator animator;

        public GameObject PastPlayer;

        private float nowSpeed;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            animator = GetComponent<Animator>();
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
                    AttackTrigger();
                    DashTrigger();
                    JumpManager();
                    _rigidbody2D.velocity = new Vector2(
                        GetSpeedByRLKey(),
                        _rigidbody2D.velocity.y
                    );
                    if (_rigidbody2D.velocity.x > 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else if (_rigidbody2D.velocity.x < 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }

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
                    state = PlayerState.Idle;
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

        void AttackTrigger()
        {
            if (GetAttackKey() && playerData.stateCheck.Attack)
            {
                animator.SetInteger("State", 1);
                state = PlayerState.Attack;
                playerData.stateCheck.Attack = false;
            }
            else
            {
                if (playerData.attackData.CdCounter > playerData.attackData.Cd)
                {
                    playerData.stateCheck.Attack = true;
                }

                if (!playerData.stateCheck.Attack)
                {
                    playerData.attackData.CdCounter += Time.deltaTime;
                }
            }
        }

        private bool GetDashKey()
        {
            return Input.GetKey(KeyCode.K);
        }

        private bool GetAttackKey()
        {
            return Input.GetKey(KeyCode.J);
        }

        private void JumpManager()
        {
            var jumpData = playerData.jumpData;

            var canJump = playerData.stateCheck.Jump;
            var isPressJump = GetJumptKey();
            var isForceTimeOverflow = jumpData.nowForceTime > jumpData.maxForceTime;
            var isJumping = jumpData.nowForceTime != 0;
            var isColddown = jumpData.CdCounter > jumpData.Cd;

            var isJumpingTimeOverflow = canJump && isPressJump && onGround && isForceTimeOverflow;
            var isStartJump = canJump && isPressJump && onGround;
            var isCancel = canJump && !isPressJump && isJumping;
            var isCooldowning = !canJump && isColddown;
            var isColddowned = canJump && isColddown;

            if (isJumpingTimeOverflow || isCancel)
            {
                // Jump End
                playerData.jumpData.nowForceTime = 0;
                playerData.stateCheck.Jump = false;
                onGround = false;
                return;
            }

            if (isStartJump)
            {
                playerData.jumpData.nowForceTime += Time.deltaTime;
                _rigidbody2D.velocity += Vector2.up;
                return;
            }

            if (isCooldowning)
            {
                playerData.stateCheck.Jump = true;
                playerData.jumpData.CdCounter = 0;
                return;
            }

            if (!canJump)
            {
                playerData.jumpData.CdCounter += Time.deltaTime;
            }
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