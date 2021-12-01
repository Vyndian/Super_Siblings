using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Sorceress : MonoBehaviour
{
    //config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;

    //state
    bool isAlive = true;

    //cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;

    //Messages then methods
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Run();
        Jump();
        FlipSprite();
    }

    private void Run()
    {
        //value between -1 and + 1
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 sorceressVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = sorceressVelocity;

        bool sorceressHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", sorceressHasHorizontalSpeed);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
         }

    }
    private void FlipSprite()
    {
        bool sorceressHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (sorceressHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }
}
