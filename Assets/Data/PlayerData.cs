using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct stateData
{
    public float stateCd;
    public float stateCdCounter;
    public float stateKeeptime;
    public float stateKeeptimeCounter;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerDataScriptableObject", order = 1)]
public class PlayerData : ScriptableObject
{
    public float xMax;

    public float moveSpeed;
    public float jumpSpeed;
    public float dashFrameMoveTimes;

    public Queue<Vector2> pastLocal = new Queue<Vector2>();
    public float dashBackTime;
    public float queueTime;

    public int playerDamege;
    public bool canMove;
    public bool canDash;
    public bool canAttack;
    public bool dashing;
    public Vector2 unitDash;

    public int enemyHp;
    public stateData dash;
    public stateData attack;
}