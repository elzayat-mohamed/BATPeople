using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour 
{

    private Rigidbody2D rigidBody;
    public Vector2 moveSpeed;
    private bool IsActive;
    
    void Awake() 
    {
        this.rigidBody = this.GetComponent<Rigidbody2D>();
        this.rigidBody.isKinematic = true;

        this.IsActive = false;
	}
	
	void FixedUpdate () 
    {
        if (IsActive)
        {
            Move();
            Leaning();
        }
	}

    public void Jump()
    {
        if(IsActive)
        {
            this.rigidBody.velocity = new Vector2(0,moveSpeed.y);// moveSpeed;
            leaningTimePassed = 0;
        }
    }


    public void Enable()
    {
        this.rigidBody.isKinematic = false;
        this.IsActive = true;

        this.rigidBody.velocity = moveSpeed + new Vector2(0, 10f);

        leaningTimePassed = 0;
    }
    public void Disable()
    {
        this.rigidBody.isKinematic = true;
        this.IsActive = false;

        this.rigidBody.velocity = Vector2.zero;

        leaningTimePassed = 0;
    }

    private void Move()
    {
        this.rigidBody.velocity = new Vector2(moveSpeed.x, this.rigidBody.velocity.y);
   
    }

    public float leaningTime = 0;
    public float leaningTimePassed;

    private void Leaning()
    {
        float angle = Mathf.LerpAngle(0, 160, leaningTimePassed / leaningTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);

        leaningTimePassed += Time.deltaTime;
    }
    
    public float timeToReachMaxSpeed;
    public float maxSpeed;
    public float minSpeed;

    public void CalculateHorizontalSpeed(float timePassed)
    {
       // moveSpeed.x = Mathf.Lerp(minSpeed, maxSpeed, timePassed / timeToReachMaxSpeed);
        moveSpeed.x = Mathf.Lerp(minSpeed, maxSpeed, 0);
        
    }
    public void GameOver()
    {
        rigidBody.velocity = Vector2.zero;
        leaningTimePassed = 0;
        IsActive = false;
    }

    public void Restart()
    {
        Disable();
    }
}
