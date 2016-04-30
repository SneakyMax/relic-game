using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    // Should be put on CameraShakeGimbal
    [UnityComponent]
    public class CameraShaker : MonoBehaviour
    {
        private float startShakeTime;

        public static CameraShaker Get()
        {
            return Camera.main.GetComponentInParent<CameraShaker>();
        }

        public void Update()
        {
        }

        public void TicScreen(float factor)
        {
            StartCoroutine(TicCoroutine(factor));
        }

        public void ShakeScreen(float factor, TimeSpan duration)
        {
            startShakeTime = Time.time;
            StartCoroutine(ShakeCoroutine(factor, duration));
        }

        private IEnumerator TicCoroutine(float factor)
        {
            var angle = Random.Range(0, Mathf.PI * 2);

            var movement = new Vector3(Mathf.Cos(angle), -Mathf.Sin(angle), 0) * factor;

            transform.localPosition = movement;

            yield return null;

            transform.localPosition = new Vector3();
        }

        private IEnumerator ShakeCoroutine(float factor, TimeSpan duration)
        {
            while (true)
            {
                var elapsed = TimeSpan.FromSeconds(Time.time - startShakeTime);
                var percentThrough = (float)elapsed.TotalMilliseconds / (float)duration.TotalMilliseconds;

                if (percentThrough >= 1)
                {
                    transform.localPosition = new Vector3();
                    break;
                }

                var amount = factor * (1.0f - percentThrough);
                var angle = Random.Range(0, Mathf.PI * 2);

                var movement = new Vector3(Mathf.Cos(angle), -Mathf.Sin(angle), 0) * amount / 5;

                transform.localPosition = movement;

                yield return null;
            }
        }
    }
}