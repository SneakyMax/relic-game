using System;
using Prime31.StateKit;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class ReadyingUp : SKState<GameStateController>
    {
        private ReadyUpController readyUpController;

        public override void begin()
        {
            var readyUpControllerObj = GameObject.Find("Ready Up Controller");

            if (readyUpControllerObj == null)
                throw new InvalidOperationException("Missing ready up controller in scene.");

            readyUpController = readyUpControllerObj.GetComponent<ReadyUpController>();

            if (_context.IsResetReady == false)
            {
                foreach (var player in _context.GetPlayersIn())
                {
                    readyUpController.ReadyUp(player, true); //for portals
                }
                ReadyUpControllerOnEveryoneReady();
                return;
            }

            readyUpController.EveryoneReady += ReadyUpControllerOnEveryoneReady;
            readyUpController.StartController();
        }

        public override void end()
        {
            _context.UnsetNoOneReady();
            readyUpController.StopController();
        }

        private void ReadyUpControllerOnEveryoneReady()
        {
            var playerSpawner = GameObject.Find("Player Spawner").GetComponent<PlayerSpawner>();

            foreach (var playerIn in GameStateController.Instance.GetPlayersIn())
            {
                playerSpawner.AddPlayer(playerIn);
            }

            _context.Transition<StartCountdown>();
        }

        public override void update(float deltaTime)
        {
        }
    }
}