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
    private new Rigidbody rigidbody;

	// Use this for initialization
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RandomImpulse();
        }
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

    public void RandomImpulse()
    {
        const float pi = Mathf.PI;

        var angle1 = Random.Range(pi, pi + (pi / 3));
        var angle2 = Random.Range(pi + (pi / 3), pi * 2);

        var angle = Random.Range(0, 2) == 0 ? angle1 : angle2;

        var x = Mathf.Cos(angle);
        var y = -Mathf.Sin(angle);

        var force = new Vector3(x, y, 0) * EjectionSpeed;

        rigidbody.AddForce(force);

        var rotation = new Vector3(0, 0, 1) * Random.Range(RotationMax / 2, RotationMax);

        rigidbody.AddTorque(rotation);
    }
}
