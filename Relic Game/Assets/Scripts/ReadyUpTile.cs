using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts {
	public class ReadyUpTile : MonoBehaviour {

		private Text text;
		public int PlayerNumber = -1;
		public bool ready = false;

		void Start () {
			text = GetComponentInChildren<Text>();
			if(PlayerNumber == -1) {
				throw new InvalidOperationException("PlayerNumber on ready up screen not set");
			}
		}

		void Update () {
			if(Input.GetButtonUp("buttonA" + PlayerNumber)) {
				ready = !ready;
				if(ready) {
					Vector3 pos = GetComponentInParent<RectTransform>().anchoredPosition;
					pos.y = 100;
					GetComponentInParent<RectTransform>().anchoredPosition = pos;
					text.text = "Press A to leave";
				} else {
					Vector3 pos = GetComponentInParent<RectTransform>().anchoredPosition;
					pos.y = 0;
					GetComponentInParent<RectTransform>().anchoredPosition = pos;
					text.text = "Press A to join";
				}
			}
		}
	}
}