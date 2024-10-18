using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float lifeTime; //lifetime
    [SerializeField] private GameObject hitPS; //hit ParticleSystem
    [SerializeField] private GameObject parent;
    private void Start()
    {
        
        StartCoroutine(Lifetime(lifeTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) //hitting enemy
        {
            collision.gameObject.GetComponentInChildren<EnemyHealth>().TakeDamage(1);
            PlayHitParticles();
            Destroy(parent.gameObject);
        }
        else if (collision.gameObject.CompareTag("LaserTrigger")) //hitting laser wall
        {
            Destroy(collision.gameObject);
            PlayHitParticles();
            Destroy(parent.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayHitParticles();
        Destroy(parent.gameObject);
    }

    private IEnumerator Lifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(parent.gameObject);
    }

    private void PlayHitParticles() //playing hit particles
    {
        Transform particlesObject = GameObject.Find("Particles").transform;
        GameObject particles = Instantiate(hitPS, parent.transform.position, parent.transform.rotation, particlesObject);
        particles.GetComponent<ParticleSystem>().Play();
    }
}
