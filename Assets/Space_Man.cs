using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Man : MonoBehaviour {

    [SerializeField] float mainThrust = 500f;
    [SerializeField] ParticleSystem jetPackParticles;

    public float jumpForce = 15f;
    public float downForce = 5f;
    public float horizontalSpeed = 3f;
    public float horizontalFlyingSpeed = 3f;
    

    private bool isGrounded;
    Rigidbody rigidBody;

    enum State { Jumping, Flying, Grounded, Falling}
    State state = State.Grounded;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Grounded)
        {
            RespondToJumpInput();
            RespondToThrustInput();
            RespondToHorizontalInput(horizontalSpeed);
        } else if (state == State.Jumping || state == State.Falling)
        {
            RespondToHorizontalInput(horizontalSpeed);
            RespondToThrustInput();
        } else if (state == State.Flying || state == State.Jumping)
        {
            RespondToHorizontalInput(horizontalFlyingSpeed);
        }
        Fall();
    }

    private void Fall()
    {
        rigidBody.AddForce(-Vector3.up * downForce);
    }

    private void RespondToHorizontalInput(float speed)
    {
        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.MovePosition(transform.position + (-transform.forward) * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidBody.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);
        }
    }


    private void RespondToJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded && !Input.GetKey(KeyCode.Space))
        {
            state = State.Jumping;
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) // todo add fuel component and gauge
        {
            state = State.Flying;
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            state = State.Grounded;
            //  todo jetPackParticles.Play();
        } else
        {
            // todo jetPackParticles.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        state = State.Grounded;

    }
}