using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float mouvementSpeed = 5.0f;
    [SerializeField] float jumpSpeed = 5.0f;
    [SerializeField] float mass = 1.0f;
    [SerializeField] Transform cameraTransform;
    Animator anim;
    CharacterController controller;
    Vector3 velocity;
    Vector2 look;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        UpdateGravity();
        UpdateMovement();
        UpdateLook();
    }

    void UpdateGravity()
    {
        var gravity = Physics.gravity * mass * Time.deltaTime;
        velocity.y = controller.isGrounded ? -1.0f : velocity.y + gravity.y;
    }

    void UpdateMovement()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        AnimPlayer();

        var input = new Vector3();
        input += transform.forward * y;
        input += transform.right * x;
        input = Vector3.ClampMagnitude(input, 1.0f);

        controller.Move((input * mouvementSpeed + velocity) * Time.deltaTime);

    }

    void UpdateLook()
    {
        look.x += Input.GetAxis("Mouse X");
        look.y += Input.GetAxis("Mouse Y");
        look.y = Mathf.Clamp(look.y, -89.0f, 89.0f);

        cameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, look.x, 0);
    }

    void AnimPlayer()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");   

        if(controller.isGrounded)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                anim.SetTrigger("defend");
            }

            if(Input.GetButtonDown("Jump") )
            {
                velocity.y += jumpSpeed * 0.4f;
            }

            //Anim position
            if(y > 0.1f)
            {
                anim.SetBool("isWalking", true);
            } 
            else if(y < -0.1f)
            {
                anim.SetBool("movingBackward", true);
            } 
            else if(y <= 0.1f && y >= -0.1f)
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("movingBackward", false);
            }

            if(x > 0.1f)
            {
                anim.SetBool("movingRight", true);
            } 
            else if(x < -0.1f)
            {
                anim.SetBool("movingLeft", true);
            } 
            else if(x <= 0.1f && x >= -0.1f)
            {
                anim.SetBool("movingRight", false);
                anim.SetBool("movingLeft", false);
            }
        }

        if(GetComponent<PlayerCollision>().megaJump)
        {
            velocity.y += jumpSpeed;
            GetComponent<PlayerCollision>().megaJump = false;
        }       
    }
}
