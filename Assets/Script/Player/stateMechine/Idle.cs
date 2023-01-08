using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Player.stateMechine
{
    public class Idle : StateMachineBehaviour
    {
        public PlayerData data;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            Debug.Log("Idle");
            if (Input.GetKey("j") && data.canAttack)
            {
                Debug.Log("ATTACK!");
                animator.SetInteger("State", (int)PlayerManager.PlayerState.Attack);
            }

            if (data.canMove)
            {
                if (Input.GetKey("k") && data.canDash)
                {
                    animator.SetInteger("State", (int)PlayerManager.PlayerState.Dash);
                }
            }
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }
    }
}