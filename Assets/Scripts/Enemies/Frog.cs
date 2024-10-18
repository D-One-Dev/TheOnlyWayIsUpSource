using System.Collections;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [SerializeField] private float jumpForce; //jump force
    [SerializeField] private Vector2 jumpVector; //jump vector
    [SerializeField] private float jumpCooldown; //jump cooldown
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    private bool isAwake; //is enemy awake
    private Transform playerTransform; //player Transform
    private void FixedUpdate()
    {
        animator.SetFloat("YVelocity", Mathf.Abs(rb.velocity.y)); //updating animator parameter
    }
    public void WakeUp(GameObject player) //waking up on player approach
    {
        if (!isAwake)
        { 
            isAwake = true;
            playerTransform = player.transform;
            Jump();
        }
    }

    private void Jump() //jumping towards player
    {
        if (transform.position.x <= playerTransform.position.x) //if player is to the right
        {
            rb.AddForce(new Vector2(jumpVector.x, jumpVector.y) * jumpForce);
        }
        else //if player is to the left
        {
            rb.AddForce(new Vector2(-jumpVector.x, jumpVector.y) * jumpForce);
        }

        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown() //jump cooldown
    {
        yield return new WaitForSeconds(jumpCooldown);
        if(playerTransform != null) Jump();
    }
}
