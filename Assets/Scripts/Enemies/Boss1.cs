using System.Collections;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpRecoilTime;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject projectile;
    private CameraController cam;
    private GameObject projectilesObject;
    private bool isAwake;
    void Start()
    {
        projectilesObject = GameObject.Find("Projectiles");
        cam = Camera.main.gameObject.GetComponent<CameraController>();
    }
    void Update()
    {
        animator.SetFloat("YSpeed", rb.velocity.y);
    }

    public void WakeUp()
    {
        if (!isAwake)
        {
            isAwake = true;
            StartCoroutine(Jump());
        }
    }

    private IEnumerator Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce));
        yield return new WaitForSeconds(jumpRecoilTime);
        StartCoroutine(Jump());
    }

    public void Shoot()
    {
        if(isAwake)
        {
            GameObject leftBullet = Instantiate(projectile, transform.position, Quaternion.identity, projectilesObject.transform);
            leftBullet.transform.right = Vector2.left;
            Instantiate(projectile, transform.position, Quaternion.identity, projectilesObject.transform);
            cam.ShakeCamera(.2f, .005f);
        }
    }
}
