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
            if(Input.GetKeyDown(KeyCode.A))
                ApplyShake(Random.Range(0.0f, 1), TimeSpan.FromSeconds(0.5));
        }

        public void ApplyShake(float factor, TimeSpan duration)
        {
            startShakeTime = Time.time;
            StartCoroutine(ShakeCoroutine(factor, duration));
        }

        public IEnumerator ShakeCoroutine(float factor, TimeSpan duration)
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

                var movement = new Vector3(Mathf.Cos(angle), -Mathf.Sin(angle), 0) * amount;

                transform.localPosition = cameraBasePosition + movement;

                yield return null;
            }
        }
    }
}