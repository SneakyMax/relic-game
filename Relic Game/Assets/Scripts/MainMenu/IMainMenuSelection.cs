namespace Assets.Scripts.MainMenu
{
    public interface IMainMenuSelection
    {
        void NavigatedTo();

        void NavigatedAwayFrom();

        void Selected();
    }
}