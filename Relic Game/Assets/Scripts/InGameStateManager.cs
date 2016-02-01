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
            SetUpPlayers();
            
            TransitionToSpawnPlayers();
            
            ScoreController.ScoreChanged += HandleScoreChanged;
        }

        private void SetUpPlayers()
        {
            var prefsObj = GameObject.Find("Main Scene Preferences");

            if (prefsObj != null)
            {
                var prefs = prefsObj.GetComponent<MainScenePreferences>();
                foreach (var pair in prefs.PlayersIn)
                {
                    if (pair.Value)
                    {
                        PlayerSpawner.AddPlayer(pair.Key);
                    }
                }
                Destroy(prefsObj);
            }
            else
            {
                PlayerSpawner.AddPlayer(1);
                PlayerSpawner.AddPlayer(2);
                PlayerSpawner.AddPlayer(3);
                PlayerSpawner.AddPlayer(4);
            }
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

        private void HandleScoreChanged(int playerNumber, int newScore, string pickedUpRelic)
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
            
            yield return new WaitForSeconds(2);

            PlayerSpawner.DespawnAllPlayers();

            yield return new WaitForSeconds(1);

            TransitionToSpawnPlayers();
        }

        private void NextLevel()
        {
            var levelManager = GameObject.Find("LevelManager");

            if (levelManager == null)
            {
                TransitionToSpawnPlayers();
                return;
            }

            levelManager.GetComponent<LevelManager>().NextRandomLevel();
        }

        private IEnumerator SpawnAfterDelay(TimeSpan delay)
        {
            yield return new WaitForSeconds((float)delay.TotalSeconds);

            TransitionToSpawnPlayers();
        }
    }
}