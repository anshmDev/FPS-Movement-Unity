using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fps Movement Script by Ansh Maru, with inspiration from Brackeys

public class Movement : MonoBehaviour
{
    public CharacterController player;

    public Transform playerCapsule;
    
    //some movement jump and speed variables
    public float walkSpeed = 4f;
    public float runSpeed = 6f;
    public float crouchSpeed = 2f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public float maxSpeed = 10f;

    //stuff for the grounded stuff
    public Transform groundCheck;
    public float groundDistance = 2f;
    public LayerMask groundMask;
    
    //crouching stuff
    private bool m_Crouch = false;
    private float m_originalHeight;
    [SerializeField] private float m_crouchHeight = 1f;
    public KeyCode crouchKey = KeyCode.LeftControl;
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale = new Vector3(1, 1f, 1);
    
    //other shit
    Vector3 velocity;

    public bool isGrounded;
    
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
        if (Input.GetKeyDown(crouchKey) && (isGrounded == true))
        {
            m_Crouch = !m_Crouch;
            
            //yep another separate function 
            CheckCrouch();
        }
    }
    
    
    //the movement function to, u kno, move the player (includes jump)
    public void Move()
    {
        //this is all to check if the player is grounded or not
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        //down to here is all the isgrounded stuff
        
        //get the axes for the player to move
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        //get a vector3 to calculate where the player is going to move
        Vector3 move = transform.right * x + transform.forward * z;

        player.Move(move * (walkSpeed * 3) * Time.deltaTime);

        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        player.Move(velocity * Time.deltaTime);
        
    }
    
    //this is just to check if the player should be crouching or not
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

