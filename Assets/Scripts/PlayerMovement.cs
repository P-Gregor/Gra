using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody player;
    public float moveSpeed = 25f; // Adjust this value to set the player's maximum movement speed. Default: 5f
    public float friction = -1000000f;  // Adjust this value to control the rate of speed reduction
    public float stoppingSpeed = 20f; // Adjust this value to control how quickly the player stops
    private float currentSpeed = 0f;
    // private bool blink = false;
    // private Vector3 lastMoveDirection;
    //Both variables used for teleportation
    private bool dash = false;
    public Vector3 lastPosition;
    private bool stableGround = true;
    public float teleportDistance = 5f; // Adjust this value to set the teleport distance
    void Start()
    {
        player.freezeRotation = true;
        Debug.Log("Players position: " + player.position);
    }
    private void Awake()
    {
        player = GetComponent<Rigidbody>();
    }


    public void movement()
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dash = true;
        }
        /*
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction based on input
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Check if the player is moving
        if (moveDirection != Vector3.zero)
        {
            // Store the last movement direction
            lastMoveDirection = moveDirection;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Calculate the teleport position based on the current position, last move direction, and teleport distance
            Vector3 teleportPosition = transform.position + lastMoveDirection * teleportDistance;

            // Teleport the player to the new position
            transform.position = teleportPosition;
        }
        //blink but doesnt work as intended. Collision detection needed
        */

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
