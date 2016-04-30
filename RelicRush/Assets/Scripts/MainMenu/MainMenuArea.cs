using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    [UnityComponent]
    public class MainMenuArea : MonoBehaviour
    {
        [AssignedInUnity]
        public GameObject[] Selections;

        [AssignedInUnity]
        public GameObject Container;

        private int currentSelectionIndex;

        private readonly IList<IMainMenuSelection> selections = new List<IMainMenuSelection>();

        public MainMenuController Parent { get; set; }

        public void Show()
        {
            Container.SetActive(true);

            while (currentSelectionIndex != 0)
                PreviousSelection();

            selections[currentSelectionIndex].NavigatedTo();
        }

        public void Hide()
        {
            Container.SetActive(false);
        }

        [UnityMessage]
        public void Awake()
        {
            if (Selections.Length == 0)
            {
                throw new InvalidOperationException("MISSING SELECTIONS");
            }

            foreach (var selectionObj in Selections)
            {
                var selection = selectionObj.GetComponents<MonoBehaviour>().OfType<IMainMenuSelection>().FirstOrDefault();
                selections.Add(selection);
            }
            
            foreach (var selection in selections)
            {
                selection.NavigatedAwayFrom();
            }

            currentSelectionIndex = 0;
            selections[currentSelectionIndex].NavigatedTo();
        }

        public void MakeSelection()
        {
            selections[currentSelectionIndex].Selected();
        }

        public void PreviousSelection()
        {
            selections[currentSelectionIndex].NavigatedAwayFrom();

            if (currentSelectionIndex == 0)
            {
                currentSelectionIndex = Selections.Length - 1;
            }
            else
            {
                currentSelectionIndex--;
            }

            selections[currentSelectionIndex].NavigatedTo();
        }

        public void NextSelection()
        {
            selections[currentSelectionIndex].NavigatedAwayFrom();

            if (currentSelectionIndex == Selections.Length - 1)
            {
                currentSelectionIndex = 0;
            }
            else
            {
                currentSelectionIndex++;
            }

            selections[currentSelectionIndex].NavigatedTo();
        }
    }
}