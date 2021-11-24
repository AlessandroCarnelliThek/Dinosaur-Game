using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoTouchMovement : MonoBehaviour
{
    private DinoController controller;
    private Animator anim;

    private Touch jumpTouch;
    private Touch crouchTouch;

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

        if (controller.isDead)
        {
            anim.Play("dino_death");
            Debug.Log("DEAD");
        }
        else
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.touches[i];
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0f;

                if (touchPosition.x > 0)
                {
                    jumpTouch = touch;
                    if (!isJumping)
                    {
                        crouch = false;
                        jump = true;
                        isJumping = true;
                        anim.Play("dino_jump");
                        Debug.Log("JUMP");
                    }
                }
                else if(touchPosition.x < 0)
                {
                    crouchTouch = touch;
                }

               

            }
            switch (crouchTouch.phase)
            {

                case TouchPhase.Began:
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
                    Debug.Log("CROUCH down");
                    break;

                case TouchPhase.Ended:
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
                    Debug.Log("CROUCH up");
                    break;
            }


            if (controller.isLanding)
            {
                isJumping = false;

                if (isWaitingToCrouch)
                {
                    crouch = true;
                    anim.Play("dino_crouch");
                    Debug.Log("CROUCH");
                    isWaitingToCrouch = false;
                }
                else
                {
                    anim.Play("dino_run");
                    Debug.Log("RUN");
                }

                controller.isLanding = false;

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
