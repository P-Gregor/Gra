using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody player;
    private float moveSpeed = 5f; // Adjust this value to set the player's maximum movement speed. Default: 5f
    private float stoppingSpeed = 20f; // Adjust this value to control how quickly the player stops
    private float currentSpeed = 0f;
    private float damping = 1f;
    private Vector3 moveVelocity;
    private bool dash = false;
    private Vector3 lastPosition;
    private bool stableGround = true;
    void Start()
    {
        player.freezeRotation = true;
    }
    private void Awake()
    {
        player = GetComponent<Rigidbody>();
    }


    private void movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        if (horizontalInput < 0) // Moving left (A key or left arrow)
        {
            if (dash)
            {
                currentSpeed = -(moveSpeed + 150);
                dash = false;
            }
            else
            {
                currentSpeed = -moveSpeed;
            }

        }
        else if (horizontalInput > 0) // Moving right (D key or right arrow)
        {
            if (dash)
            {
                currentSpeed = (moveSpeed + 150);
                dash = false;
            }
            else
            {
                currentSpeed = moveSpeed;
            }
        }
        else
        {
            // Apply stopping speed when no input is pressed
            if (currentSpeed > 0)
            {
                currentSpeed = Mathf.Max(0f, currentSpeed - stoppingSpeed * Time.fixedDeltaTime);
            }
            else if (currentSpeed < 0)
            {
                currentSpeed = Mathf.Min(0f, currentSpeed + stoppingSpeed * Time.fixedDeltaTime);
            }
        }

    }


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dash = true;
        }
    }

    private void FixedUpdate()
    {
        movement();
        if (stableGround)
        {
            Vector3 newPosition = transform.position + Vector3.right * currentSpeed * Time.fixedDeltaTime; //calculate the players position
            // Move the player to the new position
            transform.position = newPosition;
            lastPosition = transform.position + (Vector3.right) * currentSpeed * (Time.fixedDeltaTime - .04f);
        }
        else if (!stableGround)
        {
            //Vector3 newPosition = transform.position + (Vector3.right) * currentSpeed * (Time.fixedDeltaTime - .05f);
            // Move the player to the new position
            transform.position = lastPosition;
            stableGround = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            stableGround = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            stableGround = false;
        }
    }



}