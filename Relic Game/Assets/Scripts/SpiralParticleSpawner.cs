using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpiralParticleSpawner : MonoBehaviour
    {
        public class SpawnerInfo
        {
            public float Angle { get; set; }

            public Vector3 Position { get; set; }
        }

        private new ParticleSystem particleSystem;

        [Range(0, 5)]
        public float Radius;

        [Range(0.0f, 6f)]
        public float SpinSpeed;

        [Range(0, 8)]
        public float Count;

        [Range(0, 100)]
        public float ParticlesPerSecond;

        private IList<SpawnerInfo> spawners = new List<SpawnerInfo>();

        public void SetColor(Color color)
        {
            particleSystem.startColor = color;
            particleSystem.GetComponent<ParticleSystemRenderer>().material.color = color;
        }

        public void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();

            var angleDiff = (Mathf.PI * 2) / Count;
            for (var i = 0; i < Count; i++)
            {
                var angle = angleDiff * i;

                var info = new SpawnerInfo
                {
                    Angle = angle
                };

                spawners.Add(info);
            }
        }

        public void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                SpawnParticles();
                yield return new WaitForSeconds(1.0f / ParticlesPerSecond);
            }
        }

        public void FixedUpdate()
        {
            foreach (var info in spawners)
            {
                var angle = info.Angle;
                var newPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Radius;

                info.Position = newPosition;
                info.Angle += SpinSpeed * Time.fixedDeltaTime;
            }
        }

        public void SpawnParticles()
        {
            foreach(var info in spawners)
            {
                var worldSpacePosition = transform.position + info.Position;

                var velocity = worldSpacePosition.UnitVectorTo(transform.position) * particleSystem.startSpeed;

                var emitInfo = new ParticleSystem.EmitParams
                {
                    velocity = velocity,
                    position = new Vector3(info.Position.x, info.Position.y, -1.5f)
                };

                particleSystem.Emit(emitInfo, 1);
            }
        }
    }
}