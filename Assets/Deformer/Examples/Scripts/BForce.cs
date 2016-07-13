using UnityEngine;
using System.Collections;

public class BForce : MonoBehaviour
{
    public Vector3 force;

    // Use this for initialization
    void Start()
    {
        var rigidBody = GetComponent<Rigidbody>();

        rigidBody.AddForce(force);
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }
}
