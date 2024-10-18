using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject deathPS;
    public void WakeUp()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void PlayDeathParticles() //playing death particles
    {
        Transform particlesObject = GameObject.Find("Particles").transform;
        GameObject particles = Instantiate(deathPS, gameObject.transform.position, gameObject.transform.rotation, particlesObject);
        particles.GetComponent<ParticleSystem>().Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(transform.position);
        PlayDeathParticles();
        Destroy(parent);
    }
}
