using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>The relic when the player is holding it.</summary>
    public class HoldingRelic : MonoBehaviour
    {
        public RelicSpawner Spawner;

        public RelicPlayer RelicPlayer { get; set; }

        public void DropAtPlayer()
        {
            Spawner.SpawnAsDropFromPlayer(RelicPlayer);
        }

        public void RemoveAndRespawn()
        {
            Spawner.RemoveAndSpawnNewRelic(this);
        }
    }
}