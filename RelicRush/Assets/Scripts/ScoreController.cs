using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public delegate void PlayerScoreChanged(int playerNumber, int newScore, string pickedUpRelic);

    public class ScoreController : MonoBehaviour
    {
        public event PlayerScoreChanged ScoreChanged;

        public PlayersDefinition PlayersDefinition;

        private IDictionary<int, int> playerScores;

        private IList<AudioSource> audioSources;
        private AudioSource currentMusic;

        public void Awake()
        {
            if (PlayersDefinition == null)
                throw new InvalidOperationException("Must set players definition.");

            playerScores = new Dictionary<int, int>();

            foreach (var player in PlayersDefinition.Players)
            {
                playerScores[player.PlayerNumber] = 0;
            }

            audioSources = GetComponents<AudioSource>();
        }
        public void Start()
        {
            PlayMusic("rushIG_(NORELIC)");
        }

        public int GetScore(int playerNumber)
        {
            if (playerScores.ContainsKey(playerNumber) == false)
                throw new InvalidOperationException("No player number " + playerNumber);

            return playerScores[playerNumber];
        }

        public void AddScore(int playerNumber, int score, string pickedUpRelic)
        {
            if (playerScores.ContainsKey(playerNumber) == false)
                throw new InvalidOperationException("No player " + playerNumber);

            playerScores[playerNumber] += score;

            if(ScoreChanged != null)
                ScoreChanged(playerNumber, playerScores[playerNumber], pickedUpRelic);

            var maxScore = playerScores.Max(x => x.Value);

            switch(maxScore)
            {
                case 1:
                    PlayMusic("rushIG_(1RELIC)");
                    break;
                case 2:
                    PlayMusic("rushIG_(2RELIC)");
                    break;
                case 4:
                    PlayMusic("rushIG_(END)");
                    break;
                case 5:
                    PlayMusic("rushIG_(FIN)");
                    break;
            }
        }

        public void PlayMusic(string name)
        {
            if (currentMusic != null)
                currentMusic.Stop();
            currentMusic = GetAudio(name);
            GetAudio(name).Play();
        }

        public AudioSource GetAudio(string name)
        {
            return audioSources.FirstOrDefault(x => x.clip.name == name);
        }

        public void ResetScores()
        {
            foreach (var pair in playerScores.ToList())
            {
                playerScores[pair.Key] = 0;

                if (ScoreChanged != null)
                    ScoreChanged(pair.Key, 0, null);
                PlayMusic("rushIG_(NORELIC)");
            }
        }
    }
}