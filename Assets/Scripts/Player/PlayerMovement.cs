using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator animator; //player animator
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveForce; //moving force
    [SerializeField] private float maxMoveSpeed; //maximum moving speed
    [SerializeField] private float jumpForce; //jump force
    [SerializeField] private Transform groundCollider; //ground collider
    [SerializeField] private Transform wallCollider; //wall collider
    [SerializeField] private LayerMask wallsLayer; //walls layer
    [SerializeField] private LayerMask wallsAndPlatformsLayer; //walls and platforms layers
    [SerializeField] private Vector2 wallJumpVector; //wall jump vector
    [SerializeField] private AudioSource AS;
    [SerializeField] private AudioClip jumpSound; //jump sound
    [SerializeField] private AudioClip goldPickupSound;
    [SerializeField] private AudioClip gunPickupSound;
    [SerializeField] private GameObject normalJumpPS; //normal jump PartcleSystem
    [SerializeField] private GameObject wallJumpPS; //wall jump PartcleSystem
    [SerializeField] private float hurtPushForce; //push force when hurt
    [SerializeField] private Suit[] suits;
    public Suit suit;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    public bool hasDoubleJumped = false;

    public void MoveLeftDown(){isMovingLeft=true;}
    public void MoveLeftUp(){isMovingLeft=false;}
    public void MoveRightDown(){isMovingRight=true;}
    public void MoveRightUp(){isMovingRight=false;}
    private void Start()
    {
        suit = suits[PlayerPrefs.GetInt("Suit", 0)];
        maxMoveSpeed = suit.movementSpeed;
        GetComponent<SpriteRenderer>().color = suit.color;
    }

    void Update()
    {
        if(Time.timeScale > 0f) //if game is not paused
        {
            //FOR DEBUGGING
            if(Input.GetKeyDown(KeyCode.A)) MoveLeftDown();
            if (Input.GetKeyUp(KeyCode.A)) MoveLeftUp();
            if (Input.GetKeyDown(KeyCode.D)) MoveRightDown();
            if (Input.GetKeyUp(KeyCode.D)) MoveRightUp();
            if (Input.GetKeyDown(KeyCode.Space)) Jump();

            if (isMovingLeft)
            {
                if (rb.velocity.x > 0) rb.velocity = new Vector2(0f, rb.velocity.y);
                rb.AddForce(new Vector2(-moveForce * Time.deltaTime, 0f)); //moving left
            }
            else if (isMovingRight)
            {
                if (rb.velocity.x < 0) rb.velocity = new Vector2(0f, rb.velocity.y);
                rb.AddForce(new Vector2(moveForce * Time.deltaTime, 0f)); //moving right
            }
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxMoveSpeed, maxMoveSpeed), rb.velocity.y); //clamping maximum movement speed

            //rotating player sprite
            if (isMovingRight) transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
            else if (isMovingLeft) transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
            else
            {
                if (rb.velocity.x > 0.1f) transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
                else if(rb.velocity.x < -0.1f) transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
            }

            //updating animator parameters
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        }
    }

    public void Jump() //jumping
    {
        if(Time.timeScale > 0f) //if game is not paused
        {
            if (Physics2D.OverlapCircle(groundCollider.position, 0.1f, wallsAndPlatformsLayer)) //first jump
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(new Vector2(0f, jumpForce));
                hasDoubleJumped = false;
                PlayJumpParticles(false);
                PlaySound(jumpSound);
            }
            else
            {
                if (Physics2D.OverlapCircle(wallCollider.position, 0.2f, wallsLayer)) //wall jump
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    if (transform.rotation.y == 0)
                    {

                        rb.AddForce(new Vector2(- wallJumpVector.x, wallJumpVector.y) * jumpForce);
                    }
                    else
                    {

                        rb.AddForce(new Vector2(wallJumpVector.x, wallJumpVector.y) * jumpForce);
                    }
                    PlaySound(jumpSound);
                    PlayJumpParticles(true);
                }
                else if (!hasDoubleJumped) //double jump
                {
                    hasDoubleJumped = true;
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(new Vector2(0f, jumpForce));
                    PlayJumpParticles(false);
                    PlaySound(jumpSound);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAI")) //waking enemy up on approaching
        {
            switch (collision.gameObject.GetComponent<EnemyHealth>().enemyType)
            {
                case "Fly":
                    collision.GetComponent<Fly>().WakeUp(gameObject);
                    break;
                case "Frog":
                    collision.GetComponent<Frog>().WakeUp(gameObject);
                    break;
                case "Wasp":
                    collision.GetComponent<Wasp>().WakeUp(gameObject);
                    break;
                default:
                    Debug.LogErrorFormat("Cannot find an enemy with this type!");
                    break;
            }
        }
        else if (collision.gameObject.CompareTag("Gold")) //collecting gold
        {
            GetComponent<MoneyController>().collectGold();
            Destroy(collision.gameObject);
            PlaySound(goldPickupSound);
        }

        else if (collision.gameObject.CompareTag("Gun")) //collecting new gun
        {
            Gun newGun = collision.gameObject.GetComponent<GunHandler>().gun;
            GetComponent<PlayerShoot>().currGun = newGun;
            GetComponent<PlayerShoot>().ammoLeft = newGun.ammoAmount;
            GetComponent<PlayerShoot>().UpdateAmmoUI();
            Debug.LogFormat("Picked up " + newGun.gunName);
            Destroy(collision.gameObject);
            PlaySound(gunPickupSound);
        }
    }

    private void PlayJumpParticles(bool isWallJump) //playing jump particles
    {
        Transform particlesObject = GameObject.Find("Particles").transform;
        GameObject particles;
        if(isWallJump) particles = Instantiate(wallJumpPS, groundCollider.position, gameObject.transform.rotation, particlesObject);
        else particles = Instantiate(normalJumpPS, groundCollider.position, gameObject.transform.rotation, particlesObject);
        particles.GetComponent<ParticleSystem>().Play();
    }

    public void HurtPush(Vector2 enemyPos) //pushing player from enemy
    {
        Vector2 pushVector = new Vector2(transform.position.x - enemyPos.x, transform.position.y - enemyPos.y);
        pushVector.Normalize();
        rb.velocity += pushVector * hurtPushForce;
    }

    public void PlaySound(AudioClip sound)
    {
        AS.pitch = Random.Range(.8f, 1.2f);
        AS.PlayOneShot(sound);
    }
}
