using UnityEngine;
using System.Collections;

public class multiplayerScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col )
	{
		Debug.Log(col.gameObject.name);
	}
}
