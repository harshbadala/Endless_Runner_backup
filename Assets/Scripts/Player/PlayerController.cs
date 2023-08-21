using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterController controller;
    public float forwardSpeed;
    public float maxSpeed;
    private Vector3 direction;

    public bool isGrounded;

    private int desiredLane = 1;//0:left 1:middle 2:right
    public float laneDistance = 4; //the distance between two lanes

    public float jumpForce;
    public float Gravity = -20;

    public Animator animator;
    private bool isSliding = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerManager.isGameStarted)
            return;
        if(forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;
        }
        
        animator.SetBool("isGameStarted", true);
        direction.z = forwardSpeed;

     //   isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        if(controller.isGrounded)
        {
            direction.y = -1;
            if(SwipeManager.swipeUp)
            {
                animator.SetBool("isGrounded", isGrounded);
                Jump();
            }
        } else
            animator.SetBool("isGrounded", !isGrounded);
            direction.y += Gravity * Time.deltaTime;
        
        if(SwipeManager.swipeDown && !isSliding)
        {
            StartCoroutine(Slide());
        }
        //Gather the inputes on which lane we should be

        if(SwipeManager.swipeRight)
        {
            desiredLane++;
            if(desiredLane == 3)
                desiredLane = 2;                
        }

         if(SwipeManager.swipeLeft)
        {
            desiredLane--;
            if(desiredLane == -1)
                desiredLane = 0;                
        }

        //Calculate where we should be in the future

         Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

         if(desiredLane == 0)
         {
            targetPosition += Vector3.left * laneDistance;
         } else if (desiredLane ==2)
         {
            targetPosition += Vector3.right * laneDistance;
         }
         
       //  transform.position = Vector3.Lerp(transform.position, targetPosition, 80 * Time.deltaTime);
       if(transform.position == targetPosition)
            return;
       Vector3 diff = targetPosition - transform.position;
       Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
       if(moveDir.sqrMagnitude < diff.sqrMagnitude)
       {
            controller.Move(moveDir);
       }
       else
            controller.Move(diff);

    }

    private void FixedUpdate()
    {
        if(!PlayerManager.isGameStarted)
        {
            return;
        }
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;
        yield return new WaitForSeconds(1.3f);

        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
        isSliding = false;
    }

}
