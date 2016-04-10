using System;
using Prime31.StateKit;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class StartCountdown : SKState<GameStateController>
    {
        private CountdownController countdownController;

        public override void begin()
        {
            var countdownControllerObj = GameObject.Find("Countdown Controller");
            if (countdownControllerObj == null)
                throw new InvalidOperationException("Missing countdown controller");

            countdownController = countdownControllerObj.GetComponent<CountdownController>();

            countdownController.Done += CountdownControllerOnDone;
            countdownController.StartCountdown();
        }

        private void CountdownControllerOnDone()
        {
            _context.Transition<InGame>();
        }

        public override void update(float deltaTime)
        {
        }
    }
}