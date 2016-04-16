using System.Collections;
using Prime31.StateKit;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class StartScene : SKState<GameStateController>
    {
        public override void begin()
        {
            _context.Transition<ReadyingUp>();
        }

        public override void update(float deltaTime)
        {
            
        }
    }
}