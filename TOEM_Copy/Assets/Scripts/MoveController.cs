using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    //public SpriteRenderer SpriteRenderer;
    public Rigidbody Rig;
    //public Animator AnimatorMove;
    //public Animator AnimatorFlip;


    public float MoveSpeed;
    //public float JumpForce;
    //public float GroundRayLength;
    //public LayerMask WhatIsGround;
    //public Transform GroundPoint;

    //private bool isGround;
    private Vector2 _moveInput;
    //private bool moveingBackords;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput.x = Input.GetAxis("Horizontal");
        _moveInput.y = Input.GetAxis("Vertical");
        _moveInput.Normalize();

        Rig.velocity = new Vector3(_moveInput.x * MoveSpeed, Rig.velocity.y, _moveInput.y * MoveSpeed);
        //AnimatorMove.SetFloat("moveSpeed", Rig.velocity.magnitude);

        //Debug.DrawRay(GroundPoint.position, Vector3.down, Color.green, GroundRayLength);
        //RaycastHit hit;
        //if (Physics.Raycast(GroundPoint.position, Vector3.down, out hit, GroundRayLength))
        //{
        //    isGround = true;
        //}
        //else
        //{
        //    isGround = false;
        //}

        //if (Input.GetButtonDown("Jump") && isGround)
        //{
        //    Rig.velocity += new Vector3(0, JumpForce, 0);
        //}
        //AnimatorMove.SetBool("onGround", isGround);

        //if (!SpriteRenderer.flipX && _moveInput.x < 0)
        //{
        //    SpriteRenderer.flipX = true;
        //    AnimatorFlip.SetTrigger("Flip");
        //}
        //else if (SpriteRenderer.flipX && _moveInput.x > 0)
        //{
        //    SpriteRenderer.flipX = false;
        //    AnimatorFlip.SetTrigger("Flip");
        //}

        //if (!moveingBackords && _moveInput.y > 0)
        //{
        //    moveingBackords = true;
        //    AnimatorFlip.SetTrigger("Flip");
        //}
        //else if (moveingBackords && _moveInput.y < 0)
        //{
        //    moveingBackords = false;
        //    AnimatorFlip.SetTrigger("Flip");
        //}
        //AnimatorMove.SetBool("movingBackwards", moveingBackords);

    }


}
