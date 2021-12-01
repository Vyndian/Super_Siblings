    using UnityEngine;

/*
 * Camera will follow the player.
 */

//[RequireComponent(typeof(Rigidbody2D))]
public class BasicCamera : MonoBehaviour
{
    private Vector2 playerPosition;
    private GameObject[] colliders;
    public bool isStatic;
    public GameObject player;
    private float zPosition;

    public bool endGame = false;

    void Start()
    {
        float height = 2f * GetComponent<Camera>().orthographicSize;
        float width = height * GetComponent<Camera>().aspect;
        zPosition = Camera.main.transform.position.z;

        player = GameObject.FindGameObjectWithTag("Player");
        colliders = new GameObject[4];
        colliders[0] = new GameObject("Top", (typeof(BoxCollider2D)));
        colliders[1] = new GameObject("Right", (typeof(BoxCollider2D)));
        colliders[2] = new GameObject("Bottom", (typeof(BoxCollider2D)));
        colliders[3] = new GameObject("Left", (typeof(BoxCollider2D)));
        colliders[0].GetComponent<BoxCollider2D>().isTrigger = true;
        colliders[1].GetComponent<BoxCollider2D>().isTrigger = true;
        colliders[2].GetComponent<BoxCollider2D>().isTrigger = true;
        colliders[3].GetComponent<BoxCollider2D>().isTrigger = true;
        colliders[0].transform.position = new Vector3(0, height / 2 + 0.5f, 0); colliders[0].transform.localScale = new Vector3(width, 1, 1); colliders[0].transform.SetParent(transform); colliders[0].layer = 9;
        colliders[1].transform.position = new Vector3(width / 2 + 0.5f, 0, 0); colliders[1].transform.localScale = new Vector3(1, height, 1); colliders[1].transform.SetParent(transform); colliders[1].layer = 9;
        colliders[2].transform.position = new Vector3(0, -height / 2 - 0.5f, 0); colliders[2].transform.localScale = new Vector3(width, 1, 1); colliders[2].transform.SetParent(transform); colliders[2].layer = 9;
        colliders[3].transform.position = new Vector3(-width / 2 - 0.5f, 0, 0); colliders[3].transform.localScale = new Vector3(1, height, 1); colliders[3].transform.SetParent(transform); colliders[3].layer = 9;
    }
    void Update()
    {
        if (!isStatic && !endGame)
        {
            Camera.main.transform.position = new Vector3
                (
                    player.transform.position.x,
                    player.transform.position.y,
                    zPosition
                );
        }
    }
}
