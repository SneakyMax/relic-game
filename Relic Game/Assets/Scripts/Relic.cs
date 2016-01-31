using System.Collections;
using System.Linq;
using UnityEngine;
using Assets.Scripts;
using Random = UnityEngine.Random;

public class Relic : MonoBehaviour
{
    public ParticleSystem CollisionParticleSystemPrefab;

    [Range(0, 2000)]
    public float EjectionSpeed;

    [Range(0, 500)]
    public float RotationMax;

    [Range(0, 1)]
    public float CameraShakeFactor;

    [Range(0, 10)]
    public float MinCollisionSpeed;

    [Range(0, 10)]
    public float SpawnPickupDelay;

    private CameraController cameraController;
    private new Rigidbody rigidbody;

    public RelicSpawner Spawner { get; set; }
    
    public bool CanPickUp { get; set; }

	// Use this for initialization
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnCollisionEnter(Collision collision)
    {
        var collisionMagnitude = collision.relativeVelocity.magnitude;
        if (collisionMagnitude < MinCollisionSpeed)
            return;

        cameraController.TicScreen(collisionMagnitude * CameraShakeFactor);

        var collisionPoint = collision.contacts.First().point;
        var particlePoint = new Vector3(collisionPoint.x, collisionPoint.y, 0);

        var particles = (GameObject)Instantiate(CollisionParticleSystemPrefab.gameObject, particlePoint, Quaternion.identity);
        Destroy(particles, 8);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("KillZone"))
            EnterKillZone();
    }

    private void EnterKillZone()
    {
        Spawner.RemoveAndSpawnNewRelic();
    }

    public void RandomImpulse()
    {
        const float pi = Mathf.PI;

        var angle1 = Random.Range(pi, pi + (pi / 3));
        var angle2 = Random.Range(pi + (pi / 3), pi * 2);

        var angle = Random.Range(0, 2) == 0 ? angle1 : angle2;

        var x = Mathf.Cos(angle);
        var y = -Mathf.Sin(angle);

        var force = new Vector2(x, y) * EjectionSpeed;

        rigidbody.AddForce(force);
        rigidbody.AddTorque(new Vector3(0, 0, Random.Range(RotationMax / 2, RotationMax)));
    }

    public HoldingRelic BeHeldBy(RelicPlayer player)
    {
        Spawner.DespawnRelic();

        var holdingRelic = Instantiate(Spawner.HoldingRelicPrefab);
        holdingRelic.transform.SetParent(player.gameObject.transform, false);

        holdingRelic.Spawner = Spawner;

        return holdingRelic;
    }

    public void DelayBeingAbleToBePickedUp()
    {
        StartCoroutine(DelayBeingAbleToBePickedUpCouroutine());
    }

    private IEnumerator DelayBeingAbleToBePickedUpCouroutine()
    {
        CanPickUp = false;
        
        yield return new WaitForSeconds(SpawnPickupDelay);

        CanPickUp = true;
    }
}
