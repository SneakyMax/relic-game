using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Blink : MonoBehaviour
    {
        public float OnTime = 1;
        public float OffTime = 1;

        public GameObject Target;

        private Coroutine blinking;

        public void Awake()
        {
            blinking = StartCoroutine(DoBlink());
        }

        public IEnumerator DoBlink()
        {
            while (true)
            {
                Target.SetActive(true);
                yield return new WaitForSeconds(OnTime);
                Target.SetActive(false);
                yield return new WaitForSeconds(OffTime);
            }
        }

        public void TurnOff()
        {
            if(blinking != null)
                StopCoroutine(blinking);
            Target.SetActive(false);
        }
    }
}