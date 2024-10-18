using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class Wasp : MonoBehaviour
{
    
    [SerializeField] private float idleMovement; // idle movement amount
    [SerializeField] private float recoilTime;
    private Transform playerTransform;
    [SerializeField] private GameObject projectile;
    private GameObject projectilesObject; //GameObject for all projectiles
    public bool isAlive = true;
    private bool isAwake = false;
    [SerializeField] private GameObject parent;
    private CameraController cam;
    private void Start()
    {
        projectilesObject = GameObject.Find("Projectiles");
        cam = Camera.main.gameObject.GetComponent<CameraController>();
    }
    public void WakeUp(GameObject player) //waking up on player approach
    {
        if (!isAwake)
        {
            isAwake = true;
            playerTransform = player.transform;
            StartCoroutine(Shoot());
        }
    }

    private void FixedUpdate() // idle movement
    {
         Vector2 bias = new Vector2(0f, Mathf.Sin(Time.time) * idleMovement);
         parent.transform.position += (Vector3)bias;
    }

    private IEnumerator Shoot()
    {
        Vector2 dir = playerTransform.position - transform.position;
        if(dir.y < 10f)
        {
            cam.ShakeCamera(.2f, .005f);
            GameObject projectileObj = Instantiate(projectile, transform.position, Quaternion.identity, projectilesObject.transform);
            projectileObj.transform.right = dir;
        }
        yield return new WaitForSeconds(recoilTime);
        if(isAlive)StartCoroutine(Shoot());
    }
}
