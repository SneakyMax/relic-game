using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class RelicPlayer : MonoBehaviour
    {
        public enum DeathType
        {
            None,
            Player,
            Squash
        }

        public int PlayerNumber = 0;

        public HoldingRelic HoldingRelicPrefab;

        [Range(0, 5)]
        public float CrushingDistance;

        public HoldingRelic HoldingRelic { get; set; }
        public PlayerInfo PlayerInfo { get; set; }

        private PlayerController playerController;

        private Transform leftHoldPosition;
        private Transform rightHoldPosition;

        public GameObject SquishEffects;
        public GameObject DeathByPlayerEffects;

        private PlayerController.Direction? lastDirection;

        private CameraController cameraController;

        private Vector3 deathPosition;

        public void Awake()
        {
            playerController = GetComponent<PlayerController>();

            leftHoldPosition = transform.FindChild("RelicHoldPositionLeft");
            rightHoldPosition = transform.Find("RelicHoldPositionRight");

            cameraController = Camera.main.GetComponent<CameraController>();
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Relic"))
            {
                CollideWithRelic(collision);
                return;
            }

            if (collision.gameObject.CompareTag("DropPoint"))
            {
                CollideWithDropPoint(collision);
            }

            if (collision.gameObject.CompareTag("CrushingTrap"))
                CollideWithCrushingTrap(collision);
        }

        public void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("CrushingTrap"))
                CollideWithCrushingTrap(collision);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("KillZone"))
                CollideWithKillZone();
        }

		public void OnTriggerStay(Collider other)
		{
			if (other.gameObject.CompareTag("LeverZone"))
				CollideWithLeverObject(other);
		}

        private void CollideWithKillZone()
        {
            if (HoldingRelic)
            {
                HoldingRelic.RemoveAndRespawn();
                HoldingRelic = null;
            }

            Die(DeathType.None);
        }

		private void CollideWithLeverObject(Collider collider)
		{
			var leverController = collider.gameObject.GetComponent<leverScript>();
			if(leverController == null)
				throw new InvalidOperationException("LeverZone tagged object does not contain a LeverScript");

			if(Input.GetButtonDown("buttonX" + PlayerNumber))
			{
				leverController.Activate();
			}
		}

        private void CollideWithCrushingTrap(Collision collision)
        {
            var normal = collision.contacts.First().normal;

            var allInRaycast = Physics.RaycastAll(new Ray(transform.position, normal), CrushingDistance);

            var hit = allInRaycast.Any(x => x.collider.gameObject.CompareTag("Player") == false);

            if (!hit)
                return;

            BeSquashed(collision.gameObject, DeathType.Squash);

            var trapController = collision.gameObject.GetComponentInParent<trapScript>();
            if (trapController == null)
                throw new InvalidOperationException("huh?");

            trapController.StopCrushingAndReturn();
        }

        private void CollideWithDropPoint(Collision collision)
        {
            if (HoldingRelic == null || HoldingRelic.gameObject == null)
                return;

            Destroy(HoldingRelic.gameObject);
            HoldingRelic = null;
            lastDirection = null;

            collision.gameObject.GetComponent<DropPoint>().AcceptRelic(this);
        }

        private void CollideWithRelic(Collision collision)
        {
            var relic = collision.gameObject.GetComponent<Relic>();

            if (relic.CanPickUp == false)
                return;

            var holdingRelic = relic.BeHeldBy(this);
            
            HoldingRelic = holdingRelic;
            HoldingRelic.RelicPlayer = this;
        }

        public void Update()
        {
            if (HoldingRelic == null)
                return;

            if (playerController.LastRequestedDirection == lastDirection)
                return;

            HoldingRelic.transform.localPosition = 
                playerController.LastRequestedDirection == PlayerController.Direction.Left ? 
                leftHoldPosition.localPosition : rightHoldPosition.localPosition;

            lastDirection = playerController.LastRequestedDirection;
        }

        public void SquashOtherPlayer(GameObject otherPlayer, Collision collision)
        {
            otherPlayer.GetComponent<RelicPlayer>().BeSquashed(gameObject, DeathType.Player);

            GetComponent<PlayerController>().DoBounceOnOtherPlayer(collision);
        }

        private void Die(DeathType deathType)
        {
            deathPosition = transform.position;

            PlayerInfo.Spawner.Despawn(PlayerNumber);
            PlayerInfo.Spawner.SpawnAfterDelay(PlayerNumber);

            if (deathType == DeathType.Player)
                PlayerDeathEffects();
            else if (deathType == DeathType.Squash)
                SquashDeathEffects();
        }

        private void SquashDeathEffects()
        {
            cameraController.ShakeScreen(1, TimeSpan.FromSeconds(0.5f));

            if(SquishEffects != null)
                Instantiate(SquishEffects, deathPosition, Quaternion.identity);
        }

        private void PlayerDeathEffects()
        {
            cameraController.ShakeScreen(0.5f, TimeSpan.FromSeconds(0.5f));

            if(DeathByPlayerEffects != null)
                Instantiate(DeathByPlayerEffects, deathPosition, Quaternion.identity);
        }

        public void BeSquashed(GameObject squasher, DeathType deathType)
        {
            Die(deathType);

            if (HoldingRelic != null)
            {
                HoldingRelic.DropAtPlayer();
                HoldingRelic = null;
            }
        }
    }
}