using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        public GameObject[] Selections;

        private bool axisIsReset = true;

        private int currentSelectionIndex;

        private IList<IMainMenuSelection> selections = new List<IMainMenuSelection>();

        public void Start()
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

        public void Update()
        {
            var y = Input.GetAxis("Any Y Axis");

            if (Mathf.Abs(y) > 0.7)
            {
                if (axisIsReset)
                {
                    var isUp = y > 0;
                    if (isUp)
                    {
                        PreviousSelection();
                    }
                    else
                    {
                        NextSelection();
                    }
                }

                axisIsReset = false;
            }
            else
            {
                axisIsReset = true;
            }

            if (Input.GetButtonDown("Any A Button"))
            {
                selections[currentSelectionIndex].Selected();
            }
        }

        private void PreviousSelection()
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

        private void NextSelection()
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