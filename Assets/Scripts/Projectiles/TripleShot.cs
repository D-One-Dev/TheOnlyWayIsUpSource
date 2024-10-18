using UnityEngine;

public class TripleShot : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    private void Start()
    {
        float angle = 30f;
        Transform projectilesObject = GameObject.Find("Projectiles").transform;
        for (int i = 0; i < 3; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, projectilesObject);
            if (transform.eulerAngles.y == 0f)
                projectile.transform.eulerAngles = new Vector3(0f, 0f, angle);
            else
                projectile.transform.eulerAngles = new Vector3(0f, -180f, angle);
            angle -= 30f;
        }
        Destroy(gameObject);
    }
}
