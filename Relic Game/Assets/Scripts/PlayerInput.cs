using UnityEngine;
using System.Collections;

namespace Assets.Scripts {

	public class InputState {
		public int x;
		public int y;
		bool a;
	}

	public class PlayerInput : MonoBehaviour {

		private int playerNumber = -1;

		public PlayerInput(int playerNumber) {
			this.playerNumber = playerNumber;
		}
	}
}
