using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private float jumpTime = 0.3f;
    [SerializeField] private int jumpAmount = 0;
    [SerializeField] private int jumpMax = 2;

    [SerializeField] private Transform GFX;

    private PlayerControls playerControls;
    private InputAction jumpAction;
    private InputAction crouchAction;

    private bool canFly = true;
    private bool isGrounded;
    [SerializeField] private bool isJumping;
    private float jumpTimer;
    private bool jumpPressed, crouchPressed;

    private float crouchHeight = 0.7f;

    private void Awake()
    {
        playerControls = new PlayerControls();
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
        //jump counter
        if (jumpAction.triggered && isJumping)
        {
            jumpAmount += 1;
        }

        if (isGrounded && jumpPressed && !isJumping) //onground and [jump] pressed
        {
            isJumping = true;
            jumpTimer = 0f;
            rb.velocity = Vector2.up * jumpForce;
        }
        else if (isJumping && jumpPressed && jumpTimer < jumpTime && canFly) //flies if [jump] is held
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpTimer += Time.deltaTime;
        }
        else if (!jumpPressed && jumpTimer >= jumpTime || jumpAmount >= jumpMax) //stops flying
        {
            isJumping = false;
            jumpAmount = 0;
        }
        else if (isGrounded)
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
            if (isJumping && isGrounded)
            {
                GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
                canFly = false;
                rb.velocity = Vector2.up * jumpForce;
            }
        }
        if(!crouchPressed)
        {
            canFly = true;
            GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
        }
    }
}
