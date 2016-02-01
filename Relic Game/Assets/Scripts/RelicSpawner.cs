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
        public string LastRelicName { get; private set; }

        public Transform[] SpawnLocations;

        public Relic[] RelicPrefabs;

        [Range(0, 5)]
        public float RelicSpawnDelay;

        private Coroutine spawnAfterDelayCoroutine;
        public HoldingRelic HoldingRelicPrefab;

        public void Start()
        {
            /*foreach (var item in RelicPrefabs) {

			}
                throw new InvalidOperationException("No relic prefab");*/

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

			var relicRandom = RelicPrefabs[Random.Range(0, RelicPrefabs.Length)];

            var newObj = (GameObject)Instantiate(relicRandom.gameObject, position, Quaternion.identity);
            var relic = newObj.GetComponent<Relic>();
            relic.Spawner = this;
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();
            }
            relic.RandomImpulse();
            CurrentRelic = relic;

			LastRelicName = relicRandom.RelicName; //TODO

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
            if (CurrentRelic == null)
                return;

            Destroy(CurrentRelic.gameObject);
            CurrentRelic = null;
        }

        public void SpawnAsDropFromPlayer(RelicPlayer relicPlayer)
        {
            SpawnRelicAtPosition(relicPlayer.transform.position);
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();
            }
        }

        public void RemoveAndSpawnNewRelic(HoldingRelic holdingRelic)
        {
            Destroy(holdingRelic);
            SpawnRelicAfterRespawnDelay();
        }
    }
}