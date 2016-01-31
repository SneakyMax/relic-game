using UnityEngine;
//using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public enum GameState
    {
        Start,
        ReadyUp,
        ActiveGame,
        Restart,
        Pause
    }

    public delegate void GameStateChanged(GameState oldState, GameState newState);

    public class GameStateController : MonoBehaviour
    {
        public RelicSpawner RelicSpawner;

		public event GameStateChanged StateChanged;

        public GameState GameState
        {
            get { return gameState; }
            set
            {
                if(value == gameState)
                    return;

                if(StateChanged != null)
                    StateChanged(gameState, value);

                gameState = value;
            }
        }

        private GameState gameState = GameState.Start;

        public void Awake()
        {
            DontDestroyOnLoad(gameObject); //keeps the state controller alive across all scenes (i think)
        }

        public void Start()
        {
            StateChanged += HandleNewState;

            //RelicSpawner.RemoveAndSpawnNewRelic();
        }

        private void HandleNewState(GameState oldState, GameState newState)
        {
            if (oldState == GameState.Start && newState == GameState.ReadyUp)
            {
                //SceneManager.LoadScene(1);
            }
            else if (oldState == GameState.ReadyUp && newState == GameState.ActiveGame)
            {

            }
        }
    }
}