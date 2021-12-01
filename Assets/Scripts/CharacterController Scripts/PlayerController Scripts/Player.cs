using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
 * The player! Knight in this case. I was going to have a base player class and then knight subclass,
 * but there is no point right now. We can change it later if we ever come back to this project, which is unlikely.
 */

public class Player : MonoBehaviour
{
    #region Fields
    // Public ~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    // Private Serialized ~~~~~~~~~~~~~~~~~

    [Header("Movement")]
    // The speed of the character. Exposed so we can change it in inspector.
    //[SerializeField] private float speed = 3.0f;
    // The amount of force that should be added to the player when they jump.
    [SerializeField] private float jumpForce = 1000f;
    // The amount of force that should be added to the player when they move left or right.
    [SerializeField] private float moveForce = 365f;
    // Transform of a gameObject childed to the character. Sits below the feet of the player at all times.
    // If ground is between player and this, player is grounded.
    [SerializeField] private Transform groundCheck;

    [Header("Abilities")]
    // How long, in seconds, the shield stays on before dissappearing if not used.
    [SerializeField] private float shieldDuration = 3.0f;
    // How long, in seconds, it takes for the shield ability to recharge once consumed or once it times out.
    [SerializeField] private float shieldRechargeRate = 3.0f;
    [SerializeField] private Image shieldCooldownRadial;
    [SerializeField] private GameObject shield;

    // Private ~~~~~~~~~~~~~~~~~~~~~~~~~~

    // References to the player's Rigidbody2D and their Animator.
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    // Horizontal and Vertical are used for player input for movement.
    private float horizontal = 0.0f;
    // Whether or not the player needs to jump in FixedUpdate.
    private bool jump = false;
    // Whether or not the player is currently on the ground (not jumping).
    private bool grounded;

    // If the shield is currently protecting the player.
    private bool shieldUp = false;
    // If the shield is currently available for use, but not on.
    private bool shieldAvailable = true;
    #endregion Fields


    #region Unity Methods
    // Runs like Start, but also for every time the gameObject is re-enabled.
    private void OnEnable()
    {
        // Find and save refereces to the rigidbody and the animator.
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Use update for anything not related to physics (like movement of rigidbodies) that needs to be constantly updated.
    private void Update()
    {
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        // Get the player's current input for x and y axes, send it to the animator.
        horizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("MoveX", horizontal);

        // If player pressing up, W, or spacebar,
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // and the player is grounded,
            if (grounded)
            {
                // Check if the player is looking left.
                if (horizontal < 0)
                {
                    // If so, tell animator to flip the jump animation.
                    animator.SetBool("FlipJump", true);
                }
                // Else, reset to not flip jump animation.
                else
                {
                    animator.SetBool("FlipJump", false);
                }
                // Tell the animator to jump.
                animator.SetTrigger("Jump");
                // Player will now jump during FixedUpdate
                jump = true;
            }
        }
        // Else, they are not trying to jump.
        else
        {

        }

        // If they are pressing the left- or right-click on the mouse and the shield is available,
        if (shieldAvailable && (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)))
        {
            // Activate the shield. Enable the GameObject.
            shieldUp = true;
            shieldAvailable = false;
            shield.SetActive(true);
            // Start the shield duration timer.
            StartCoroutine(StartShieldDuration());
        }
    }

    // Use FixedUpdate to do any updates involving physics, like movement of rigidbodies.
    private void FixedUpdate()
    {
        // If jump is true from Update checks,
        if (jump)
        {
            // Add a vertical force to the player (jump), then reset jump to false
            rigidbody2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

        // Move the player based on what Update determined.
        rigidbody2d.AddForce(Vector2.right * horizontal * moveForce);
    }
    #endregion Unity Methods


    #region Dev-Defined Methods
    private IEnumerator StartShieldDuration()
    {
    // The timer for how long it has been since the shield was activated.
    float timer = 0.0f;
        // Until the timer runs out,
        while (timer < shieldDuration)
        {
            // Check if the shield has already been disabled (consumed).
            if (!shieldUp)
            {
                // If so, break from this loop.
                break;
            }
            // Add to the timer and wait.
            timer += Time.deltaTime;
            yield return null;
        }
        // Once the timer is done or the shield is consumed, deactivate the shield.
        shieldUp = false;
        shield.SetActive(false);
        // Start the timer for the recharge.
        StartCoroutine(StartShieldRecharge());
    }

    private IEnumerator StartShieldRecharge()
    {
        // Set the cooldoan radial (for the UI) to full.
        shieldCooldownRadial.fillAmount = 1;
        // The timer for how long it has been since the shield was used/timed out.
        float timer = 0.0f;

        // Until the timer runs out,
        while (timer < shieldRechargeRate)
        {
            // Gradually lower the cooldown radial,
            shieldCooldownRadial.fillAmount = 1 - (timer / shieldRechargeRate);
            // Add to the timer, and wait.
            timer += Time.deltaTime;
            yield return null;
        }
        // Once timer runs out, make the shield available again.
        shieldAvailable = true;
        // Finish the cooldown radial.
        shieldCooldownRadial.fillAmount = 0;
    }

    // Called by fire when it hits the shield's edge colliders.
    public bool ConsumeShield()
    {
        // If shield is up,
        if (shieldUp)
        {
            // Shield has been consumed. Return true, shield was consumed.
            shieldUp = false;
            return true;
        }
        // Else, return false. Shield was no consumed.
        else
        {
            return false;
        }
        // Setting shieldUp to false will break that loop and disable the shield.
    }
    #endregion Dev-Defined Methods
}
