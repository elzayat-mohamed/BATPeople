using UnityEngine;
using System.Collections;


public class ObstacleViewItem : MonoBehaviour 
{
    public Obstacle Obstacle { get; set; }
    private Rigidbody2D rigidBody;
    private float moveSpeed;

    void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
    }

    public void Show(Vector3 position, float moveSpeed, Obstacle obstacle)
    {
        this.gameObject.SetActive(true);
        this.transform.position = position;
        this.moveSpeed = moveSpeed;
        
        this.Obstacle = obstacle;

        if (obstacle.IsMoving)
        {
            this.rigidBody.velocity = Vector2.zero;
        }
    } 

    public void Hide()
    {
        //this.rigidBody.velocity = new Vector2(0, 0);
        this.gameObject.SetActive(false);
    }

    public void Activate()
    {
        if (Obstacle.IsMoving)
        {
            this.rigidBody.velocity = new Vector2(-moveSpeed, 0);               
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (Obstacle.IsMoving)
        {
            rigidBody.gravityScale = 1f;
        }
    }
}
