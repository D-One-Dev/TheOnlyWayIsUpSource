using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Image healthbarFill; //healthbar fill image
    [SerializeField] private GameObject deathScreen; //death screen GameObject
    [SerializeField] private int startHealth; //player start health
    [SerializeField] private bool isProtected = false; //is player currently protected
    [SerializeField] private float protectionTime = 1f; //player protection time
    public int curHealth; // current player health
    [SerializeField] private AudioSource AS; //AudioSource
    [SerializeField] private AudioClip healSound; //heal sound
    [SerializeField] private AudioClip damageSound; //damage sound
    [SerializeField] private AudioClip deathSound; //death sound
    [SerializeField] private CameraController cameraController; //CameraController script
    [SerializeField] private GameObject playerHutPS; //player hurt ParticleSystem
    [SerializeField] private Suit[] suits;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Suit suit;
    bool isPlayingHurtAnim;
    private void Start()
    {
        suit = suits[PlayerPrefs.GetInt("Suit", 0)];
        startHealth = suit.health;

        if (PlayerPrefs.GetInt("CurrentHealth", -1) == -1) curHealth = startHealth; //setting the player health
        else curHealth = PlayerPrefs.GetInt("CurrentHealth");

        UpdateHealthbar();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isProtected)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                TakeDamage(collision.gameObject.transform.position); //take damage
            }
            else if (collision.gameObject.CompareTag("Obstacle"))
            {
                TakeDamage(collision.GetContact(0).point); //take damage
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isProtected)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                TakeDamage(collision.gameObject.transform.position); //take damage
            }
            else if (collision.gameObject.CompareTag("Obstacle"))
            {
                TakeDamage(collision.gameObject.transform.position); //take damage
            }
        }
    }

    public void TakeDamage(Vector2 enemyPos) //taking damage
    {
        AS.PlayOneShot(damageSound); //playing damage sound
        PlayJumpParticles();
        cameraController.ShakeCamera(.2f, .02f); //shaking camera
        curHealth--; //decrease health
        UpdateHealthbar(); //update UI

        gameObject.GetComponent<PlayerMovement>().HurtPush(enemyPos);

        if (curHealth <= 0)
        {
            Death(); //death
        }
        else //taking damage
        {
            AS.pitch = Random.Range(.8f, 1.2f);
            StartCoroutine(Protection(protectionTime));
        }
    }

    private IEnumerator Protection(float time)
    {
        if (!isProtected)
        {
            isProtected = true; //enabling protection
            if (!isPlayingHurtAnim)
            {
                isPlayingHurtAnim = true;
                StartCoroutine(ProtectionVisuals());
            }
            yield return new WaitForSeconds(protectionTime);
            isProtected = false; //disabling protection
        }
    }

    private IEnumerator ProtectionVisuals()
    {
        if (isProtected)
        {
            if (_spriteRenderer.enabled == true) _spriteRenderer.enabled = false;
            else _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.125f);
            StartCoroutine(ProtectionVisuals());
        }
        else
        {
            _spriteRenderer.enabled = true;
            isPlayingHurtAnim = false;
        }
    }

    private void UpdateHealthbar() //updating healthbar UI
    {
        healthbarFill.fillAmount = (float)curHealth / startHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision) //collecting medpacks
    {
        if (collision.gameObject.CompareTag("Meds") && curHealth < startHealth)
        {
            Heal(1);
            Destroy(collision.gameObject);
        }
    } 

    private void Heal(int healAmount) //healing
    {
        AS.pitch = Random.Range(.8f, 1.2f);
        AS.PlayOneShot(healSound);
        if ((curHealth + healAmount) < startHealth) curHealth += healAmount;
        else curHealth = startHealth;
        UpdateHealthbar();
    }

    private void Death() //death
    {
        AS.pitch = Random.Range(.8f, 1.2f);
        AS.PlayOneShot(deathSound);

        //GetComponent<MoneyController>().SaveGold();

        gameObject.SetActive(false);
        deathScreen.SetActive(true);

        isPlayingHurtAnim = false;
    }

    private void PlayJumpParticles() //playing jump particles
    {
        Transform particlesObject = GameObject.Find("Particles").transform;
        GameObject particles = Instantiate(playerHutPS, gameObject.transform.position, gameObject.transform.rotation, particlesObject);
        particles.GetComponent<ParticleSystem>().Play();
    }

    public void Revive()
    {

        curHealth = startHealth;
        UpdateHealthbar();
        gameObject.SetActive(true);
        StartCoroutine(Protection(10f));
    }
}
