using System;
using System.Collections;
using UnityEngine;
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
        
        public void Start()
        {
            if (RelicPrefab == null)
                throw new InvalidOperationException("No relic prefab");
        }
        
        public void SpawnRelic()
        {
            if (CurrentRelic != null)
                return; // Only one relic at a time

            var position = SpawnLocations.Length == 0 ? new Vector3() :
                SpawnLocations[Random.Range(0, SpawnLocations.Length)].position;

            var newObj = (GameObject)Instantiate(RelicPrefab.gameObject, position, Quaternion.identity);
            var relic = newObj.GetComponent<Relic>();

            relic.RandomImpulse();
            CurrentRelic = relic;
        }

        public void RemoveAndSpawnNewRelic()
        {
            if (CurrentRelic != null)
                Destroy(CurrentRelic.gameObject);

            StartCoroutine(SpawnRelicAfterDelay(TimeSpan.FromSeconds(RelicSpawnDelay)));
        }

        private IEnumerator SpawnRelicAfterDelay(TimeSpan spawnDelay)
        {
            yield return new WaitForSeconds((float)spawnDelay.TotalSeconds);
            SpawnRelic();
        }
    }
}