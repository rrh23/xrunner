using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance;
    [SerializeField] private float jumpStrength;

    [SerializeField] private Transform GFX;

    private PlayerControls playerControls;
    private InputAction jumpAction;
    private InputAction crouchAction;

    private bool canFly = true;
    private bool isGrounded;
    private bool isJumping;
    [SerializeField] private float jumpTimer, jumpTime;
    private bool jumpPressed, crouchPressed;

    private float currentStamina, 
        maxStamina = 1;
    public SliderLogic sliderLogic;

    private float crouchHeight = 0.7f;

    //anim vars
    private float animJump;

    private void Awake()
    {
        playerControls = new PlayerControls();

    }
    private void Start()
    {
        maxStamina = jumpStrength;
        currentStamina = maxStamina;
        sliderLogic.SetMaxVal(maxStamina);

        jumpTime = jumpStrength; //initialize jumptime
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

        currentStamina = jumpTime - jumpTimer;
        sliderLogic.SetStamina(currentStamina);


        //[JUMPING]

        //anim
        if (jumpPressed)
        {
            GFX.localScale = new Vector3(GFX.localScale.x, 1.4f, GFX.localScale.z);
            Debug.Log("jump!");
        }
        else
        {
            GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
        }

        if (isGrounded) //resets params if hit ground
        {
            isJumping = false;
            canFly = true;
            jumpTimer = 0f;
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
        else if (!jumpPressed && currentStamina <= 0) //stops flying
        {
            isJumping = false;
        }

        //[CROUCHING]
        if (crouchPressed)
        {
            GFX.localScale = new Vector3(GFX.localScale.x, crouchHeight, GFX.localScale.z);

            if (crouchAction.triggered) //only when pressed and not held
            {
                rb.velocity = Vector2.down * jumpForce * 3;
            }
        }
        if(!crouchPressed)
        {
            canFly = true;
            GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
        }
    }
}
