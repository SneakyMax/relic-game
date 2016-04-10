using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class RoundTimer : MonoBehaviour
    {
        public GameObject TimerTextContainer;

        public Text TimerText;

        private int timeLeft;

        private Coroutine timer;

        public void Start()
        {
            TimerTextContainer.SetActive(false);

            GameStateController.Instance.StateChanged += InstanceOnStateChanged;
        }

        private void InstanceOnStateChanged(string newState)
        {
            if (newState != "InGame")
            {
                TimerTextContainer.SetActive(false);
                if (timer != null)
                {
                    StopCoroutine(timer);
                    timer = null;
                }
                return;
            }

            if (GameStateController.Instance.CurrentGameMode != GameMode.Time)
                return;

            TimerTextContainer.SetActive(true);

            var timeStart = GameStateController.Instance.CurrentGameModeArgument;

            timeLeft = timeStart;

            timer = StartCoroutine(DoTimer());
        }

        private IEnumerator DoTimer()
        {
            while (true)
            {
                TimerText.text = timeLeft.ToString();
                yield return new WaitForSeconds(1);
                if(timeLeft > 0)
                    timeLeft--;
            }
        }

        public void OnDestroy()
        {
            GameStateController.Instance.StateChanged -= InstanceOnStateChanged;
        }
    }
}