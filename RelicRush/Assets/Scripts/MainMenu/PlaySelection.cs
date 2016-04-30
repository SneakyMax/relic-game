using Assets.Scripts.GameStates;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenu
{
    [UnityComponent]
    public class PlaySelection : MonoBehaviour, IMainMenuSelection
    {
        [AssignedInUnity]
        public GameObject[] Arrows;

        [AssignedInUnity]
        public Text NumberText;

        private bool isNavigatedTo;
        private bool axisIsReset = true;

        [AssignedInUnity]
        public int CounterValue = 3;

        [AssignedInUnity]
        public int CounterIncrement = 1;

        [AssignedInUnity]
        public bool IsFirstToScore;
        
        [UnityMessage]
        public void Start()
        {
            SetCounter(CounterValue);
        }

        public void NavigatedTo()
        {
            isNavigatedTo = true;

            foreach (var arrow in Arrows)
            {
                arrow.SetActive(true);
            }
        }

        public void NavigatedAwayFrom()
        {
            isNavigatedTo = false;

            foreach (var arrow in Arrows)
            {
                arrow.SetActive(false);
            }
        }

        [UnityMessage]
        public void Update()
        {
            if (!isNavigatedTo)
                return;

            var x = Input.GetAxis("Any X Axis");

            if (Mathf.Abs(x) > 0.7)
            {
                if (axisIsReset)
                {
                    var isLeft = x < 0;
                    if (isLeft)
                    {
                        DecrementCounter();
                    }
                    else
                    {
                        IncrementCounter();
                    }
                }

                axisIsReset = false;
            }
            else
            {
                axisIsReset = true;
            }
        }

        private void IncrementCounter()
        {
            SetCounter(CounterValue + CounterIncrement);
        }

        private void DecrementCounter()
        {
            if (CounterValue - CounterIncrement <= 0)
                return;
            SetCounter(CounterValue - CounterIncrement);
        }

        private void SetCounter(int num)
        {
            NumberText.text = num.ToString();
            CounterValue = num;
        }

        public void Selected()
        {
            var controller = GameStateController.Instance;
            controller.SetGameMode(IsFirstToScore ? GameMode.Score : GameMode.Time, CounterValue);
            controller.SetOptions(MainMenuController.Instance.Options);
            controller.Transition<PostMainMenu>();
        }
    }
}