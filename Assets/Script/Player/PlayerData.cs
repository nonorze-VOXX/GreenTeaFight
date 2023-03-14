using System.Collections.Generic;
using UnityEngine;

namespace Script.Player
{
    [System.Serializable]
    public struct DashData
    {
        public float maxKeepTime;
        public float keepCounter;
        public float Cd;
        public float CdCounter;
    }

    [System.Serializable]
    public struct JumpData
    {
        public float maxForceTime;
        public float nowForceTime;
        public float Cd;
        public float CdCounter;
    }

    [System.Serializable]
    public struct MoveData
    {
        public float speed;
    }

    [System.Serializable]
    public struct PastPlayerData
    {
        public Queue<Vector2> path;
        public float pathTime;
        public float maxPathTime;
    }

    [System.Serializable]
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


    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
    [System.Serializable]
    public class PlayerData : ScriptableObject
    {
        public MoveData moveData;
        public StateCheck stateCheck;
        public JumpData jumpData;
        public DashData dashData;
        public PastPlayerData pastPlayerData;
    }
}