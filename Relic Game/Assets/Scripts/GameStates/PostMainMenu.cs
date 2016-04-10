using Prime31.StateKit;

namespace Assets.Scripts.GameStates
{
    public class PostMainMenu : SKState<GameStateController>
    {
        public override void begin()
        {
            _context.SetNoOneReady();
            _context.Transition<LevelChange>();
        }

        public override void update(float deltaTime)
        {
            
        }
    }
}