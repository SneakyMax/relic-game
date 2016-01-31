using UnityEngine;
using System.Collections;

public class leverActionScript : MonoBehaviour {

	public AnimationCurve leverFlipCurve;
	public bool leverLeft = false;
	public float flipDegree = 20.0f;
	public float duration = 0.0f;
	
	private float timer = 0.0f;
	private float left_rot;
	private float right_rot;
	
	void Start()
	{
		left_rot = flipDegree;
		right_rot = -flipDegree;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float rotVal = 0.0f;
		if (leverLeft && timer >= 0.0f)
		{
			timer -= Time.deltaTime;
			if (timer < 0.0f) 
			{
				timer = 0.0f;			
				LeverFlip();
			}
		}
		else if (!leverLeft && timer <= duration)
		{
			timer += Time.deltaTime;
			if (timer > duration) timer = duration;
		}
		float t = timer / duration;
		t = leverFlipCurve.Evaluate(t);
		rotVal = right_rot + ( t * ( left_rot - right_rot ) );
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotVal);
	}
	
	public void LeverFlip()
	{
		leverLeft = !leverLeft;
	}
}
