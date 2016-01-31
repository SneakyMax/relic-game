using System;
using System.Collections;
using System.Linq;
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
            PlayerSpawner.Enable();

            State = GameState.SpawningIn;

            ScoreController.ResetScores();
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

            StartCoroutine(PlayerWonEvents(playerNumber));

            
        }

        private IEnumerator PlayerWonEvents(int playerNumber)
        {
            PlayerSpawner.Disable();
            RelicSpawner.DespawnAndStop();

            foreach (var player in PlayerSpawner.Players.Where(x => x.PlayerNumber != playerNumber))
            {
                if (player.PlayerInstance != null)
                    player.PlayerInstance.StopInput();
            }

            yield return new WaitForSeconds(1);

            foreach (var player in PlayerSpawner.Players.Where(x => x.PlayerNumber != playerNumber))
            {
                if (player.PlayerInstance != null)
                    player.PlayerInstance.Die(RelicPlayer.DeathType.Squash);
            }

            var surviving = PlayerSpawner.Players.FirstOrDefault(x => x.PlayerInstance != null);
            if (surviving == null)
                throw new InvalidOperationException("no player left??");

            yield return new WaitForSeconds(2);

            PlayerSpawner.DespawnAllPlayers();

            yield return new WaitForSeconds(1);

            TransitionToSpawnPlayers();
        }

        private IEnumerator SpawnAfterDelay(TimeSpan delay)
        {
            yield return new WaitForSeconds((float)delay.TotalSeconds);

            TransitionToSpawnPlayers();
        }
    }
}