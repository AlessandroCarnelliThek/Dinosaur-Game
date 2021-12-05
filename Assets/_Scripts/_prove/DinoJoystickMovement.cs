using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoJoystickMovement : MonoBehaviour
{
    private DinoController controller;
    private Animator anim;

    private bool jump;
    private bool isJumping;
    private bool crouch;
    private bool isWaitingToCrouch;

    public Joystick joystick;

    private float verticalMove;

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
        verticalMove = joystick.Vertical;
        Debug.Log(verticalMove);
        if (controller.isDeath)
        {
            anim.Play("dino_death");
            Debug.Log("DEAD");
        }
        else
        {
            if (verticalMove < 0)
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
            else if (verticalMove > 0)
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
            else if (verticalMove == 0)
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
            /*
            if (!crouch && !isJumping)
            {
                anim.Play("dino_run");
                Debug.Log("RUN");
            }
            */

        }
    }
    private void FixedUpdate()
    {
        controller.Move(ref jump, ref crouch);
    }
}
