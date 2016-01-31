using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayersDefinition : MonoBehaviour
    {
        public PlayerDefinition[] Players;

		public void Start() {
			DontDestroyOnLoad(gameObject);
		}
    }

    [Serializable]
    public struct PlayerDefinition
    {
        public int PlayerNumber;

        public Color Color;

		public bool isPlaying;
    }
}