using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Script.Player
{
    public class PastPlayer : MonoBehaviour
    {
        public PlayerData playerData;
        public GameObject player;

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (playerData.pastPlayerData.pathTime >= playerData.pastPlayerData.maxPathTime)
            {
                transform.position = playerData.pastPlayerData.path.Dequeue();
            }
            else
            {
                playerData.pastPlayerData.pathTime += Time.deltaTime;
            }

            playerData.pastPlayerData.path.Enqueue(player.transform.position);
        }

        private void Init()
        {
            playerData.pastPlayerData.path = new Queue<Vector2>();
            playerData.pastPlayerData.pathTime = 0;
        }
    }
}