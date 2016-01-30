using UnityEngine;

namespace Assets.Scripts
{
    public class DropPoint : MonoBehaviour
    {
        public RelicSpawner RelicSpawner;

        public ScoreController ScoreController;

        public void AcceptRelic(RelicPlayer player)
        {
            RelicSpawner.RemoveAndSpawnNewRelic();
        }
    }
}