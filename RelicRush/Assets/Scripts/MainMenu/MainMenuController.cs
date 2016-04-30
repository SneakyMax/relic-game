using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    [UnityComponent]
    public class MainMenuController : MonoBehaviour
    {
        public static MainMenuController Instance { get; private set; }

        [AssignedInUnity]
        public MainMenuArea[] Areas;

        [AssignedInUnity]
        public MainMenuArea StartArea;

        private MainMenuArea currentArea;
        private bool axisIsReset;

        public IDictionary<string, object> Options { get; private set; }

        public MainMenuController()
        {
            Options = new Dictionary<string, object>();
        }

        [UnityMessage]
        public void Awake()
        {
            Instance = this;
            foreach (var area in Areas)
            {
                area.Parent = this;
            }
        }

        [UnityMessage]
        public void Start()
        {
            foreach (var area in Areas)
            {
                area.Hide();
            }

            ChangeArea(StartArea);
        }

        [UnityMessage]
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
                currentArea.MakeSelection();
            }
        }

        private void PreviousSelection()
        {
            currentArea.PreviousSelection();
        }

        private void NextSelection()
        {
            currentArea.NextSelection();
        }

        public void ChangeArea(MainMenuArea area)
        {
            if (Areas.Any(x => x == area) == false)
                throw new InvalidOperationException("Not an area.");

            if(currentArea != null)
                currentArea.Hide();

            currentArea = area;
            area.Show();
        }

        public void SetOption(string optionName, object value)
        {
            Options[optionName] = value;
        }
    }
}