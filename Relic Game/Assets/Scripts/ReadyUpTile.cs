using System;
using UnityEngine;
using System.Collections;

public class ReadyUpTile : MonoBehaviour {

	public int PlayerNumber = -1;

	void Start () {
		if(PlayerNumber == -1) {
			throw new InvalidOperationException("PlayerNumber on ready up screen not set");
		}
	}

	void Update () {
		
	}
}
