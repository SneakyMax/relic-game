using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ReadyUpController : MonoBehaviour
    {
        public PlayersDefinition PlayersDefinition;

        public MainScenePreferences Preferences;

        public Text NextStepText;

        public int MinPlayers = 2;

        private IDictionary<int, bool> readyStates;

        public void Start()
        {
            readyStates = new Dictionary<int, bool>();

            if (PlayersDefinition == null)
                throw new InvalidOperationException("Missing players definition.");

            if (NextStepText == null)
                throw new InvalidOperationException("Mising next step text.");

            foreach (var player in PlayersDefinition.Players)
            {
                readyStates[player.PlayerNumber] = false;
            }

            RefreshTexts();
        }

        public void ReadyUp(int playerNumber, bool isReady)
        {
            readyStates[playerNumber] = isReady;
            RefreshTexts();
        }

        private void RefreshTexts()
        {
            NextStepText.enabled = readyStates.Count(x => x.Value) >= MinPlayers;
        }

        public void TryStart()
        {
            if (readyStates.Count(x => x.Value) < MinPlayers)
                return;

            foreach (var pair in readyStates)
            {
                Preferences.PlayersIn[pair.Key] = pair.Value;
            }

            var levelManager = GameObject.Find("LevelManager");

            if (levelManager == null)
                throw new InvalidOperationException("Couldn't find level manager");

            levelManager.GetComponent<LevelManager>().NextRandomLevel();
        }
    }
}