using UnityEngine;

namespace Assets.Scripts
{
    public class GameStateController : MonoBehaviour
    {
        public RelicSpawner RelicSpawner;

        public void Start()
        {
            RelicSpawner.RemoveAndSpawnNewRelic();
        } 
    }
}