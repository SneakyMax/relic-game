using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    [Serializable]
    public struct LevelInfo
    {
        public int LevelId;
        public bool IsEgyptLevel;
    }

    public class LevelManager : MonoBehaviour
    {
        public float SecondsToHoldSelect = 2;

        public int ReadyUpLevelId;

        public LevelInfo[] Levels;

        private Queue<LevelInfo> levelQueue;
        private Random random;

        private float secondsSelectHeld;

        public LevelInfo CurrentLevel { get; private set; }

        public bool CurrentLevelIsEgypt
        {
            get { return CurrentLevel.IsEgyptLevel; }
        }

        public void Start()
        {
            var existingManager = GameObject.Find("LevelManager");
            if (existingManager != null && existingManager != gameObject)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            levelQueue = new Queue<LevelInfo>();
            random = new Random();

            QueueAll();
        }

        private void QueueAll()
        {
            foreach (var level in Levels.OrderBy(x => random.Next(10000)))
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
            Application.LoadLevel(level.LevelId);
#pragma warning restore 618

            CurrentLevel = level;
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
#pragma warning disable 618
                Destroy(GameObject.Find("Main Scene Preferences"));
                Application.LoadLevel(ReadyUpLevelId);
#pragma warning restore 618
            }
        }
    }
}