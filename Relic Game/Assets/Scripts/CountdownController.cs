using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CountdownController : MonoBehaviour
    {
        public event Action Done;

        public ReadyUpController ReadyUpController;

        public RectTransform CounterPrefab;

        public Canvas GUICanvas;

        public int CountdownFrom = 3;

        private GameObject counterInstance;
        private Text counterText;

        public void StartCountdown()
        {
            counterInstance = Instantiate(CounterPrefab.gameObject);
            counterInstance.transform.SetParent(GUICanvas.transform, false);

            counterText = counterInstance.GetComponentInChildren<Text>();

            ReadyUpController.DeactivateText();

            StartCoroutine(CounterCoroutine());
        }

        private IEnumerator CounterCoroutine()
        {
            for (var i = CountdownFrom; i > 0; i--)
            {
                if (i == 3)
                {
                    ReadyUpController.CreateSpawningPortals();
                }

                SetText(i);
                yield return new WaitForSeconds(1);
            }

            if (Done != null)
                Done();

            if (counterInstance != null)
                Destroy(counterInstance);
        }

        private void SetText(int count)
        {
            counterText.text = count.ToString();
        }
    }
}