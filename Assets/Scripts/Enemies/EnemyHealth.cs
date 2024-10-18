using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 10; //enemy health
    [SerializeField] private GameObject deathPS; //death ParticleSystem
    [SerializeField] private GameObject parent;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private SpriteRenderer sr;
    private GameObject player;
    private PlayerMovement _playerMovement;
    public string enemyType; //enemy type (fly,frog,etc.)
    private bool isDamaged;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = player.GetComponent<PlayerMovement>();
    }
    public void TakeDamage(int dmg) //taking damage
    {
        StartCoroutine(DamageIndicator());
        health--; //decrease health
        if (health == 0) //enemy death
        {
            if (enemyType == "Wasp") GetComponentInChildren<Wasp>().isAlive = false;
            PlayDeathParticles();
            _playerMovement.PlaySound(deathSound);
            GetComponent<ItemDrop>().DropItem(); //drop loot
            Destroy(parent);
        }
        else _playerMovement.PlaySound(hurtSound);
    }

    private void PlayDeathParticles() //playing death particles
    {
        Transform particlesObject = GameObject.Find("Particles").transform;
        GameObject particles = Instantiate(deathPS, gameObject.transform.position, gameObject.transform.rotation, particlesObject);
        particles.GetComponent<ParticleSystem>().Play();
    }

    private IEnumerator DamageIndicator()
    {
        if (!isDamaged)
        {
            isDamaged = true;
            Color defaultColor = sr.color;
            int r = Mathf.RoundToInt(Mathf.Clamp(defaultColor.r + 200, 0, 255));
            int g = Mathf.RoundToInt(Mathf.Clamp(defaultColor.g + 200, 0, 255));
            int b = Mathf.RoundToInt(Mathf.Clamp(defaultColor.b + 200, 0, 255));
            sr.color = new Color(r, g, b, 255);
            yield return new WaitForSeconds(.01f);
            sr.color = defaultColor;
            isDamaged = false;
        }
    }
}
