using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerScoreContainer : MonoBehaviour
    {
        public PlayersDefinition PlayersDefinition;

        public PlayerSpawner PlayerSpawner;

        public ScoreController ScoreController;

        public int PlayerNumber;

        public GameObject ItemPrefab;

        private Stack<GameObject> items;

        public Sprite EgyptContainer;
        public Sprite MayanContainer;

		public void Awake()
		{
		    items = new Stack<GameObject>();

		    ScoreController.ScoreChanged += HandleScoreChange;

			if (ScoreController == null || PlayersDefinition == null || ItemPrefab == null || PlayerSpawner == null || EgyptContainer == null || MayanContainer == null)
				throw new InvalidOperationException("Incorrectly configured.");
		}

        public void Start()
        {
            SetBackgroundColor();

            var playerInfo = PlayerSpawner.Players.FirstOrDefault(x => x.PlayerNumber == PlayerNumber);
            if (playerInfo == null)
            {
                gameObject.SetActive(false);
            }

            var levelManager = GameObject.Find("LevelManager");
            if (levelManager == null)
            {
                SetMayan();
            }
            else
            {
                if (levelManager.GetComponent<LevelManager>().CurrentLevelIsEgypt)
                {
                    SetEgypt();
                }
                else
                {
                    SetMayan();
                }
            }
        }

        public void SetMayan()
        {
            transform.FindChild("Background").GetComponent<Image>().overrideSprite = MayanContainer;
        }

        public void SetEgypt()
        {
            transform.FindChild("Background").GetComponent<Image>().overrideSprite = EgyptContainer;
        }

        private void HandleScoreChange(int playerNumber, int newScore)
        {
            if (playerNumber != PlayerNumber)
                return;

            while (items.Count != newScore)
            {
                if (items.Count > newScore)
                {
                    var item = items.Pop();
                    Destroy(item);
                }
                else
                {
                    var item = Instantiate(ItemPrefab);
                    items.Push(item);

                    item.transform.SetParent(transform.FindChild("Items"), false);
                }
            }
        }

        private void SetBackgroundColor()
        {
            var playerDefinition = PlayersDefinition.Players.FirstOrDefault(x => x.PlayerNumber == PlayerNumber);

            var color = playerDefinition.Color;
            var transparentColor = new Color(color.r, color.g, color.b, 0.3f); 

            transform.FindChild("Background").GetComponent<Image>().color = transparentColor;
        }
    }
}