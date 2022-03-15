using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightController : MonoBehaviour
{
    //public fields
    public float speed = 1;

    //Private fields
    Rigidbody2D rb;
    Animator animator;
    Player player;
    [SerializeField]Collider2D standingCollider;
    [SerializeField]Transform groundCheckCollider;
    [SerializeField]Transform overheadCheckCollider;
    [SerializeField]LayerMask groundLayer;
    [SerializeField]int totalJumps;
    int availableJumps;
    const float groundCheckRadius = 0.2f;
    const float overheadCheckRadius = 0.2f;
    float horizontalValue;
    float runSpeedModifier = 2f;
    float crouchSpeedModifier = 0.5f;
    [SerializeField] float jumpPower = 500f;
    bool isGrounded = false;
    bool isRunning = false;
    bool facingRight = true;
    bool crouchPressed = false;
    bool multipleJump = false;
    bool coyoteJump = false;
    public bool berzerkActive = false;
    float nextDrainTick = 0f;

    void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if(player.doubleJumpAbility)
            totalJumps = 2;
        else
            totalJumps = 1;
        availableJumps = totalJumps;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(KeyCode.LeftShift))
            isRunning = true;
        if(Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;

        if(Input.GetButtonDown("Jump"))
            Jump();
        if(Input.GetButtonDown("Crouch"))
            crouchPressed = true;
        else if(Input.GetButtonUp("Crouch"))
            crouchPressed = false;
        if(Input.GetKeyDown(KeyCode.Q) && player.berzerkAbility && player.power > 0)
        {
            StartCoroutine(drainPower(50, 5.0f));
        }
        animator.SetFloat("yVelocity", rb.velocity.y);
        if(Input.GetKeyDown(KeyCode.R))
        {
            Loader.Load(Loader.Scene.hub);
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            Loader.Load(Loader.Scene.demo);
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            Loader.Load(Loader.Scene.ares);
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            Loader.Load(Loader.Scene.hades);
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            Loader.Load(Loader.Scene.hermes);
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            FindObjectOfType<Player>().hasBeatenHermes = true;
            FindObjectOfType<Player>().doubleJumpAbility = true;
            GameObject.Find("Jump").GetComponent<Image>().enabled = true;
            totalJumps = 2;
            availableJumps = totalJumps;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            FindObjectOfType<Player>().hasBeatenAthena = true;
            FindObjectOfType<Player>().shieldAbility = true;
            GameObject.Find("Shield").GetComponent<Image>().enabled = true;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            FindObjectOfType<Player>().hasBeatenAres = true;
            FindObjectOfType<Player>().berzerkAbility = true;
            GameObject.Find("Rage").GetComponent<Image>().enabled = true;
        }
    }

    void FixedUpdate()
    {
        if(!player.GetComponent<PlayerCombat>().blockPressed)
        {
            GroundCheck();
            Move(horizontalValue, crouchPressed);
        }
    }

    public IEnumerator drainPower(float drainAmount, float duration)
    {
        berzerkActive = true;
        AudioManager.instance.Play("Beserk");
        animator.SetTrigger("Berzerk");
        yield return new WaitForSeconds(0.25f);
        GameObject.Find("BerzerkCanvas").GetComponent<Canvas>().enabled = true;
        yield return new WaitForSeconds(0.25f);
        float targetPowerAmount = player.power - drainAmount;
        float drainPerSecond = (float)drainAmount / duration;
        float fps = 1.0f / Time.unscaledDeltaTime;
        float drainPerFrame = drainPerSecond / fps;
        while(player.power > targetPowerAmount && player.power != 0)
        {
            while(GameManager.instance.gameIsPaused)
            {
                yield return new WaitForSeconds(0.25f);
            }
            player.power -= drainPerFrame;
            yield return null;
        }
        berzerkActive = false;
        GameObject.Find("BerzerkCanvas").GetComponent<Canvas>().enabled = false;
        yield break;
    }

    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if(colliders.Length > 0)
        {
            isGrounded = true;
            if(!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJump = false;
            }
        } else {
            if(wasGrounded)
                StartCoroutine(CoyoteJumpDelay());
        }
        animator.SetBool("Jump", !isGrounded);
    }

    IEnumerator CoyoteJumpDelay()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(0.2f);
        coyoteJump = false;
    }

    void Jump()
    {
        if(isGrounded && !crouchPressed)
        {
            isRunning = false;
            multipleJump = true;
            availableJumps--;
            FindObjectOfType<AudioManager>().Play("Jump");
            rb.velocity = Vector2.up * jumpPower;
            animator.SetBool("Jump", true);
        } else {
            if(coyoteJump)
            {
                isRunning = false;
                multipleJump = true;
                availableJumps--;
                FindObjectOfType<AudioManager>().Play("Jump");
                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
                Debug.Log("Coyote Jump!");
            }
            if(multipleJump && availableJumps > 0)
            {
                isRunning = false;
                availableJumps--;
                FindObjectOfType<AudioManager>().Play("Jump");
                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
                player.power -= 2f;
            }
        }
    }
    void Move(float dir, bool crouchFlag)
    {
        #region Crouch
        if(!crouchFlag)
        {
            if(Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, groundLayer))
                crouchFlag = true;
        }
        animator.SetBool("Crouch", crouchFlag);
        standingCollider.enabled = !crouchFlag;
        #endregion
        #region Move & Run
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        if(isRunning && !crouchFlag)
            xVal *= runSpeedModifier;
        if(crouchFlag)
            xVal *= crouchSpeedModifier;
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        rb.velocity = targetVelocity;

        Vector3 currentScale = transform.localScale;
        if(facingRight && dir<0)
        {
            currentScale.x = currentScale.x - (currentScale.x * 2);
            facingRight = false;
        }
        else if(!facingRight && dir > 0)
        {
            currentScale.x = currentScale.x - (currentScale.x * 2);
            facingRight = true;
        }
        transform.localScale = currentScale;
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        #endregion
    }
}
