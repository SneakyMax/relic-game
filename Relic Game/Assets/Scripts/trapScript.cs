using UnityEngine;
using System.Collections;

public class trapScript : MonoBehaviour {

	public bool isTrap = false;
	public enum direction {UP, RIGHT, LEFT, DOWN};
	public direction trapDirection = direction.UP;
	public bool isActive = false;
	public bool isHolding = false;
	[Range(0.2f, 20.0f)]
	public float speed = 1.0f;
	[Range(0.0f, 1.0f)]
	public float snap = 0.2f;


	private Vector3 startLocation;
	private Rigidbody rigid;
	private enum STATE {WAITING, ACTIVATED, MOVING, HIT, RETURNING}
	private STATE _currentState = STATE.WAITING;

	// Use this for initialization
	void Start () 
	{
		startLocation = transform.position;
		if(rigid == null && isTrap)
		{
			rigid = GetComponent<Rigidbody>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (_currentState) 
		{
		case STATE.WAITING:
			if(isActive)
			{
				_currentState = STATE.ACTIVATED;

			}
			// not doing anything, waiting for the word to take off.
			break;
		case STATE.ACTIVATED:
			// any initializations stuff like slight shake before taking off.
			_currentState = STATE.MOVING;
			break;
		case STATE.MOVING:
			CrushTrapMovement();
			break;
		case STATE.HIT:
			// wait for dramatic effect
			// maybe squish or something when hitting
			if(!isHolding)_currentState = STATE.RETURNING;
			break;
		case STATE.RETURNING:
			Vector3 moveVec = (startLocation - transform.position).normalized;
			if((startLocation - transform.position).magnitude > snap)
			{
				rigid.MovePosition(transform.position + ((moveVec * speed) * Time.deltaTime));
			}
			else
			{
				rigid.MovePosition(startLocation);
				_currentState = STATE.WAITING;
				isActive = false;
			}
			break;
		default:
		break;
		}
		//move towards "startLocation"
	}

	private void CrushTrapMovement()
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
	}

	//Jason is a nerd

	void OnCollisionEnter(Collision c)
	{
		// check layer for building and toggle movement off

		// v ONLY IF BUILDING v
		_currentState = STATE.HIT;
	}

	public void Activate()
	{
		isActive = true;
	}

	public void SetHold(bool val)
	{
		isHolding = val;
	}
}
