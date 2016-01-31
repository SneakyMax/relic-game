using UnityEngine;
using System.Collections;

public class leverScript : MonoBehaviour {
	public trapScript[] triggerables;

	public bool isActive = false;

	private leverActionScript action;

	void Start()
	{
		action = GetComponentInChildren<leverActionScript>();
	}

	void Update()
	{
		if (isActive)
		{
			isActive = false;
			Activate();
		}
	}

	public void Activate()
	{
		action.LeverFlip();
		ActivateAllTraps();
	}

	private void ActivateAllTraps()
	{
		foreach (trapScript trap in triggerables) 
		{
			if(trap != null)
			{
				trap.Activate();
			}
		}
	}
}
