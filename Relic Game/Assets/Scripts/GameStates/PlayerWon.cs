using System.Collections;
using System.Linq;
using Prime31.StateKit;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class PlayerWon : SKState<GameStateController>
    {
        private PlayerSpawner playerSpawner;
        private RelicSpawner relicSpawner;
        private ReadyUpController readyUpController;
        private ScoreController scoreController;

        public override void begin()
        {
            playerSpawner = GameObject.Find("Player Spawner").GetComponent<PlayerSpawner>();
            relicSpawner = GameObject.Find("Relic Spawner").GetComponent<RelicSpawner>();
            readyUpController = GameObject.Find("Ready Up Controller").GetComponent<ReadyUpController>();
            scoreController = GameObject.Find("ScoreController").GetComponent<ScoreController>();

            _context.StartCoroutine(PlayerWonEvents(_context.PlayerThatWonLast));
        }

        public override void update(float deltaTime)
        {
            
        }

        public IEnumerator PlayerWonEvents(int playerNumber)
        {
            playerSpawner.Disable();
            relicSpawner.DespawnAndStop();

            foreach (var player in playerSpawner.Players.Where(x => x.PlayerNumber != playerNumber))
            {
                if (player.PlayerInstance != null)
                    player.PlayerInstance.StopInput();
            }

            yield return new WaitForSeconds(1);

            foreach (var player in playerSpawner.Players.Where(x => x.PlayerNumber != playerNumber))
            {
                if (player.PlayerInstance != null)
                    player.PlayerInstance.Die(RelicPlayer.DeathType.Squash);
            }

            yield return new WaitForSeconds(2);

            playerSpawner.DespawnAllPlayers();

            yield return new WaitForSeconds(1);

            _context.Transition<LevelChange>();
        }
    }
}