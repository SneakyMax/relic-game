using System;
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

    private CameraController cameraController;
    private new Rigidbody2D rigidbody;

	// Use this for initialization
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnCollisionEnter2D(Collision2D collision)
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
        rigidbody.AddTorque(Random.Range(RotationMax / 2, RotationMax));
    }

    public void BeHeldBy(RelicPlayer player)
    {
        Destroy(gameObject);
    }
}
