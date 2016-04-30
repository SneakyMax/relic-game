using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    [UnityComponent]
    public class ChangeAreaSelection : MonoBehaviour, IMainMenuSelection
    {
        [AssignedInUnity]
        public MainMenuArea Area;

        [AssignedInUnity]
        public GameObject[] ShowWhenNavigated;

        [AssignedInUnity]
        public MainMenuController Controller;

        [UnityMessage]
        public void Awake()
        {
            NavigatedAwayFrom();
        }

        public void NavigatedTo()
        {
            foreach (var obj in ShowWhenNavigated)
            {
                obj.SetActive(true);
            }
        }

        public void NavigatedAwayFrom()
        {
            foreach (var obj in ShowWhenNavigated)
            {
                obj.SetActive(false);
            }
        }

        public void Selected()
        {
            Controller.ChangeArea(Area);
        }
    }
}