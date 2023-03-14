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


        public GameObject PastPlayer;

        private float nowSpeed;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            //tmp
            nowSpeed = 10;
            //todo newgame init
        }


        private void Update()
        {
            switch (state)
            {
                case PlayerState.Idle:
                    MoveManager();
                    break;
                case PlayerState.Walk:
                    break;
                case PlayerState.Run:
                    break;
                case PlayerState.Dash:
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


        private void MoveManager()
        {
            _rigidbody2D.velocity = HorizontalMoveManager();
            JumpManager();
        }

        private void JumpManager()
        {
            switch (playerData.stateCheck.Jump)
            {
                case true:
                {
                    var jumpData = playerData.jumpData;
                    if (GetJumptKey() && onGround)
                    {
                        if (jumpData.nowForceTime > jumpData.maxForceTime)
                        {
                            JumpEnd();
                        }
                        else
                        {
                            playerData.jumpData.nowForceTime += Time.deltaTime;
                            _rigidbody2D.velocity += Vector2.up;
                        }
                    }
                    else if (!GetJumptKey() && playerData.jumpData.nowForceTime != 0)
                    {
                        JumpEnd();
                    }

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
            return Input.GetKey(KeyCode.Space);
        }

        private Vector2 HorizontalMoveManager()
        {
            if (GetRightKey() && GetLeftKey())
            {
                return Vector2.zero;
            }
            else if (GetRightKey())
            {
                return new Vector2(nowSpeed, _rigidbody2D.velocity.y);
            }
            else if (GetLeftKey())
            {
                return new Vector2(-nowSpeed, _rigidbody2D.velocity.y);
            }

            return new Vector2(0, _rigidbody2D.velocity.y);
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