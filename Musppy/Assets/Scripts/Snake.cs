using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public int health;
    public float speed;
    public Transform wallCheck;

    private bool facingRight = true;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private bool touchedWall = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        TouchedWall();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack"))
        {
            DamageEnemy();
        }
        
    }
    */

    void TouchedWall()
    {
        touchedWall = Physics2D.Linecast(transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (touchedWall)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        speed *= -1;
    }

    IEnumerator DamageEffect()
    {
        float actualSpeed = speed;
        speed *= -1;
        sprite.color = Color.red;
        rb.AddForce(new Vector2(0f, 200f));
        yield return new WaitForSeconds(0.1f);
        speed = actualSpeed;
        sprite.color = Color.white;
    }

    /*
    public void DamageEnemy(int damage)
    {
        health--;
        StartCoroutine(DamageEffect());

        if(health < 1)
        {
            Destroy(gameObject);
        }
    }
    */

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(DamageEffect());

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
