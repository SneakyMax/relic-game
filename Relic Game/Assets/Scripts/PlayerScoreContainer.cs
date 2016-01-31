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

		private Image relic0;

		private Image relic1;

		private Image relic2;

		public void Awake() {
			SetupScoreText ();
			Component[] comps = gameObject.GetComponentsInChildren(typeof(Image));
			foreach(Component comp in comps) {
				Image img;
				if(comp is Image)
					img = (Image) comp;
				else
					continue;
				if(img.name == "Relic0")
					relic0 = img;
				else if(img.name == "Relic1")
					relic1 = img;
				else if(img.name == "Relic2")
					relic2 = img;
			}

			if (ScoreController == null || PlayersDefinition == null || relic0 == null || relic1 == null || relic2 == null)
				throw new InvalidOperationException("Incorrectly configured.");

			relic0.enabled = false;
			relic1.enabled = false;
			relic2.enabled = false;
		}

        public void Start()
        {
            SetBackgroundColor();
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
            ScoreController.ScoreChanged += (playerNumber, score) =>
            {
                if (playerNumber == PlayerNumber) {
					Debug.Log("score changed: "+score);
					if(score == 0) {
						relic0.enabled = false;
						relic1.enabled = false;
						relic2.enabled = false;
					} else if(score == 1) {
						relic0.enabled = true;
						relic1.enabled = false;
						relic2.enabled = false;
					} else if(score == 2) {
						relic0.enabled = true;
						relic1.enabled = true;
						relic2.enabled = false;
					} else if(score == 3) {
						relic0.enabled = true;
						relic1.enabled = true;
						relic2.enabled = true;
					}
				}
            };
        }
    }
}