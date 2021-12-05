using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoMovement : MonoBehaviour
{
    private DinoController controller;
    private Animator anim;

    private bool jump;
    private bool isJumping;
    private bool crouch;
    private bool isWaitingToCrouch;

    private void Start()
    {
        controller = GetComponent<DinoController>();
        anim = GetComponent<Animator>();


        jump = false;
        isJumping = false;
        crouch = false;
        isWaitingToCrouch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isDeath)
        {
            anim.Play("dino_death");
            Debug.Log("DEAD");
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (!isJumping)
                {
                    crouch = true;
                    anim.Play("dino_crouch");
                    Debug.Log("CROUCH");
                }
                else
                {
                    isWaitingToCrouch = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                if (!isJumping)
                {
                    crouch = false;
                    anim.Play("dino_run");
                    Debug.Log("RUN");
                }
                else
                {
                    isWaitingToCrouch = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (!isJumping)
                {
                    crouch = false;
                    jump = true;
                    isJumping = true;
                    anim.Play("dino_jump");
                    Debug.Log("JUMP");
                }
            }

            if (controller.isLanding)
            {
                isJumping = false;
                controller.isLanding = false;

                if (isWaitingToCrouch)
                {
                    crouch = true;
                    anim.Play("dino_crouch");
                    Debug.Log("CROUCH");
                    isWaitingToCrouch = false;
                }
            }
            if (!crouch && !isJumping)
            {
                anim.Play("dino_run");
                Debug.Log("RUN");
            }

        }
    }
    private void FixedUpdate()
    {
        controller.Move(ref jump, ref crouch);
    }
}
