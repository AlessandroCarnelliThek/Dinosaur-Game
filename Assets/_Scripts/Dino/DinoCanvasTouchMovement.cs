using System;
using System.Collections;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DinoCanvasTouchMovement : MonoBehaviour
{
    [SerializeField] CrouchButton crouchButton;
    [SerializeField] JumpButton jumpButton;

    private DinoController controller;
    private Animator anim;

    private bool jump = false;
    private bool isJumping = false;
    private bool crouch = false;
    private bool isWaitingToCrouch = false;

    /*private enum AnimationState
    {
        IDLE,
        RUN,
        JUMP,
        CROUCH,
        DEATH,
    }*/
    private AnimationState animationState;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<DinoController>();
        anim = GetComponent<Animator>();

        crouchButton.onCrouchButtonDown.AddListener(CrouchButtonPressedDown);
        crouchButton.onCrouchButtonUp.AddListener(CrouchButtonPressedUp);
        jumpButton.onJumpButtonDown.AddListener(JumpButtonPressedDown);

        //animationState = AnimationState.IDLE;
    }

    // Update is called once per frame
    public void UpdateDino()
    {
        if (GameManager.instance.dinoIsRunning)
        {
            if (!controller.isDead)
            {
                if (controller.isLanding)
                {
                    isJumping = false;
                    controller.isLanding = false;

                    if (isWaitingToCrouch)
                    {
                        crouch = true;
                        controller.NormalizeGravity();
                        anim.Play("greenDino_crouch");


                        isWaitingToCrouch = false;
                    }
                    else
                    {
                        anim.Play("greenDino_run");
                    }
                }
            }
            else
            {
                anim.Play("greenDino_death");
            }
        }
        else
        {
            anim.Play("greenDino_idle");
        }
    }
    
    private void FixedUpdate()
    {
        controller.GroundCheck();
        controller.Move(ref jump, ref crouch);
    }
    

    public void CrouchButtonPressedDown()
    {
        if (!controller.isDead)
        {
            if (!isJumping)
            {
                crouch = true;
                //animationState = AnimationState.CROUCH;
                anim.Play("greenDino_crouch");

            }
            else
            {
                controller.SmashDown();
                isWaitingToCrouch = true;
            }
        }
    }
    public void CrouchButtonPressedUp()
    {
        if (!controller.isDead)
        {
            if (!isJumping)
            {
                crouch = false;
                //animationState = AnimationState.RUN;
                anim.Play("greenDino_run");

            }
            else
            {
                controller.NormalizeGravity();
                isWaitingToCrouch = false;
            }
        }
    }
    public void JumpButtonPressedDown()
    {
        if (!controller.isDead)
        {
            if (!isJumping)
            {
                crouch = false;
                isJumping = true;
                jump = true;
                //animationState = AnimationState.JUMP;
                anim.Play("greenDino_jump");

            }
        }
    }
    public void StartDinoIntroAnimation()
    {
        StartCoroutine(DinoIntroAnimation());
    }
    IEnumerator DinoIntroAnimation()
    {
        float timer = 0;
        float duration = 0.4f;
        Vector3 startPoint = new Vector3(-5, 0, 0);
        Vector3 endPoint = new Vector3(-4.5f, 0, 0);

        JumpButtonPressedDown();
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.StartDino();

        while(timer < duration)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPoint, endPoint, timer / duration);
            yield return null;
        }
    }
}
