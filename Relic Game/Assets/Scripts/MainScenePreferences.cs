using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class MainScenePreferences : MonoBehaviour
    {
        public void Start()
        {
            DontDestroyOnLoad(gameObject);

            PlayersIn = new Dictionary<int, bool>();
        }

        public IDictionary<int, bool> PlayersIn { get; set; }
    }
}