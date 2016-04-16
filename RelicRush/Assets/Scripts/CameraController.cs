using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        private float startShakeTime;
        private Vector3 cameraBasePosition;

        public void Start()
        {
            cameraBasePosition = transform.localPosition;
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

            transform.localPosition = cameraBasePosition + movement;

            yield return null;

            transform.localPosition = cameraBasePosition;
        }

        private IEnumerator ShakeCoroutine(float factor, TimeSpan duration)
        {
            while (true)
            {
                var elapsed = TimeSpan.FromSeconds(Time.time - startShakeTime);
                var percentThrough = (float)elapsed.TotalMilliseconds / (float)duration.TotalMilliseconds;

                if (percentThrough >= 1)
                {
                    transform.localPosition = cameraBasePosition;
                    break;
                }

                var amount = factor * (1.0f - percentThrough);
                var angle = Random.Range(0, Mathf.PI * 2);

                var movement = new Vector3(Mathf.Cos(angle), -Mathf.Sin(angle), 0) * amount / 5;

                transform.localPosition = cameraBasePosition + movement;

                yield return null;
            }
        }
    }
}