using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if(horizontalMove == 0)
        {
            anim.SetBool("isRunning", false);
        } else {
            anim.SetBool("isRunning", true);
        }
        if(Input.GetButtonDown("Jump"))
        {
            anim.SetBool("isRunning", false);
            anim.SetTrigger("takeOff");
            jump = true;
            anim.SetBool("isJumping", true);
        }
        if(Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if(Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

    }
}
