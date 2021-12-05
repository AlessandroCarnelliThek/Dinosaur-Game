using System;
using System.Collections;
using UnityEngine;

public class DinoCanvasTouchMovement : MonoBehaviour
{
    [SerializeField] JumpButton jumpButton;
    [SerializeField] CrouchButton crouchButton;

    private Animator anim;
    private DinoController controller;

    private bool jump = false;
    private bool crouch = false;
    private bool isJumping = false;
    private bool isWaitingToCrouch = false;

    // START
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<DinoController>();

        crouchButton.onCrouchButtonDown.AddListener(CrouchButtonPressedDown);
        crouchButton.onCrouchButtonUp.AddListener(CrouchButtonPressedUp);
        jumpButton.onJumpButtonDown.AddListener(JumpButtonPressedDown);

        // serve solo nel main
        anim.Play("greenDino_idle");
    }

    // FIXED UPDATE
    private void FixedUpdate()
    {
        if (!controller.isDeath)
        {
            controller.GroundCheck();
            controller.Move(ref jump, ref crouch);
        }
    }

    // UPDATE DINO
    public void UpdateDino()
    {
        if (!controller.isDeath)
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
                else { anim.Play("greenDino_run"); }
            }
        }
        else { anim.Play("greenDino_death"); }
    }

    // ON ENABLE
    //public void OnEnable() { anim?.Play("greenDino_idle"); }

    // ON DISABLE
    //public void OnDisable() { anim.Play("greenDino_death"); }

    // JOYSTICK
    public void CrouchButtonPressedDown()
    {
        if (!isJumping)
        {
            crouch = true;
            anim.Play("greenDino_crouch");
        }
        else
        {
            controller.SmashDown();
            isWaitingToCrouch = true;
        }
    }
    public void CrouchButtonPressedUp()
    {
        if (!isJumping)
        {
            crouch = false;
            anim.Play("greenDino_run");
        }
        else
        {
            controller.NormalizeGravity();
            isWaitingToCrouch = false;
        }
    }
    public void JumpButtonPressedDown()
    {
        if (!isJumping)
        {
            crouch = false;
            isJumping = true;
            jump = true;
            anim.Play("greenDino_jump");
        }
    }

    // INTRO ANIMATION
    public void StartDinoIntroAnimation(Action callback)
    {
        StartCoroutine(DinoIntroAnimation(callback));
    }
    IEnumerator DinoIntroAnimation(Action callback)
    {
        float timer = 0;
        float duration = 0.4f;
        Vector3 startPoint = transform.position;
        Vector3 endPoint = new Vector3(transform.position.x + 0.5f, 0, 0);

        JumpButtonPressedDown();
        yield return new WaitForSeconds(0.7f);

        callback();
        GameManager.instance.StartDino();

        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPoint, endPoint, timer / duration);
            yield return null;
        }
    }
}
