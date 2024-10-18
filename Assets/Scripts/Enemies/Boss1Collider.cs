using UnityEngine;

public class Boss1Collider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning(collision);
        if (collision.gameObject.CompareTag("Platform")) GetComponentInChildren<Boss1>().Shoot();
    }
}
