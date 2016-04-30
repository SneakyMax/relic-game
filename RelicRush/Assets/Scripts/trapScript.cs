using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class trapScript : MonoBehaviour {

	#region public variables

	public enum direction {UP, RIGHT, LEFT, DOWN};

	public direction trapDirection = direction.UP;

	public bool isActive = false;

	public bool isHolding = false;

	[Range(0.01f, 2.0f)]
	public float speed = 0.4f;

	[Range(0.0f, 1.0f)]
	public float snap = 0.2f;

	public AnimationCurve speedEasingCrushingCurve = AnimationCurve.Linear(0, 0, 1, 1);

	public float timeToWaitAfterHit = 0.0f;

	[Range(0.0f, 2.0f)]
	public float timeToMaxSpeed = 0.5f;

	public float crushingEasingTimer = 0.0f;

	#endregion
	#region private variables

	private Vector3 startLocation;

	private Rigidbody rigid;

	public enum STATE {WAITING, ACTIVATED, MOVING, HIT, RETURNING}

	[SerializeField]
	public STATE CurrentState = STATE.WAITING;

	private float hitPauseTimer = 0.0f;

	#endregion

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
	void FixedUpdate () 
	{
		switch (CurrentState) 
		{
		case STATE.WAITING:
			if(isActive)
			{
				CurrentState = STATE.ACTIVATED;
			}
			rigid.MovePosition(startLocation);
			rigid.velocity = Vector3.zero;
			crushingEasingTimer = 0.0f;
			hitPauseTimer = 0.0f;
			// not doing anything, waiting for the word to take off.
			break;
		case STATE.ACTIVATED:
			// any initializations stuff like slight shake before taking off.
			CurrentState = STATE.MOVING;            
                break;
		case STATE.MOVING:
			CrushTrapMovement();               
                break;
		case STATE.HIT:
			hitPauseTimer += Time.deltaTime;

			if(hitPauseTimer >= timeToWaitAfterHit)
				CurrentState = STATE.RETURNING;
			break;
		case STATE.RETURNING:
			Vector3 moveVec = (startLocation - transform.position).normalized;
			if((startLocation - transform.position).magnitude > snap)
			{
				rigid.MovePosition(transform.position + ((moveVec * speed)));
			}
			else
			{
				rigid.MovePosition(startLocation);
				CurrentState = STATE.WAITING;
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
		crushingEasingTimer += Time.fixedDeltaTime;

		float t = crushingEasingTimer / speed;

		t = speedEasingCrushingCurve.Evaluate(t);

		float modifiedSpeed = t * speed;

		switch (trapDirection)
		{
		case direction.UP:		
			rigid.MovePosition (transform.position + (Vector3.up * modifiedSpeed));
			rigid.position.Scale(new Vector3 (0, 1, 1));
			rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
			break;
			//move UP
		case direction.DOWN:
			rigid.MovePosition(transform.position + (Vector3.down * modifiedSpeed));
			rigid.position.Scale(new Vector3 (0, 1, 1));
			rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
			break;
			//move DOWN
		case direction.LEFT:
			rigid.MovePosition(transform.position + (Vector3.left * modifiedSpeed));
			rigid.position.Scale(new Vector3 (1, 0, 1));
			rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
			break;
			//move LEFT
		case direction.RIGHT:
			rigid.MovePosition(transform.position + (Vector3.right * modifiedSpeed));
			rigid.position.Scale(new Vector3 (1, 0, 1));
			rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
			break;
			//move RIGHT
		default:
			trapDirection = direction.UP;
			break;
		}
	}

	//Jason is a nerd

	void OnCollisionEnter(Collision c)
	{
		// check layer for Player or toggle movement off
		if(c.gameObject.CompareTag("Player") || CurrentState != STATE.MOVING || c.gameObject.CompareTag("Relic") )
			return;

		// v ONLY IF BUILDING v
		if(CurrentState == STATE.MOVING)
		{
		    StopCrushingAndReturn();
		}
	}

	public void Activate()
	{
		isActive = true;
	}

	public void SetHold(bool val)
	{
		isHolding = val;
	}

	public void StopCrushingAndReturn()
	{
	    CameraShaker.Get().ShakeScreen(1, TimeSpan.FromSeconds(0.35));

        GeneralAudioController.PlaySound("RushSquish");
        CurrentState = STATE.HIT;
	}
}
