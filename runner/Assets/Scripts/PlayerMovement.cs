using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public Transform feetPos;
    [SerializeField] public float groundDistance;
    [SerializeField] public float jumpStrength;

    [SerializeField] private Transform GFX;

    private PlayerControls playerControls;
    private InputAction jumpAction;
    private InputAction crouchAction;
    public PlayerLogic playerLogic;

    private bool canFly = true;
    private bool isGrounded;
    private bool isJumping;
    public float jumpTimer, jumpTime;
    private bool jumpPressed, crouchPressed;

    
    public SliderLogic sliderLogic;

    private float crouchHeight = 0.7f;

    //anim vars
    private float animJump = 1f;
    private bool collCheck = false;

    private void Awake()
    {
        playerControls = new PlayerControls();

    }
    private void Start()
    {
        jumpTime = jumpStrength; //initialize jumptime
        jumpTimer = 0f; 
    }

    private void OnEnable()
    {
        jumpAction = playerControls.Player.Jump;
        jumpAction.Enable();

        crouchAction = playerControls.Player.Crouch;
        crouchAction.Enable();
    }

    private void OnDisable()
    {
        jumpAction.Disable();
        crouchAction.Disable();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);
        jumpPressed = jumpAction.ReadValue<float>() > 0.1f;
        crouchPressed = crouchAction.ReadValue<float>() > 0.1f;

        //[JUMPING]

        //anim
        if (jumpPressed)
        {
            Anim("Jump");
        }
        else
        {
            GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
        }

        if (isGrounded) //resets params if hit ground
        {
            Anim("Bounce");
            isJumping = false;
            canFly = true;
            //jumpTimer = 0f;
            if(jumpTimer > 0) 
            {
                jumpTimer -= Time.deltaTime * 2;
            }
            else jumpTimer = 0;
        }


        if (isGrounded && jumpPressed && !isJumping) //onground and [jump] pressed
        {
            isJumping = true;
            rb.velocity = Vector2.up * jumpForce;
        }
        else if (isJumping && jumpPressed && jumpTimer < jumpTime && canFly) //flies if [jump] is held
        {
            isJumping = true;
            rb.velocity = Vector2.up * jumpForce;
            jumpTimer += Time.deltaTime;
        }
        else if (!jumpPressed && playerLogic.currentStamina <= 0) //stops flying
        {
            isJumping = false;
        }

        //[CROUCHING]
        if (crouchPressed)
        {
            Anim("Crouch");
        }
        
        else if(!jumpPressed && !crouchPressed)
        {
            GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
        }
    }

    private void Anim(string name) 
    {
        if (name == "Jump")
        {
            GFX.localScale = new Vector3(GFX.localScale.x, animJump, GFX.localScale.z);
            if (jumpAction.triggered)
            {
                animJump = 1.4f;
            }
            if (animJump > 1f)
            {
                animJump -= Time.deltaTime;
            }
        }
        if(name == "Crouch")
        {
            GFX.localScale = new Vector3(GFX.localScale.x, crouchHeight, GFX.localScale.z);

            if (crouchAction.triggered) //only when pressed and not held
            {
                rb.velocity = Vector2.down * jumpForce * 3;
            }
            if (isJumping) //go back to normal size if both is pressed
            {
                GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
            }
        }
        if(name == "Bounce")
        {
            GFX.localScale = new Vector3(GFX.localScale.x, animJump, GFX.localScale.z);
            if(collCheck) // only trigger this for a split second
            {
                animJump = 0.8f;
                collCheck = false;
            }
            if (animJump < 1f)
            {
                animJump += Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collCheck = true;
    }
}
