using UnityEngine;
using UnityEngine.UI;

public class Player_Scripts : MonoBehaviour
{
    private float movement;
    [SerializeField] private float moveSpeed = 5f;
    private bool isRight = true;
    [SerializeField] private float jumpHeight;

    public Rigidbody2D rb;
    [SerializeField]
    private bool isGrounded = true;

    [SerializeField]
    private Animator playerAni;

    private int maxHealth = 3;

    public Text lifePoint;

    public Transform attackPoint;
    public float attackRadius = 1.22f;
    public LayerMask layerMask;

    void Update()
    {
        
        if (maxHealth <= 0)
        {
            die(); return;
        }
        lifePoint.text = maxHealth.ToString();
        movement = Input.GetAxis("Horizontal");

        if (movement < 0f && isRight)
        {
            isRight = false;
            transform.eulerAngles = new Vector3(0f, -180f, 0f);

        }
        else if (movement > 0f && !isRight)
        {
            isRight = true;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            jump();
            isGrounded = false;
            playerAni.SetBool("Jump", true);

        }
        if (Mathf.Abs(movement) > 0.1f)
        {
            playerAni.SetFloat("Run", 1f);
        }
        else if (movement == 0f)
        {
            playerAni.SetFloat("Run", 0f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            playerAni.SetTrigger("Attack");
        }

    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0, 0) * Time.fixedDeltaTime * moveSpeed;
    }

    private void Attack()
    {
        Collider2D colliderInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, layerMask);
        if (colliderInfo)
        {
            if (colliderInfo.gameObject.GetComponent<Enemies_Scripts>() != null)
            {
                colliderInfo.gameObject.GetComponent<Enemies_Scripts>().takeDamage(1);

            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
        return;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            playerAni.SetBool("Jump", false);
        }
    }
    void die()
    {

        Debug.Log("Player died.");
        playerAni.SetBool("Died", true);
        FindFirstObjectByType<GameManger>().isGameAcive = false;
        Destroy(this.gameObject,1.2f);

    }
    public bool takeDamage(int damage)
    {
        if (maxHealth == 0) { return true; }
        else
        {
            maxHealth -= damage;
            return false;
        }


    }
    private void jump()
    {
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);

    }
}
