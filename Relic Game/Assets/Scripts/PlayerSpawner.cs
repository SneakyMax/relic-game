using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerSpawner : MonoBehaviour
    {
        public PlayersDefinition PlayersDefinition;

        public IList<PlayerInfo> Players { get; private set; }

        public PlayerSpawnPoint[] PlayerSpawnPoints;

        public RelicPlayer PlayerPrefab;

        [Range(0, 10)]
        public float RespawnDelay = 3;
        
        public void Awake()
        {
            if (PlayersDefinition == null)
                throw new InvalidOperationException("no players definition");

            Players = new List<PlayerInfo>();

            if (PlayerSpawnPoints.Length == 0)
                throw new InvalidOperationException("Need to add player spawn points.");
        }

        public void Start()
        {
            
        }

        public void AddPlayer(int playerNumber)
        {
            if (Players.Any(x => x.PlayerNumber == playerNumber))
                throw new InvalidOperationException("Player has already been added.");

            Players.Add(new PlayerInfo
            {
                PlayerNumber = playerNumber
            });
        }

        public void SpawnAfterDelay(int playerNumber, TimeSpan? delay = null)
        {
            StartCoroutine(SpawnAfterDelayCoroutine(playerNumber, delay ?? TimeSpan.FromSeconds(RespawnDelay)));
        }

        private IEnumerator SpawnAfterDelayCoroutine(int playerNumber, TimeSpan delay)
        {
            yield return new WaitForSeconds((float) delay.TotalSeconds);
            Spawn(playerNumber);
        }

        public void Spawn(int playerNumber)
        {
            var player = Players.FirstOrDefault(x => x.PlayerNumber == playerNumber);

            if (player == null)
                throw new InvalidOperationException("Player " + playerNumber + " hasn't been added!");

            var spawnPoint = PlayerSpawnPoints.FirstOrDefault(x => x.PlayerNumber == playerNumber);
            if (spawnPoint == null)
            {
                spawnPoint = PlayerSpawnPoints.First();
            }

            var playerInstance = (GameObject)Instantiate(PlayerPrefab.gameObject, spawnPoint.transform.position, Quaternion.identity);
            var relicPlayer = playerInstance.GetComponent<RelicPlayer>();

            relicPlayer.PlayerNumber = playerNumber;
            relicPlayer.PlayerInfo = player;

            player.PlayerInstance = relicPlayer;
            player.Spawner = this;
        }

        public void SpawnEveryone()
        {
            foreach (var player in Players)
            {
                Spawn(player.PlayerNumber);
            }
        }

        public void DespawnAllPlayers()
        {
            foreach (var player in Players)
            {
                if (player.PlayerInstance != null)
                {
                    Destroy(player.PlayerInstance);
                    player.PlayerInstance = null;
                }
            }
        }

        public void Despawn(int playerNumber)
        {
            var info = Players.FirstOrDefault(x => x.PlayerNumber == playerNumber);
            if (info == null)
                throw new InvalidOperationException("wrong player number");

            if (info.PlayerInstance != null)
            {
                Destroy(info.PlayerInstance.gameObject);

                info.PlayerInstance = null;
            }
        }
    }

    public class PlayerInfo
    {
        public RelicPlayer PlayerInstance { get; set; }

        public int PlayerNumber { get; set; }

        public PlayerSpawner Spawner { get; set; }
    }
}