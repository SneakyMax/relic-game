using UnityEngine;
using System.Collections;

public class PlatformMove : MonoBehaviour {

	public Vector3 Pos1 = new Vector3(-1, 0, 0);
	public Vector3 Pos2 = new Vector3(1, 0, 0);

	public AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

	[Range(0.0f, 5.0f)]
	public float speed  = 0.5f;

	public float percent;
	public int direction = 1;

	void Start ()
	{

	}

	void FixedUpdate()
	{
		float val = animCurve.Evaluate (percent);

		percent += Time.deltaTime * speed;

		if(direction == 1)
			transform.position = (Vector3.Lerp(Pos1, Pos2, val));
		else
			transform.position = (Vector3.Lerp(Pos2, Pos1, val));

		if (percent > 1) 
		{
			percent = 0;
			direction = -direction;
		}

		/*if (percent > 1) 
		{
			percent = 1;
			direction = -1;
		}
		//lerp transition for smooth transition
		if (percent < 0) 
		{
			percent = 0;
			direction = 1;
		}*/
	}
}