using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public delegate void PlayerScoreChanged(int playerNumber, int newScore);

    public class ScoreController : MonoBehaviour
    {
        public event PlayerScoreChanged ScoreChanged;

        public PlayersDefinition PlayersDefinition;

        private IDictionary<int, int> playerScores;

        public void Awake()
        {
            if (PlayersDefinition == null)
                throw new InvalidOperationException("Must set players definition.");

            playerScores = new Dictionary<int, int>();

            foreach (var player in PlayersDefinition.Players)
            {
                playerScores[player.PlayerNumber] = 0;
            }
        }

        public int GetScore(int playerNumber)
        {
            if (playerScores.ContainsKey(playerNumber) == false)
                throw new InvalidOperationException("No player number " + playerNumber);

            return playerScores[playerNumber];
        }

        public void AddScore(int playerNumber, int score)
        {
            if (playerScores.ContainsKey(playerNumber) == false)
                throw new InvalidOperationException("No player " + playerNumber);

            playerScores[playerNumber] += score;

            if(ScoreChanged != null)
                ScoreChanged(playerNumber, playerScores[playerNumber]);
        }
    }
}