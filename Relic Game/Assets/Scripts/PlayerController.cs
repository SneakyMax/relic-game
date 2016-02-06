using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {	
        [Range(0, 3)]
        public float Gravity;

        [Range(0, 30)]
        public float MaxHorizontalSpeed;

        [Range(0, 30)]
        public float MaxHorizontalAccelleration;

        [Range(0, 5)]
        public float AirControl;

        [Range(0, 10)]
        public float GroundFriction;

        [Range(1, 5)]
        public float ReverseSpeedFactor = 1;
        
        [Range(0, 10)]
        public float JumpStartThreshhold = 1;

        [Range(0, 5)]
        public float JumpStartAmount = 3;
        
        [Range(0, 60)]
        public float JumpVelocity;

        [Range(0, 10)]
        public float InstantStopSpeedThreshold;

        [Range(0, 30)]
        public float BounceOtherPlayerForce;

        [Range(0, 100)]
        public float MaxVerticalSpeed;

        public enum Direction { Left, Right }

        public Direction LastRequestedDirection { get; set; }

        public PlayerState State { get; set; }

        public bool AllowInput { get; set; }

        public enum PlayerState
        {
            Grounded,
            InAir,
            ClimbingUpLedge
        }

		private int playerNumber;

        private bool jumpRequested;
        private bool jumpStopRequested;

        private float desiredHorizontalAccelleration;
        private bool hitGround;
		private Animator AnimController;
		private float localModelScale = 0.25f;

        private Vector2 currentVelocity;

        private new Rigidbody rigidbody;       

        public void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

		public void Start()
		{
			playerNumber = GetComponent<RelicPlayer>().PlayerNumber;
			AnimController = GetComponentInChildren<Animator>();
		    AllowInput = true;
		}

        public void Update()
        {
            UpdateInput();
            if (Math.Abs(currentVelocity.x) > 0.05f)
            {
                AnimController.SetBool("isRunning", true);
                if (currentVelocity.x < 0.0f)
                    AnimController.gameObject.transform.localScale = new Vector3(-localModelScale, localModelScale, -localModelScale);
                else
                    AnimController.gameObject.transform.localScale = new Vector3(-localModelScale, localModelScale, localModelScale);
            }
            else
                AnimController.SetBool("isRunning", false);
        }

        public void FixedUpdate()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            currentVelocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y);

            // If you're on the ground...but you're falling...you're not on the ground
            if (currentVelocity.y < 0.1 && State == PlayerState.Grounded)
                State = PlayerState.InAir;

            // Always apply gravity

            if (currentVelocity.y > -MaxVerticalSpeed)
            {
                rigidbody.AddForce(0, -Gravity, 0, ForceMode.VelocityChange);
            }

            if (hitGround)
            {
                State = PlayerState.Grounded;
                hitGround = false;
                AnimController.SetBool("isJumping", false);
            }

            if (State == PlayerState.Grounded)
            {
                jumpStopRequested = false;

                if (Mathf.Abs(desiredHorizontalAccelleration) > 0)
                {
                    TryPlayerMoveOnGround();
                }
                else
                {
                    StopInstantly();
                    //SlowDownHorizontalMovement();
                }
                
                TryJump();
            }
            else if (State == PlayerState.InAir)
            {
                if (Mathf.Abs(desiredHorizontalAccelleration) > 0)
                {
                    // Air control
                    TryPlayerMoveInAir();
                }

                if (jumpStopRequested && currentVelocity.y > 0.1)
                {
                    jumpStopRequested = false;
                    SetVerticalVelocity(0);
                }
            }

            if (Mathf.Abs(currentVelocity.x) > MaxHorizontalSpeed)
            {
                SetHorizontalVelocity(MaxHorizontalSpeed * Mathf.Sign(currentVelocity.x));
            }
        }

        private void SetHorizontalVelocity(float velocity)
        {
            var difference = currentVelocity.x - velocity;

            rigidbody.AddForce(new Vector3(-difference, 0, 0), ForceMode.VelocityChange);
        }

        private void SetVerticalVelocity(float velocity)
        {
            var verticalVelocity = rigidbody.velocity.y;
            var difference = verticalVelocity - velocity;

            rigidbody.AddForce(new Vector3(0, -difference, 0), ForceMode.VelocityChange);
        }

        private void TryJump()
        {
            if (!jumpRequested)
                return;

            AnimController.SetBool("isJumping", true);
            SetVerticalVelocity(JumpVelocity);
            jumpRequested = false;
            State = PlayerState.InAir;
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
        }

        private void StopInstantly()
        {
            rigidbody.AddForce(new Vector3(-currentVelocity.x, 0, 0), ForceMode.VelocityChange);
        }

        private void TryPlayerMoveOnGround()
        {
            if (Mathf.Abs(currentVelocity.x) < JumpStartThreshhold)
            {
                // We need a jump-start
                rigidbody.AddForce(new Vector3(JumpStartAmount * Mathf.Sign(desiredHorizontalAccelleration), 0, 0),
                    ForceMode.Impulse);
            }

            if ((int) Mathf.Sign(desiredHorizontalAccelleration) != (int) Mathf.Sign(currentVelocity.x))
            {
                //We're trying to reverse direction
                rigidbody.AddForce(new Vector3(desiredHorizontalAccelleration * GroundFriction * ReverseSpeedFactor, 0, 0));
            }
            else
            {
                // Apply force to move
                rigidbody.AddForce(new Vector3(desiredHorizontalAccelleration, 0, 0));
            }
        }

        private void TryPlayerMoveInAir()
        {
            if (Mathf.Abs(currentVelocity.x) < JumpStartThreshhold)
            {
                // We need a jump-start
                var amount = new Vector3(JumpStartAmount * Mathf.Sign(desiredHorizontalAccelleration), 0, 0) *
                             AirControl;
                rigidbody.AddForce(amount, ForceMode.Impulse);
            }

            if ((int)Mathf.Sign(desiredHorizontalAccelleration) != (int)Mathf.Sign(currentVelocity.x))
            {
                //We're trying to reverse direction
                rigidbody.AddForce(new Vector3(desiredHorizontalAccelleration * ReverseSpeedFactor * AirControl, 0, 0));
            }

            if (Mathf.Abs(desiredHorizontalAccelleration) > 0.1)
            {
                // Apply force to move
                rigidbody.AddForce(new Vector3(desiredHorizontalAccelleration * AirControl, 0, 0));
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                OnCollisionPlayer(collision);
                return;
            }

            OnCollisionOther(collision);
        }

        private void OnCollisionOther(Collision collision)
        {
            var points = collision.contacts.First(); //todo
            var normal = new Vector2(points.normal.x, points.normal.y);

            var upAmount = Vector2.Dot(new Vector2(0, 1), normal);

            var verticalVelocity = rigidbody.velocity.y;

            if (upAmount > 0.1 && verticalVelocity < 0.1) //facing up and moving down
            {
                hitGround = true;
            }
        }

        private void OnCollisionPlayer(Collision collision)
        {
            var normal = collision.contacts.First().normal;

            if (currentVelocity.y >= 0)
                return; // Moving up

            var amountUp = Vector3.Dot(Vector3.up, normal);

            if (amountUp >= 0.1)
            {
                // The contact normal is "pointing up", which means we stomped someone else
                GetComponent<RelicPlayer>().SquashOtherPlayer(collision.gameObject, collision);
            }
        }

        public void CancelVerticalMomentum()
        {
            var verticalSpeed = currentVelocity.y;

            rigidbody.AddForce(new Vector3(0, -verticalSpeed, 0), ForceMode.VelocityChange);
        }

        public void DoBounceOnOtherPlayer(Collision collision)
        {
            CancelVerticalMomentum();

            rigidbody.AddForce(new Vector3(0, BounceOtherPlayerForce, 0), ForceMode.Impulse);
        }

        public void OnCollisionStay(Collision collision)
        {
            var points = collision.contacts.First(); //todo
            var normal = new Vector2(points.normal.x, points.normal.y);

            var upAmount = Vector2.Dot(new Vector2(0, 1), normal);

            var verticalVelocity = rigidbody.velocity.y;

            if (upAmount > 0.1 && verticalVelocity < 0.1) //facing up and moving down
            {
                hitGround = true;
            }
        }

        private void UpdateInput()
        {
            if (!AllowInput)
            {
                desiredHorizontalAccelleration = 0;
                jumpRequested = false;
                return;
            }

            if (playerNumber == 0)
                throw new InvalidOperationException("Player number hasn't been set.");
            
			var horizontalAxis = Input.GetAxis("Horizontal" + playerNumber);			         

            desiredHorizontalAccelleration = horizontalAxis * MaxHorizontalAccelleration;

            LastRequestedDirection = horizontalAxis < 0
                ? Direction.Left
                : horizontalAxis > 0 ? Direction.Right : LastRequestedDirection;

            if (State == PlayerState.Grounded)
            {
                if (Input.GetButtonDown("buttonA" + playerNumber))
                {
                    jumpRequested = true;
                }
            }


            if (Input.GetButtonUp("buttonA" + playerNumber))
            {
                if (State == PlayerState.InAir)
                {
                    jumpStopRequested = true;
                }
            }
        }
    }
}