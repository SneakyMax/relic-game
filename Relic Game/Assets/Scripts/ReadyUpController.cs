using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ReadyUpController : MonoBehaviour
    {
        public event Action EveryoneReady;

        public Canvas GUICanvas;

        public RectTransform ReadyUpTextPrefab;

        public PlayersDefinition PlayersDefinition;

        public PlayerSpawnPoint[] SpawnPoints;

        public GameObject PortalPrefab;

        public int MinPlayers = 2;

        private IDictionary<int, bool> readyStates;

        private IDictionary<int, GameObject> portals = new Dictionary<int, GameObject>();

        private GameObject readyUpTextInstance;

        private bool acceptInput;

        public void Awake()
        {
            readyStates = new Dictionary<int, bool>();

            if (PlayersDefinition == null)
                throw new InvalidOperationException("Missing players definition.");

            foreach (var player in PlayersDefinition.Players)
            {
                readyStates[player.PlayerNumber] = false;
                portals[player.PlayerNumber] = null;
            }
        }

        public void StartController()
        {
            acceptInput = true;

            readyUpTextInstance = Instantiate(ReadyUpTextPrefab.gameObject);
            readyUpTextInstance.transform.SetParent(GUICanvas.transform, false);
        }

        public void StopController()
        {
            if (readyUpTextInstance != null)
                Destroy(readyUpTextInstance);

            acceptInput = false;
        }

        public void ToggleReady(int playerNumber)
        {
            var currentReady = readyStates[playerNumber];
            ReadyUp(playerNumber, !currentReady);
        }

        public void ReadyUp(int playerNumber, bool isReady)
        {
            readyStates[playerNumber] = isReady;

            if (isReady)
            {
                var spawnPoint = SpawnPoints.FirstOrDefault(x => x.PlayerNumber == playerNumber);
                if (spawnPoint == null)
                    throw new InvalidOperationException("Missing spawn point for player " + playerNumber);

                var playerDefinition = PlayersDefinition.Players.FirstOrDefault(x => x.PlayerNumber == playerNumber);
                if (playerDefinition.PlayerNumber == 0)
                    throw new InvalidOperationException("Missing player definition for player " + playerNumber);

                var portal = Instantiate(PortalPrefab);
                portal.transform.position = spawnPoint.transform.position;

                var portalScript = portal.GetComponent<SpiralParticleSpawner>();
                if (portalScript != null)
                    portalScript.SetColor(playerDefinition.Color);

                portals[playerNumber] = portal;
            }
            else
            {
                if (portals[playerNumber] != null)
                    Destroy(portals[playerNumber]);
            }
        }

        public void RemoveAllPortals()
        {
            foreach (var pair in portals)
            {
                if (pair.Value != null)
                    Destroy(pair.Value);
            }
        }

        public void TryStart()
        {
            if (readyStates.Count(x => x.Value) < MinPlayers)
                return;

            foreach (var pair in readyStates)
            {
                GameStateController.Instance.SetPlayerIn(pair.Key, pair.Value);
            }

            if (EveryoneReady != null)
                EveryoneReady();
        }

        public void Update()
        {
            if (!acceptInput)
                return;

            foreach (var definition in PlayersDefinition.Players)
            {
                if (Input.GetButtonDown("buttonA" + definition.PlayerNumber))
                {
                    ToggleReady(definition.PlayerNumber);
                }
            }

            if (Input.GetButtonDown("buttonStartAny"))
            {
                TryStart();
            }
        }
    }
}