using UnityEngine;

namespace Assets.Scripts
{
    public class RelicPlayer : MonoBehaviour
    {
        public HoldingRelic HoldingRelicPrefab;

        public HoldingRelic HoldingRelic { get; set; }

        private PlayerController playerController;

        private Transform leftHoldPosition;
        private Transform rightHoldPosition;

        private PlayerController.Direction? lastDirection;

        public void Awake()
        {
            playerController = GetComponent<PlayerController>();

            leftHoldPosition = transform.FindChild("RelicHoldPositionLeft");
            rightHoldPosition = transform.Find("RelicHoldPositionRight");
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Relic"))
            {
                CollideWithRelic(collision);
                return;
            }
        }

        public void OnTriggerEnter(Collider collider)
        {
            if(collider.gameObject.CompareTag("DropPoint"))
            {
                CollideWithDropPoint(collider);
            }
        }

        private void CollideWithDropPoint(Collider collision)
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

            relic.BeHeldBy(this);

            var holdingRelic = Instantiate(HoldingRelicPrefab);
            holdingRelic.transform.SetParent(transform, false);

            HoldingRelic = holdingRelic;
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
    }
}