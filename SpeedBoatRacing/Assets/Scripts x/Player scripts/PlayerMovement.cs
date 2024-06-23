using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float speedRotation;

    Rigidbody rb;

    //float health = 1;

    bool running = true;
    bool controling = false;

    public Transform frontShocPos;

    bool goLeft = false;
    bool goRight = false;

    //public Transform body;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (running)
        {
            rb.velocity = transform.forward * speed;
            //rb.AddRelativeForce(transform.forward * speed * 10);
        }
        if (controling)
        { 
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
            }
            if (Input.GetKey(KeyCode.LeftArrow) || goLeft)
            {
                transform.Rotate(transform.up, -speedRotation);
                rb.AddRelativeForce(transform.right * -speed);
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
            }


            if (Input.GetKeyDown(KeyCode.RightArrow))
            {

            }
            if (Input.GetKey(KeyCode.RightArrow) || goRight)
            {
                transform.Rotate(transform.up, speedRotation);
                rb.AddRelativeForce(transform.right * speed);

            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
            }
        }
    }

    /*public void GameOver()
    {
        gameObject.tag = "Untagged";

        running = false;
        controling = false;

        rb.constraints = RigidbodyConstraints.None;
        rb.angularDrag = 1;
        

        rb.AddExplosionForce(1000, frontShocPos.position - new Vector3(0, .5f, 2), 2f);

        dustEffect.SetActive(false);
    }*/

    public void startControling()
    {
        controling = true;
    }
    
    public void StopMoving()
    {
        running = false;
        controling = false;

        rb.constraints = RigidbodyConstraints.None;
        rb.angularDrag = 0.1f;


        rb.AddExplosionForce(350, frontShocPos.position - new Vector3(0, 1f, 0), 2f);
        rb.AddTorque(transform.right * 70 );

        //rb.angularDrag = 1f;
    }

    public void LeftTouch()
    {
        goLeft = true;
    }
    public void LeftRelease()
    {
        goLeft = false;
    }

    public void RightTouch()
    {
        goRight = true;
    }
    public void RightRelease()
    {
        goRight = false;
    }

}
