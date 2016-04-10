using Assets.Scripts.GameStates;
using Prime31.StateKit;
using UnityEngine;
//using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public enum GameMode
    {
        Score,
        Time
    }

    public class GameStateController : MonoBehaviour
    {
        public static GameStateController Instance { get; private set; }

        private SKStateMachine<GameStateController> stateMachine;

        public GameMode CurrentGameMode { get; private set; }

        public void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            stateMachine = new SKStateMachine<GameStateController>(this, new OnMainMenu());

            stateMachine.addState(new InGame());
            stateMachine.addState(new LevelChange());
            stateMachine.addState(new PlayerWon());
            stateMachine.addState(new ReadyingUp());
            stateMachine.addState(new StartCountdown());
        }

        public T Transition<T>() where T : SKState<GameStateController>
        {
            return stateMachine.changeState<T>();
        }

        public void SetGameMode(GameMode mode)
        {
            CurrentGameMode = mode;
        }
    }
}