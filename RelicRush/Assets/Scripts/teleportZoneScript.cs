using UnityEngine;
using System.Collections;

public class teleportZoneScript : MonoBehaviour {

	public Transform offset;

	private void Start()
	{
		if(offset == null)
			offset = GetComponentInChildren<Transform> ();
		if (offset == null) 
		{
			Debug.Log ("teleportZoneScript does not have an offset child");
			Destroy (gameObject);
		}
	}
}