using System.Collections;
using UnityEngine;

public class DefaultBullet : MonoBehaviour
{
    [SerializeField] private float projectileSpeed; //projectile speed
    [SerializeField] private float lifeTime; //projectile lifetime
    [SerializeField] private GameObject hitPS; //hit ParticleSystem
    void Start()
    {
        StartCoroutine(Lifetime(lifeTime));
        GetComponent<Rigidbody2D>().velocity = transform.right * projectileSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) //hitting enemy
        {
            collision.gameObject.GetComponentInChildren<EnemyHealth>().TakeDamage(1);
            PlayHitParticles();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("LaserTrigger")) //hitting laser wall trigger
        {
            Destroy(collision.gameObject);
            PlayHitParticles();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayHitParticles();
        Destroy(gameObject);
    }

    private IEnumerator Lifetime(float lifetime) //destroying projectile if time ended
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void PlayHitParticles() //playing hit particles
    {
        Transform particlesObject = GameObject.Find("Particles").transform;
        GameObject particles = Instantiate(hitPS, gameObject.transform.position, gameObject.transform.rotation, particlesObject);
        particles.GetComponent<ParticleSystem>().Play();
    }
}
