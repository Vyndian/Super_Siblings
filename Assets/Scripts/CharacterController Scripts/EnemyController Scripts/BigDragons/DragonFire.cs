using UnityEngine;

public class DragonFire : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 3000.0f;

    private Animator animator;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the collider is the player,
        if (collision.gameObject.GetComponent<Player>())
        {
            // If the player has their shield up,
            if (collision.gameObject.GetComponent<Player>().ConsumeShield())
            {
                // then their shield has now been consumed. Disable the fire gameObject.
                gameObject.SetActive(false);
            }
            // Else, shield was not up. Knock the player back.
            else
            {
                Debug.Log("Shield was not up. Fire should now push back against player.");
                // First, get the rigidbody.
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                
                // Then determine if player is to the right or left of the fire.
                if (collision.gameObject.transform.position.x < gameObject.transform.position.x + 2)
                {
                    // Player is to the left of the fire. Push them left.
                    rb.velocity = Vector3.zero;
                    rb.AddForce(new Vector2(-knockbackForce, 0.0f));
                }
                else
                {
                    // Player is to the right of the fire. Push them right.
                    rb.velocity = Vector3.zero;
                    rb.AddForce(new Vector2(knockbackForce, 0.0f));
                }
            }
        }
    }
}
