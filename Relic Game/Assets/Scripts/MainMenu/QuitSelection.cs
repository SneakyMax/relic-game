using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class QuitSelection : MonoBehaviour, IMainMenuSelection
    {
        public GameObject[] Arrows;

        public void NavigatedTo()
        {
            foreach (var arrow in Arrows)
            {
                arrow.SetActive(true);
            }
        }

        public void NavigatedAwayFrom()
        {
            foreach (var arrow in Arrows)
            {
                arrow.SetActive(false);
            }
        }

        public void Selected()
        {
            Application.Quit();
        }
    }
}