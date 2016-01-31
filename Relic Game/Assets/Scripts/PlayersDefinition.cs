using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayersDefinition : MonoBehaviour
    {
        public PlayerDefinition[] Players;
    }

    [Serializable]
    public struct PlayerDefinition
    {
        public int PlayerNumber;

        public Color Color;

		public bool isPlaying;
    }
}