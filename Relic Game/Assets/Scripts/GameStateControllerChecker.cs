using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Assets.Scripts
{
    public class GameStateControllerChecker : MonoBehaviour
    {
        public GameStateController GameStateControllerPrefab;

        public void Awake()
        {
            if (GameStateController.Instance == null)
            {
                Instantiate(GameStateControllerPrefab);

                Debug.Assert(GameStateController.Instance != null, "GameStateController.Instance != null");
                GameStateController.Instance.SetGameMode(GameMode.Score, 3);
            }
        }
    }
}