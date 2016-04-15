using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class DropPoint : MonoBehaviour
    {
        public RelicSpawner RelicSpawner;

        public ScoreController ScoreController;

        public GameObject SpawnEffect;

        public GameObject HowToPlayOverlayPrefab;

        public void Start()
        {
            GameStateController.Instance.StateChanged += InstanceOnStateChanged;
        }

        public void OnDestroy()
        {
            GameStateController.Instance.StateChanged -= InstanceOnStateChanged;
        }

        private void InstanceOnStateChanged(string newState)
        {
            if (newState != "InGame")
                return;

            Instantiate(HowToPlayOverlayPrefab, transform.position, Quaternion.identity);
        }

        public void AcceptRelic(RelicPlayer player)
        {
            var relicName = RelicSpawner.LastRelicName;

            RelicSpawner.RemoveAndSpawnNewRelic();

            GeneralAudioController.PlaySound("RushShrineCollect");

            ScoreController.AddScore(player.PlayerNumber, 1, relicName);

            if (SpawnEffect != null)
                Instantiate(SpawnEffect, transform.position, Quaternion.identity);
        }
    }
}