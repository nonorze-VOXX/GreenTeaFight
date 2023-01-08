using System.Data;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Player.stateMechine
{
    public class Dash : StateMachineBehaviour
    {
        public PlayerData data;
        public GameObject player;
        public Rigidbody2D playerRigidBody;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            playerRigidBody.gravityScale = 0;
            data.dash.stateCdCounter = 0;
            data.dash.stateKeeptimeCounter = 0;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            playerRigidBody.gravityScale = 1;
            data.dashing = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            if (data.dash.stateKeeptimeCounter <= data.dash.stateKeeptime)
            {
                player.transform.position += (Vector3)data.unitDash;
            }
            else
            {
                animator.SetInteger("State", (int)PlayerManager.PlayerState.Idle);
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