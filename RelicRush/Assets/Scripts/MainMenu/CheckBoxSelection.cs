using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    [UnityComponent]
    public class CheckBoxSelection : MonoBehaviour, IMainMenuSelection
    {
        [AssignedInUnity]
        public GameObject[] ShowWhenNavigated;

        [AssignedInUnity]
        public MainMenuController Controller;

        [AssignedInUnity]
        public GameObject Check;

        [AssignedInUnity]
        public string OptionName;

        [AssignedInUnity]
        public bool Default;

        private bool value;

        [UnityMessage]
        public void Start()
        {
            value = Default;
            ValueChanged();
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
            value = !value;
            ValueChanged();
        }

        public void ValueChanged()
        {
            Check.SetActive(value);

            Controller.SetOption(OptionName, value);
        }
    }
}