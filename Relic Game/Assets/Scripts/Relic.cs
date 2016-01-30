using UnityEngine;
using System.Collections;

public class Relic : MonoBehaviour
{
    public float EjectionSpeed;

    public float RotationMax;

	// Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RandomImpulse();
        }
    }

    public void RandomImpulse()
    {
        var pi = Mathf.PI;
        var angle = Random.Range(pi + (pi / 4), (pi * 2) - (pi / 4));

        var x = Mathf.Cos(angle);
        var y = -Mathf.Sin(angle);

        var force = new Vector3(x, y, 0) * EjectionSpeed;

        GetComponent<Rigidbody>().AddForce(force);

        var rotation = new Vector3(0, 0, 1) * Random.Range(RotationMax / 2, RotationMax);

        GetComponent<Rigidbody>().AddTorque(rotation);
    }
}
