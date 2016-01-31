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

            player.PlayerInstance = relicPlayer;
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
    }

    public class PlayerInfo
    {
        public RelicPlayer PlayerInstance { get; set; }

        public int PlayerNumber { get; set; }
    }
}