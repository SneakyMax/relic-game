using UnityEngine;

namespace Assets.Scripts
{
    public class DropPoint : MonoBehaviour
    {
        public RelicSpawner RelicSpawner;

        public ScoreController ScoreController;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Relic") == false)
                return;

            RelicSpawner.RemoveAndSpawnNewRelic();
        }
    }
}