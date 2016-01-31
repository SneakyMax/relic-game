using UnityEngine;

namespace Assets.Scripts
{
    public class DropPoint : MonoBehaviour
    {
        public RelicSpawner RelicSpawner;

        public ScoreController ScoreController;

        public GameObject SpawnEffect;
        
        public void AcceptRelic(RelicPlayer player)
        {
            RelicSpawner.RemoveAndSpawnNewRelic();
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();
            }
            ScoreController.AddScore(player.PlayerNumber, 1);

            if (SpawnEffect != null)
                Instantiate(SpawnEffect, transform.position, Quaternion.identity);
        }
    }
}