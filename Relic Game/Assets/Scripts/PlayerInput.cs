using UnityEngine;
using System.Collections;

/// WARNING WARNING WARNING
//don't use this it's pointless, I just left it so everyone can see how to do input stuff
/// WARNING WARNING WARNING
namespace Assets.Scripts {

	public class InputState {
		public float x;
		public float y;
		public bool aButton;
		public bool xButton;
	}

	public class PlayerInput : MonoBehaviour {

		public int playerNumber = 1;
		public InputState inputState = new InputState();

		void Update() {
			inputState.x = Input.GetAxis("Horizontal" + playerNumber);
			inputState.y = Input.GetAxis("Vertical" + playerNumber);
			inputState.aButton = Input.GetButton("buttonA" + playerNumber);
			inputState.xButton = Input.GetButton("buttonX" + playerNumber);
		}
	}
}