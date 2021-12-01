using UnityEngine;

public class DragonWarrior : MonoBehaviour
{
    // The amount of time the warrior will walk in one directing before flipping patrol direction.
    [SerializeField, Tooltip("The amount of time the warrior will walk in one directing before flipping patrol direction.")]
    private float patrolDuration = 4.0f;
    // The speed at which the enemy moves.
    [SerializeField, Tooltip("The speed at which the enemy moves.")]
    private float moveSpeed = 2.0f;
    [SerializeField, Tooltip("The amount of force added to player when enemy pushed them.")]
    private float pushForce = 3000.0f;

    private Rigidbody2D thisRB;
    private Animator animator;
    // The amount of time that has passed since the enemy last started patroling in this direction.
    public float timePatroling;

    
    // Start is called before the first frame update
    void Start()
    {
        thisRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Add to the timer since the patrol began.
        timePatroling += Time.deltaTime;
        // If the timer has reached or passed the patrolDuration,
        if (timePatroling >= patrolDuration)
        {
            // the flip the localScale.x,
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            // and reset the timer.
            timePatroling = 0.0f;
        }

        if (IsFacingRight())
        {
            thisRB.velocity = new Vector2(moveSpeed, 0f);
        }

        else
        {
            thisRB.velocity = new Vector2(-moveSpeed, 0f);
        }

        if (timePatroling == 0)
        {

        }
    }
    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-Mathf.Sign(thisRB.velocity.x), 1f);
    }

    // Called when the enemy collides with another collider.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(gameObject.name + " collided with " + collision.gameObject.name);
        // If what they hit is the player,
        if (collision.gameObject.GetComponent<Player>())
        {
            Debug.Log(collision.gameObject.name + " is the player.");

            // Get the player script.
            Player player = collision.gameObject.GetComponent<Player>();
            // Check if their shield is up. If so, shield will be consumed.
            if (player.ConsumeShield())
            {
                // If shield was up (shield is consumed while checking), DO NOT push the player.
            }
            // Else, the player had no shield. Push them.
            else
            {
                // Get the player's rigidbody.
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                // Add push force to it.
                rb.AddForceAtPosition
                    (
                        new Vector2((pushForce * transform.localScale.x), 0.0f),
                        collision.gameObject.transform.position
                    );
            }
            // Tell the animator to perform the kick attack (whether or not shield was consumed and player pushed).
            animator.SetTrigger("Attack");
        }
    }
}
