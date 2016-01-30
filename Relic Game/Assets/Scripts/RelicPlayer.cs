using UnityEngine;

namespace Assets.Scripts
{
    public class RelicPlayer : MonoBehaviour
    {
        public HoldingRelic HoldingRelicPrefab;

        public HoldingRelic HoldingRelic { get; set; }

        private PlatformerMotor2D motor;

        private Transform leftHoldPosition;
        private Transform rightHoldPosition;
        private bool? lastDirection;

        public void Awake()
        {
            motor = GetComponent<PlatformerMotor2D>();

            leftHoldPosition = transform.FindChild("RelicHoldPositionLeft");
            rightHoldPosition = transform.Find("RelicHoldPositionRight");
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Relic"))
            {
                CollideWithRelic(collision);
                return;
            }
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.gameObject.CompareTag("DropPoint"))
            {
                CollideWithDropPoint(collider);
            }
        }

        private void CollideWithDropPoint(Collider2D collision)
        {
            Destroy(HoldingRelic.gameObject);
            HoldingRelic = null;
            lastDirection = null;

            collision.gameObject.GetComponent<DropPoint>().AcceptRelic(this);
        }

        private void CollideWithRelic(Collision2D collision)
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

            if (motor.facingLeft == lastDirection)
                return;

            HoldingRelic.transform.localPosition = 
                motor.facingLeft ? leftHoldPosition.localPosition : rightHoldPosition.localPosition;

            lastDirection = motor.facingLeft;
        }
    }
}