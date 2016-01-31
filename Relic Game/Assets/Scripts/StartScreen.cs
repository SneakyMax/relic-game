using UnityEngine;
using System.Collections;

namespace Assets.Scripts {
	public class StartScreen : MonoBehaviour {

		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
			if(Input.anyKeyDown)
				GameStateController.GameState = GameState.READY_UP;
		}
	}
}