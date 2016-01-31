using UnityEngine;
using System.Collections;

namespace Assets.Scripts {
	public class ReadyScreen : MonoBehaviour {

		public PlayersDefinition defs;
		public ReadyUpTile readyUpTile1;
		public ReadyUpTile readyUpTile2;
		public ReadyUpTile readyUpTile3;
		public ReadyUpTile readyUpTile4;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			if(Input.GetButtonUp("buttonStartAny")) {
				defs.Players[0].isPlaying = readyUpTile1.ready;
				defs.Players[1].isPlaying = readyUpTile2.ready;
				defs.Players[2].isPlaying = readyUpTile3.ready;
				defs.Players[3].isPlaying = readyUpTile4.ready;
				GameStateController.GameState = GameState.ACTIVE_GAME;
			}
		}
	}
}