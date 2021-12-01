using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoundSorceress : MonoBehaviour
{
    [SerializeField, Tooltip("The GameObject that tells the player they won. On the UI, and disabled until victory.")]
    private GameObject victoryText;
    [SerializeField, Tooltip("The exact name of the scene to load after victory, like credits.")]
    private string sceneToLoad = "Credits";
    [SerializeField,Tooltip("Amount of time to wait after victory before loading credits")]
    private float creditsDelay = 4.0f;

    // When a collider hits the Sorceress's circle collider,
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if it was the player.
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            // Then the player has completed the level, he found his sister!
            // Start the coroutine for victory.
            StartCoroutine("Victory");
        }
    }

    IEnumerator Victory()
    {
        // Turn on the victory text object.
        victoryText.SetActive(true);
        // TODO: Maybe play sound here.

        // Set timer for the delay.
        float timer = 0.0f;

        // Until a certain amount of time has passed,
        while (timer < creditsDelay)
        {
            // increase the timer by time,
            timer += Time.deltaTime;
            // and yield a return of null.
            yield return null;
        }

        // Once timer complete, load the credits.
        SceneManager.LoadScene(sceneToLoad);
    }
}
