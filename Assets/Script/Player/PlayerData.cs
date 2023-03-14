using UnityEngine;

namespace Script.Player
{
    public struct JumpData
    {
        public float maxForceTime;
        public float nowForceTime;
        public float Cd;
        public float CdCounter;
    }

    public struct MoveData
    {
        public float speed;
    }

    public struct StateCheck
    {
        public bool Jump;
        public bool Walk;
        public bool Run;
        public bool Dash;
        public bool DashAttack;
        public bool FallingDownAttack;
        public bool Attack;
        public bool Jumping;
    }

    [CreateAssetMenu(fileName = "playerData", menuName = "playerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        public MoveData moveData;
        public StateCheck stateCheck;
        public JumpData jumpData;
    }
}