using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimatorController))]
public class PlayerController : MonoBehaviour 
{

    private PlayerAnimatorController animatorController;
    private PlayerMovement movementController;

    public event Action OnGameOver;
    public event Action OnRun;
    public event Action OnJump;

    void Awake()
    {
        animatorController = this.GetComponent<PlayerAnimatorController>();
        movementController = this.GetComponent<PlayerMovement>();

        if (animatorController == null) throw new MissingReferenceException("Missing a reference to the PlayerAnimatorControler on the " + this.gameObject);
        if (movementController == null) throw new MissingReferenceException("Missing a reference to the PlayerMovement on the " + this.gameObject);


        animatorController.OnMenuAnimationsExit += movementController.Enable;


        startPosition = this.transform.position;
        startRotation = this.transform.rotation;

    }

    void Update()
    {

        if (IsActive)
        {
            HandleInput();
            CalculateScore();
            CheckIfTargetInsiderView(this.transform.position);
        }

    }

    private Vector3 startPosition;
    private Quaternion startRotation;
    public int score;
    private void CalculateScore()
    {
        score = (int) (transform.position.x - startPosition.x) / 5; 
    }

    public void CalculateSpeed(float time)
    {
        movementController.CalculateHorizontalSpeed(time);
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Jump();
            if (OnJump != null)
            {
                OnJump();
            }
        }
    }

    public void Run()
    {
        if (OnRun != null)
        { 
            OnRun();
        }
        animatorController.Run();
        IsActive = true;
    }

    public void Restart()
    {
        this.transform.rotation = startRotation;
        this.transform.position = startPosition;

        animatorController.Restart();
        movementController.Restart();
        Run();
    }

    public void JumpOnBuilding()
    {
        animatorController.JumpOnBuilding();
    }

    public void JumpFromBuilding()
    {
       this.animatorController.JumpFromBuilding();
    }

    public void Jump()
    {
        this.animatorController.Jump();
        this.movementController.Jump();
    }

    public bool IsActive = false;

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }

    public void Enable()
    {
        this.gameObject.SetActive(true);
    }

    private void GameOver(Vector3 location)
    {

        FailLocation = location;

        if (IsActive)
        {
            if (OnGameOver != null)
            {
                OnGameOver();
            }
            movementController.GameOver();
            IsActive = false;
        }
    }

    //Engine 
    public Vector3 FailLocation { get; set; }

    private float tolerance = 15;
    void CheckIfTargetInsiderView(Vector3 location)
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y > tolerance + Screen.height || screenPosition.y - tolerance < 0)
        {
            GameOver(location);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        GameOver(coll.contacts[0].point);
    }


}
