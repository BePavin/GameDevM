using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 7f;

    private Rigidbody2D rb;
    private Vector2 direction = Vector2.zero;

    public float jumpForce = 12f;
    public int health;
    public Transform foot;
    public float collisionRadius = 0.25f;
    public LayerMask layerGround;

    private bool invunerable = false;
    private SpriteRenderer sprite;
    private Animator anim;
    private bool onFloor = false;
    private bool jump = false;

    public float attackRate;
    public GameObject head;

    private float nextAttack;
    private CameraFollow cameraScript;

    public AudioClip fxHurt;
    public AudioClip fxJump;
    public AudioClip fxAttack;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<CameraFollow>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && onFloor)
        {
            jump = true;
        }

        GroundCheck();
        Animations();

        if (Input.GetButton("Fire1") && onFloor && Time.time > nextAttack)
        {
            SoundManager.instance.PlaySound(fxAttack);
            Attack();        
        }
    }

    private void FixedUpdate()
    {
        Move(Input.GetAxisRaw("Horizontal"));

        if (jump)
        {
            jump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            SoundManager.instance.PlaySound(fxJump);
        }
    }

    void Move(float move)
    {
        if (move > 0)
        {
            direction = Vector2.right;
        }

        if (move < 0)
        {
            direction = Vector2.left;
        }

        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        if (move == 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        transform.right = direction;
    }

    void GroundCheck()
    {
        onFloor = Physics2D.OverlapCircle(foot.position, collisionRadius, layerGround);
    }

    //Para controlar o raio da colisao do foot
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)foot.position, collisionRadius);
    }
    */
    

    void Animations()
    {
        anim.SetFloat("VelY", rb.velocity.y);
        anim.SetBool("JumpFall", !onFloor);
        anim.SetBool("Walk", rb.velocity.x != 0f && onFloor);
    }
    

    void Attack()
    {
        anim.SetTrigger("Shoot");
        nextAttack = Time.time + attackRate;
    }

    IEnumerator DamageEffect()
    {
        cameraScript.ShakeCamera(0.8f, 0.08f);

        for (float i = 0f; i < 1; i += 0.1f)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        invunerable = false;
    }

    public void DamagePlayer()
    {
        if (!invunerable)
        {
            invunerable = true;
            health--;
            StartCoroutine(DamageEffect());

            SoundManager.instance.PlaySound(fxHurt);

            if (health <= 0)
            {
                PlayerDeath();                            
                //Destroy(gameObject);

                Invoke("ReloadLevel", 0.7f);
                gameObject.SetActive(false);
            }
        }
    }

    public void DamageWater()
    {
        health = 0;
        DamagePlayer();
        Invoke("ReloadLevel", 0.7f);
        gameObject.SetActive(false);
    }

    public void PlayerDeath()
    {
        GameObject cloneHead = Instantiate(head, transform.position, Quaternion.identity);
        Rigidbody2D rbHead = cloneHead.GetComponent<Rigidbody2D>();

        rbHead.AddForce(Vector3.up * 10000);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}


