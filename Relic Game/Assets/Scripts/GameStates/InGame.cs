using System;
using System.Collections;
using System.Linq;
using System.Threading;
using Prime31.StateKit;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class InGame : SKState<GameStateController>
    {
        private PlayerSpawner playerSpawner;
        private RelicSpawner relicSpawner;
        private ReadyUpController readyUpController;
        private ScoreController scoreController;

        private bool isOvertime;

        public override void begin()
        {
            playerSpawner = GameObject.Find("Player Spawner").GetComponent<PlayerSpawner>();
            relicSpawner = GameObject.Find("Relic Spawner").GetComponent<RelicSpawner>();
            readyUpController = GameObject.Find("Ready Up Controller").GetComponent<ReadyUpController>();
            scoreController = GameObject.Find("ScoreController").GetComponent<ScoreController>();

            playerSpawner.Enable();
            readyUpController.RemoveAllPortals();
            playerSpawner.SpawnEveryone();
            relicSpawner.SpawnRelic();
            scoreController.ResetScores();

            scoreController.ScoreChanged += ScoreControllerOnScoreChanged;

            if (_context.CurrentGameMode == GameMode.Time)
            {
                _context.StartCoroutine(Timer());
            }
        }

        public IEnumerator Timer()
        {
            yield return new WaitForSeconds(_context.CurrentGameModeArgument);

            var players = _context.GetPlayersIn();
            var scores = players.Select(x => scoreController.GetScore(x)).ToList();

            var maxScore = scores.Max();

            if (scores.Count(x => x == maxScore) > 1)
            {
                isOvertime = true;
                yield break;
            }

            var playerWithMaxScore = players.First(x => scoreController.GetScore(x) == maxScore);
            PlayerWonRound(playerWithMaxScore);
        }

        private void ScoreControllerOnScoreChanged(int playerNumber, int newScore, string pickedUpRelic)
        {
            if (_context.CurrentGameMode == GameMode.Time)
            {
                if (isOvertime == false)
                    return;

                PlayerWonRound(playerNumber);
            }

            if (newScore == _context.CurrentGameModeArgument)
            {
                PlayerWonRound(playerNumber);
            }
        }

        private void PlayerWonRound(int playerNumber)
        {
            _context.PlayerThatWonLast = playerNumber;
            _context.Transition<PlayerWon>();
        }

        public override void update(float deltaTime)
        {
        }
    }
}