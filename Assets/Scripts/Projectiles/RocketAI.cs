using System.Collections.Generic;
using UnityEngine;

public class RocketAI : MonoBehaviour
{
    [SerializeField] private float angleChangeSpeed; //angle change speed
    [SerializeField] private float movementSpeed; //projectile speed
    private List<Transform> enemies = new List<Transform>(); //list of nerby enemies
    private Transform target; //target Transform
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (enemies.Count > 0) //if there is an enemy nearby
        {
            target = enemies[0]; //choosing first enemy in list
            Vector2 direction = (Vector2)target.position - rb.position; //calculating direction to enemy
            direction.Normalize(); //normalizing direction
            float rotateAmount = Vector3.Cross(direction, transform.right).z; //calculating rotate amount
            rb.angularVelocity = -rotateAmount * angleChangeSpeed; //rotating projectile
            rb.velocity = transform.right * movementSpeed;
        }
        else //if there are no enemies nearby
        {
            rb.velocity = transform.right * movementSpeed; //moving forward
            rb.angularVelocity = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //adding enemy to list
    {
        if (collision.gameObject.CompareTag("Enemy")) enemies.Add(collision.gameObject.transform);
    }
    private void OnTriggerExit2D(Collider2D collision) //removing enemy from list
    {
        if (collision.gameObject.CompareTag("Enemy")) enemies.Remove(collision.gameObject.transform);
    }
}
