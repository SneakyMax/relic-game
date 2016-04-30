using System;
using UnityEngine;

namespace Assets.Scripts
{
    [UnityComponent]
    public class DropPoint : MonoBehaviour
    {
        [AssignedInUnity]
        public RelicSpawner RelicSpawner;

        [AssignedInUnity]
        public ScoreController ScoreController;

        [AssignedInUnity]
        public GameObject SpawnEffect;

        [AssignedInUnity]
        public GameObject HowToPlayOverlayPrefab;

        [UnityMessage]
        public void Start()
        {
            GameStateController.Instance.StateChanged += InstanceOnStateChanged;
        }

        [UnityMessage]
        public void OnDestroy()
        {
            GameStateController.Instance.StateChanged -= InstanceOnStateChanged;
        }

        private void InstanceOnStateChanged(string newState)
        {
            if (newState != "InGame")
                return;
            
            Instantiate(HowToPlayOverlayPrefab, transform.position + HowToPlayOverlayPrefab.transform.position, Quaternion.identity);
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