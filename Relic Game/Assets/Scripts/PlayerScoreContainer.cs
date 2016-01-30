using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerScoreContainer : MonoBehaviour
    {
        public PlayersDefinition PlayersDefinition;

        public ScoreController ScoreController;

        public int PlayerNumber;

        public void Start()
        {
            if (ScoreController == null || PlayersDefinition == null)
                throw new InvalidOperationException("Incorrectly configured.");

            SetBackgroundColor();

            SetupScoreText();
        }

        private void SetBackgroundColor()
        {
            var playerDefinition = PlayersDefinition.Players.FirstOrDefault(x => x.PlayerNumber == PlayerNumber);

            var color = playerDefinition.Color;
            var transparentColor = new Color(color.r, color.g, color.b, 0.3f);

            transform.FindChild("Background").GetComponent<Image>().color = transparentColor;
        }

        private void SetupScoreText()
        {
            var formatter = transform.FindChild("ScoreText").GetComponent<TextFormatter>();

            formatter.Set("sc", ScoreController.GetScore(PlayerNumber));

            ScoreController.ScoreChanged += (playerNumber, score) =>
            {
                if (playerNumber == PlayerNumber)
                    formatter.Set("sc", score);
            };
        }
    }
}