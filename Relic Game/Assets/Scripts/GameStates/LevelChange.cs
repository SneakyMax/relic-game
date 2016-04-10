using System;
using Prime31.StateKit;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameStates
{
    public class LevelChange : SKState<GameStateController>
    {
        private readonly LevelInfo[] allScenes;

        private readonly Queue<LevelInfo> sceneQueue = new Queue<LevelInfo>();

        public LevelChange(LevelInfo[] allLevels)
        {
            allScenes = allLevels;
        }

        public override void begin()
        {
            var level = GetNextLevel();
            _context.LevelLoaded += ContextOnLeveLoaded;
            _context.CurrentLevel = level;

            SceneManager.LoadScene(level.Scene.name);
        }

        private void ContextOnLeveLoaded(int i)
        {
            _context.LevelLoaded -= ContextOnLeveLoaded;
            _context.Transition<StartScene>();
        }

        private void PopulateQueue()
        {
            if (sceneQueue.Any())
                return;

            foreach (var scene in allScenes.OrderBy(x => Random.value))
            {
                sceneQueue.Enqueue(scene);
            }
        }

        private LevelInfo GetNextLevel()
        {
            if (sceneQueue.Any() == false)
                PopulateQueue();

            return sceneQueue.Dequeue();
        }

        public override void update(float deltaTime)
        {
        }
    }
}