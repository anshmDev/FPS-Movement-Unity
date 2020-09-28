using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fps Movement Script by Ansh Maru, with inspiration from Brackeys

public class Movement : MonoBehaviour
{
    
    //this is the actual player while the transform is the player capsule
    public CharacterController player;
    public Transform playerCapsule;
    
    //some movement jump and speed variables
    public float walkSpeed = 2f;
    public float runSpeed = 4f;//the runSpeed and crouchSpeed variables are not being used rn. Planning to integrate it soo though
    public float crouchSpeed = 0.5f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f; //earths gravity is about 9.81 m/s^2, so this is the default value for the script.(if i removed the minus the player would float)
    /*public float maxSpeed = 10f;/*
    //This is not needed. This is in case the script has a conflict

    //stuff for the grounded stuff
    public Transform groundCheck;
    public float groundDistance = 2f;
    public LayerMask groundMask;
    public bool isGrounded;
    
    //crouching stuff
    private bool m_Crouch = false;
    private float m_originalHeight;
    [SerializeField] private float m_crouchHeight = 1f;
    public KeyCode crouchKey = KeyCode.LeftControl;
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale = new Vector3(1, 1f, 1);
    
    
    //poor loner :(
    Vector3 velocity;

    
    
    //START THE CODE 
    void Start()
    {
        //get the character controller component from the player gameobject
        player = GetComponent<CharacterController>();
        
        //get the current height of the player
        m_originalHeight = player.height;
    }
    void Update()
    {
        //made this a separate function to stay organized (that didnt work out did it?)
        Move();
        
        //see if the crouch keybind is pressed down
        if (Input.GetKeyDown(crouchKey) && (isGrounded))
        {
            m_Crouch = !m_Crouch;
            
            //yep another separate function 
            CheckCrouch();
        }
    }
    
    
    //the movement function to, you know, move the player (includes jump functions)
    public void Move()
    {
        //this is all to check if the player is grounded or not
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        
        //get the axes for the player to move
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");
        
        //get a vector3 to calculate where the player is going to move
        Vector3 move = transform.right * xMove + transform.forward * zMove;
        
        //Clamp the vectors magnitude to make sure that diagonal movement speed doesn't get doubled

        player.SimpleMove(Vector3.ClampMagnitude(move, 1.0f) * walkSpeed);

        
        
        /*some commented out code just in case I mess up or there is a conflict. this is basically a backup movement script
        Vector3 forwardMovement = transform.forward * zMove;
        Vector3 rightMovement = transform.right * xMove;
        Vector3 movement = new Vector3(xMove, 0, zMove);
        */
        
        //here as well
        /*player.Move(move * (walkSpeed * 3) * Time.deltaTime);*/

        //stuff for jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        player.Move(velocity * Time.deltaTime);
        
        

    }
    
    //this is just to check if the player should be crouching or not, and then scale the player accordingly
    void CheckCrouch()
    {
        if (m_Crouch == true && isGrounded == true)
        {
            player.height = m_crouchHeight;
            playerCapsule.localScale = crouchScale;
            playerCapsule.position = new Vector3(playerCapsule.position.x, playerCapsule.position.y - 0.5f, playerCapsule.position.z);
        }
        else
        {
            player.height = m_originalHeight;
            playerCapsule.localScale = playerScale;
            playerCapsule.position = new Vector3(playerCapsule.position.x, playerCapsule.position.y + 0.5f, playerCapsule.position.z);
        }
    }
    
}

