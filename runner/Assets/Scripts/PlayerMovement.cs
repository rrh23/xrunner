using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public Transform feetPos;
    [SerializeField] public float groundDistance;
    [SerializeField] public float jumpStrength;

    [SerializeField] private Transform GFX;

    public PlayerControls playerControls;
    private InputAction jumpAction, crouchAction, quitAction;
    public PlayerLogic playerLogic;
    
    public AudioManager audioManager;
    private AudioClip fly;
    
    private bool canFly = true;
    private bool isGrounded;
    private bool isJumping;
    public float jumpTimer, jumpTime;
    private bool jumpPressed, crouchPressed, quitPressed;

    
    public SliderLogic sliderLogic;

    private float crouchHeight = 0.7f;

    //anim vars
    private float animJump = 1f;
    private bool collCheck = false;
    bool isFlyPlaying = false;
    private float _f, _c, _q;

    private void Awake()
    {
        playerControls = new PlayerControls();

    }
    private void Start()
    {
        fly = audioManager.fly;
        jumpTime = jumpStrength; //initialize jumptime
        jumpTimer = 0f; 
    }

    private void OnEnable()
    {
        jumpAction = playerControls.Player.Jump;
        jumpAction.Enable();

        crouchAction = playerControls.Player.Crouch;
        crouchAction.Enable();
        
        quitAction = playerControls.Player.Quit;
        quitAction.Enable();
    }

    private void OnDisable()
    {
        jumpAction.Disable();
        crouchAction.Disable();
        quitAction.Disable();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);
        
        _f = jumpAction.ReadValue<float>();
        _c = crouchAction.ReadValue<float>();
        _q = quitAction.ReadValue<float>();

        jumpPressed = _f > 0.1f;
        crouchPressed = _c > 0.1f;
        quitPressed = _q > 0.1f;


        //[JUMPING]

        if (isGrounded) //resets params if hit ground
        {
            Anim("Bounce");
            isJumping = false;
            canFly = true;
            //jumpTimer = 0f;
            
            //stamina regen
            if (jumpTimer > 0f)
            {
                jumpTimer -= Time.deltaTime * 2;
            }
            else jumpTimer = 0;
        }
 
        if (isGrounded && jumpPressed && !isJumping) //onground and [jump] pressed
        {
            isJumping = true;
            
            rb.velocity = Vector2.up * jumpForce;
            //play anim
            Anim("Jump");
            // Debug.Log("jump.");
        }
        else if (isJumping && jumpPressed && jumpTimer < jumpTime && canFly) //flies if [jump] is held
        {
            // Debug.Log("fly!");
            isJumping = true;
            rb.velocity = Vector2.up * jumpForce;
            jumpTimer += Time.deltaTime;
        }
        else if (!jumpPressed && playerLogic.currentStamina <= 0) //stops flying
        {
            // Debug.Log("fall...");
            isJumping = false;
            canFly = false;
        }
        
        switch (rb.velocity.y)
        {
            case > 0 when !isFlyPlaying:
                playFlySFX();
                // Debug.Log("jump!");
                isFlyPlaying = true;
                break;
            case <= 0 when isFlyPlaying:
                // Debug.Log("fade...");
                isFlyPlaying = false;
                audioManager.startFade();
                break;
        }
        
        //[CROUCHING]
        if (crouchPressed)
        {
            if(crouchAction.triggered) audioManager.playSound(audioManager.crouch);
            Anim("Crouch");
        }
        
        else if(!jumpPressed && !crouchPressed)
        {
            GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
        }

        if (quitPressed) LogicScript.Instance.Pause();
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
                rb.velocity = Vector2.down * (jumpForce * 3);
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

    public void playFlySFX()
    {
        if(audioManager.fadeCoroutine != null) audioManager.StopCoroutine(audioManager.fadeCoroutine);
        audioManager.playSound(fly);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collCheck = true;
    }
}
