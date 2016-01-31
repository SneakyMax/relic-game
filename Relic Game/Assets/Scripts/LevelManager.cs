using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public float SecondsToHoldSelect = 2;

        public int ReadyUpLevelId;

        public int[] LevelIds;

        private Queue<int> levelQueue;
        private Random random;

        private float secondsSelectHeld;

        public void Start()
        {
            var existingManager = GameObject.Find("LevelManager");
            if (existingManager != null && existingManager != gameObject)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            levelQueue = new Queue<int>();
            random = new Random();

            QueueAll();
        }

        private void QueueAll()
        {
            foreach (var level in LevelIds.OrderBy(x => random.Next(10000)))
            {
                levelQueue.Enqueue(level);
            }
        }

        public void NextRandomLevel()
        {
            if (levelQueue.Any() == false)
                QueueAll();

            var level = levelQueue.Dequeue();

#pragma warning disable 618
            Application.LoadLevel(level);
#pragma warning restore 618
        }

        public void Update()
        {
            if (Input.GetButton("AnySelect"))
            {
                secondsSelectHeld += Time.deltaTime;
            }
            else
            {
                secondsSelectHeld = 0;
            }

            if (secondsSelectHeld >= SecondsToHoldSelect)
            {
                Application.LoadLevel(ReadyUpLevelId);
            }
        }
    }
}