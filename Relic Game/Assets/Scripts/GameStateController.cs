using UnityEngine;

namespace Assets.Scripts
{
	public enum GameState {
		START,
		READY_UP,
		ACTIVE_GAME,
		RESTART,
		PAUSE
	}

	public delegate void GameStateChanged(GameState oldState, GameState newState);

    public class GameStateController : MonoBehaviour
    {
        public RelicSpawner RelicSpawner;

		public event GameStateChanged stateChanged;

		private GameState gameState = GameState.START;

		public void Awake() {

			DontDestroyOnLoad(gameObject); //keeps the state controller alive across all scenes (i think)
		}

        public void Start()
        {
			addStateChangeListeners ();

            //RelicSpawner.RemoveAndSpawnNewRelic();
        }

		private void addStateChangeListeners() {
			stateChanged += (GameState oldState, GameState newState) => {
				if(oldState == GameState.START && newState == GameState.READY_UP) {
					Application.LoadLevel(1);
				} else if(oldState == GameState.READY_UP && newState == GameState.ACTIVE_GAME) {

				}
			};
		}

		public GameState GameState {
			get {
				return gameState;
			}
			set {
				if(value == gameState)
					return;
				stateChanged(gameState, value);
				gameState = value;
			}
		}
    }
}