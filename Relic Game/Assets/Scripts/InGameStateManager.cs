using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class InGameStateManager : MonoBehaviour
    {
        public enum GameState
        {
            NotStarted,
            InGame,
            PlayerWon,
            SpawningIn
        }

        public PlayerSpawner PlayerSpawner;

        public RelicSpawner RelicSpawner;

        public ScoreController ScoreController;

        public int ScoreToWin = 3;

        public GameState State { get; private set; }

        public void Awake()
        {
            State = GameState.NotStarted;            
        }

        public void Start()
        {
            // TODO temporary, this will be done on the screen where everyone readies up.
            PlayerSpawner.AddPlayer(1);
            PlayerSpawner.AddPlayer(2);
            PlayerSpawner.AddPlayer(3);
            PlayerSpawner.AddPlayer(4);

            TransitionToSpawnPlayers();
            
            ScoreController.ScoreChanged += HangleScoreChanged;
        }

        private void TransitionToSpawnPlayers()
        {
            State = GameState.SpawningIn;

            PlayerSpawner.SpawnEveryone();
            RelicSpawner.SpawnRelic();

            TransitionToInGame();
        }

        private void TransitionToInGame()
        {
            State = GameState.InGame;
        }

        private void HangleScoreChanged(int playerNumber, int newScore)
        {
            if (newScore == ScoreToWin && State == GameState.InGame)
            {
                TransitionToPlayerWon(playerNumber);
            }
        }

        private void TransitionToPlayerWon(int playerNumber)
        {
            //TODO
            State = GameState.PlayerWon;

            PlayerSpawner.DespawnAllPlayers();
            RelicSpawner.DespawnRelic();

            StartCoroutine(SpawnAfterDelay(TimeSpan.FromSeconds(3)));
        }

        private IEnumerator SpawnAfterDelay(TimeSpan delay)
        {
            yield return new WaitForSeconds((float)delay.TotalSeconds);

            TransitionToSpawnPlayers();
        }
    }
}