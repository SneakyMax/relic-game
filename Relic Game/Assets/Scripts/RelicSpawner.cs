using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class RelicSpawner : MonoBehaviour
    {
        public Relic CurrentRelic { get; set; }

        public Transform[] SpawnLocations;

        public Relic RelicPrefab;

        [Range(0, 5)]
        public float RelicSpawnDelay;

        private Coroutine spawnAfterDelayCoroutine;
        public HoldingRelic HoldingRelicPrefab;

        public void Start()
        {
            if (RelicPrefab == null)
                throw new InvalidOperationException("No relic prefab");

            if (HoldingRelicPrefab == null)
                throw new InvalidOperationException("No holding relic prefab");
        }
        
        public void SpawnRelic()
        {
            var position = SpawnLocations.Length == 0 ? new Vector3() :
                SpawnLocations[Random.Range(0, SpawnLocations.Length)].position;

            SpawnRelicAtPosition(position);
        }

        public void SpawnRelicAtPosition(Vector3 position)
        {
            if (CurrentRelic != null)
                return; // Only one relic at a time

            var newObj = (GameObject)Instantiate(RelicPrefab.gameObject, position, Quaternion.identity);
            var relic = newObj.GetComponent<Relic>();
            relic.Spawner = this;

            relic.RandomImpulse();
            CurrentRelic = relic;
            

            relic.DelayBeingAbleToBePickedUp();
        }

        public void RemoveAndSpawnNewRelic()
        {
            DespawnRelic();
            SpawnRelicAfterRespawnDelay();
        }

        private void SpawnRelicAfterRespawnDelay()
        {
            spawnAfterDelayCoroutine = StartCoroutine(SpawnRelicAfterDelayCoroutine(TimeSpan.FromSeconds(RelicSpawnDelay)));
        }

        private IEnumerator SpawnRelicAfterDelayCoroutine(TimeSpan spawnDelay)
        {
            yield return new WaitForSeconds((float)spawnDelay.TotalSeconds);
            SpawnRelic();
        }

        public void DespawnAndStop()
        {
            DespawnRelic();
            if (spawnAfterDelayCoroutine != null)
            {
                StopCoroutine(spawnAfterDelayCoroutine);
            }
        }

        public void DespawnRelic()
        {
            if (CurrentRelic != null)
            {
                Destroy(CurrentRelic.gameObject);
                CurrentRelic = null;
            }
        }

        public void SpawnAsDropFromPlayer(RelicPlayer relicPlayer)
        {
            SpawnRelicAtPosition(relicPlayer.transform.position);
        }

        public void RemoveAndSpawnNewRelic(HoldingRelic holdingRelic)
        {
            Destroy(holdingRelic);
            SpawnRelicAfterRespawnDelay();
        }
    }
}