using UnityEngine;
using System.Collections;

public class trapScript : MonoBehaviour {

	public bool isTrap = false;
	public enum direction {UP, RIGHT, LEFT, DOWN};
	public direction trapDirection = direction.UP;
	public bool isCrushing = false;
	[Range(0.2f, 20.0f)]
	public float speed = 1.0f;
	[Range(0.0f, 1.0f)]
	public float snap = 0.2f;


	private Vector3 startLocation;
	private bool hasHit = false;
	private Rigidbody rigid;

	// Use this for initialization
	void Start () 
	{
		startLocation = transform.position;
		if(rigid == null)
		{
			rigid = GetComponent<Rigidbody>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(hasHit && isCrushing) return;
		if(isCrushing)
		{
			switch (trapDirection)
			{
			case direction.UP:
				Debug.Log("Case up");
				rigid.MovePosition(transform.position + (Vector3.up * speed) * Time.deltaTime);
				rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
				break;
				//move UP
			case direction.DOWN:
				Debug.Log("Case down");
				rigid.MovePosition(transform.position + (Vector3.down * speed) * Time.deltaTime);
				rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
				break;
				//move DOWN
			case direction.LEFT:
				Debug.Log("Case left");
				rigid.MovePosition(transform.position + (Vector3.left * speed) * Time.deltaTime);
				rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
				break;
				//move LEFT
			case direction.RIGHT:
				Debug.Log("Case right");
				rigid.MovePosition(transform.position + (Vector3.right * speed) * Time.deltaTime);
				rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
				break;
				//move RIGHT
			default:
				Debug.Log("Direction not set... Setting direction to UP");
				trapDirection = direction.UP;
				break;
			}
		}else if(isTrap)
		{
			//move towards "startLocation"
			Vector3 moveVec = (startLocation - transform.position).normalized;
			if((startLocation - transform.position).magnitude > snap)
			{
				rigid.MovePosition(transform.position + ((moveVec * speed) * Time.deltaTime));
			}
			else
			{
				rigid.MovePosition(startLocation);
				hasHit = false;
			}
		}
	}
	//Jason is a nerd
	void OnCollisionEnter(Collision c)
	{
		// check layer for building type and toggle movement off
		hasHit = true;
	}
}
