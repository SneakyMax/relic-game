using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameStates;
using Prime31.StateKit;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

//using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    [Serializable]
    public enum GameMode
    {
        Score,
        Time
    }

    [Serializable]
    public struct LevelInfo
    {
        public SceneAsset Scene;
        public bool IsEgyptLevel;
    }

    public class GameStateController : MonoBehaviour
    {
        public event Action<int> LevelLoaded;

        public LevelInfo[] Scenes;

        public SceneAsset MainMenu;

        public static GameStateController Instance { get; private set; }

        private SKStateMachine<GameStateController> stateMachine;

        public GameMode CurrentGameMode;

        public bool IsResetReady { get; private set; }

        public string CurrentState;

        public IDictionary<int, bool> PlayersIn { get; private set; }

        public LevelInfo CurrentLevel { get; set; }

        public int CurrentGameModeArgument;
        public int PlayerThatWonLast { get; set; }

        public void Awake()
        {
            PlayersIn = new Dictionary<int, bool>();
            IsResetReady = true;

            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Start()
        {
            stateMachine = new SKStateMachine<GameStateController>(this, new NoState());

            stateMachine.addState(new InGame());
            stateMachine.addState(new LevelChange(Scenes));
            stateMachine.addState(new PlayerWon());
            stateMachine.addState(new ReadyingUp());
            stateMachine.addState(new StartCountdown());
            stateMachine.addState(new OnMainMenu());
            stateMachine.addState(new StartScene());
            stateMachine.addState(new PostMainMenu());

            if (SceneManager.GetActiveScene().name == MainMenu.name)
            {
                Transition<OnMainMenu>();
            }
            else
            {
                Transition<ReadyingUp>();
            }
        }

        public T Transition<T>() where T : SKState<GameStateController>
        {
            var newStateName = typeof (T).Name;
            CurrentState = newStateName;

            return stateMachine.changeState<T>();
        }

        public void SetGameMode(GameMode mode, int argument)
        {
            CurrentGameMode = mode;
            CurrentGameModeArgument = argument;
        }

        public void SetNoOneReady()
        {
            IsResetReady = true;
            PlayersIn = new Dictionary<int, bool>();
        }

        public IList<int> GetPlayersIn()
        {
            return PlayersIn.Where(x => x.Value).Select(x => x.Key).ToList();
        }

        public void SetPlayerIn(int playerNumber, bool isIn)
        {
            PlayersIn[playerNumber] = isIn;
        }

        public void UnsetNoOneReady()
        {
            IsResetReady = false;
        }

        public void Update()
        {
            stateMachine.update(Time.deltaTime);
        }

        public void OnLevelWasLoaded(int level)
        {
            if (LevelLoaded != null)
                LevelLoaded(level);
        }
    }
}