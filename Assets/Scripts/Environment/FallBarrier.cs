using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBarrier : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    // When the player hits this BoxCollider2D (when they fall off the map),
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if it was the player,
        if (collision.gameObject.GetComponent<Player>())
        {
            // and remove all velocity (stop them from moving) and move them back to the spawn point.
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.gameObject.transform.position = spawnPoint.position;
        }
    }
}
