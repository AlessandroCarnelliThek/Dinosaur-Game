using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoController : MonoBehaviour
{
    [Header("DINO COLLIDERS")]
    [SerializeField] PolygonCollider2D defaultCollider2D;
    [SerializeField] BoxCollider2D crouchCollider2D;

    [Header("GROUND")]
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform groundCheck;
    private float groundedRradius = 0.02f;
    private bool grounded = false;
    private bool wasgrounded = false;

    public bool isLanding = false;
    public bool isDeath = false;

    private Rigidbody2D rb2D;
    [SerializeField] float jumpForce = 300;

    [SerializeField] Transform DinoAncorPoint;

    // GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundedRradius);
    }

    // AWAKE
    private void Awake() { 
        rb2D = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(DinoAncorPoint.position.x, transform.position.y, transform.position.z);
    }


    public void GroundCheck()
    {
        wasgrounded = grounded;
        grounded = false;
        if (Physics2D.OverlapCircle(groundCheck.position, groundedRradius, whatIsGround))
        {
            grounded = true;
            if (!wasgrounded)
            {
                Debug.Log("LANDING");
                isLanding = true;
            }
        }
    }
    public void Move(ref bool jump, ref bool crouch)
    {
        if (crouch)
        {
            defaultCollider2D.enabled = false;
            crouchCollider2D.enabled = true;
        }
        else
        {
            crouchCollider2D.enabled = false;
            defaultCollider2D.enabled = true;
        }
        if (jump)
        {
            rb2D.AddForce(new Vector2(0f, jumpForce));
            AudioManager.instance.Play("jump");
            jump = false;
        }
        if (isDeath)
        {
            GameManager.instance.UpdateGameState(GameState.GAMEOVER);
        }
    }

    public void SmashDown() { rb2D.gravityScale = 20; }
    public void NormalizeGravity() { rb2D.gravityScale = 3; }

    public void SetIsDeath(bool var) 
    { 
        isDeath = var; 
    }
}

