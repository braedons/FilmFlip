using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb;
    private Animator animator;

    // Horizontal movement
    public float speed = 75f;
    private float horizontalComponent;
    private bool facingRight = true;

    // Jumping
    public float jumpForce = 400f;
    private float verticalComponent;
    public LayerMask whatIsGround;
    public GameObject groundCheck;
    public float groundCheckRadius = 16f;
    private bool isGrounded;

    // Pulling objects
    public string whatIsPullable;
    public GameObject pullCheck;
    public float pullCheckRadius = 16f;
    private FixedJoint2D pulledObjJoint = null;

    // Controls
    KeyCode jump = KeyCode.Z;
    KeyCode pull = KeyCode.X;

    private LevelManager levelManager;
    private AudioSource running;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        running = GetComponent<AudioSource>();
    }

    void Update() {
        // Horizontal movement
        horizontalComponent = Input.GetAxis("Horizontal") * speed;
        animator.SetFloat("VelocityAbs", Mathf.Abs(horizontalComponent));

        // Jump
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius, whatIsGround);
        if (Input.GetKeyDown(jump) && isGrounded) {
            verticalComponent = jumpForce;
            isGrounded = false;
            animator.SetTrigger("Jump");
        }
        else
            verticalComponent = rb.velocity.y;

        if (pulledObjJoint == null && 
                ((facingRight && horizontalComponent < 0) || (!facingRight && horizontalComponent > 0)))
            Flip();

        // Set movement
        rb.velocity = new Vector2(horizontalComponent, verticalComponent);

        // Pull crates
        Collider2D pulledObj = Physics2D.OverlapCircle(pullCheck.transform.position, pullCheckRadius, whatIsGround);
        if (Input.GetKeyDown(pull)) {
            
            if (pulledObj != null && pulledObj.CompareTag(whatIsPullable)) {
                pulledObjJoint = pulledObj.GetComponent<FixedJoint2D>();
                pulledObjJoint.enabled = true;
                pulledObjJoint.connectedBody = rb;
            }
        }
        else if (Input.GetKeyUp(pull) && pulledObjJoint != null) {
            pulledObjJoint.enabled = false;
            pulledObjJoint = null;
        }
        animator.SetBool("Pushing", pulledObj && (horizontalComponent * transform.localScale.x) > 0);
        animator.SetBool("Pulling", pulledObj && (horizontalComponent * transform.localScale.x) < 0);

        if (Mathf.Abs(horizontalComponent) > 0 && !running.isPlaying && isGrounded)
            running.Play();
        else
            running.Stop();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("End") && !levelManager.IsRewinding())
            levelManager.ReachedGoal("End");
        else if (other.CompareTag("Start") && levelManager.IsRewinding()) {
            levelManager.ReachedGoal("Start");
        }
    }

    private void Flip() {
        facingRight = !facingRight;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}
