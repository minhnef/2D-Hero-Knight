using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemies_Scripts : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private Transform checkPoint;
    [SerializeField]
    private float distance = 5f;
    [SerializeField]
    private LayerMask LayerMask;
    private bool facingLeft = true;


    [SerializeField]
    private Transform playerTransform;
    private bool inRange = false;
    private float attackRange = 6f;
    private float retrieveDistance = 2.5f;
    private float chaseSpeed = 5f;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform attackPoint;
    [SerializeField]
    private float attackRadius = 1f;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float maxHealth = 5f;

  

    // Update is called once per frame
    void Update()
    {
       if(FindFirstObjectByType<GameManger>().isGameAcive == false)
        {
            return;
        }

        if (maxHealth <= 0f)
        {
            animator.SetBool("Died", true);
            Died();
        }
        
        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }

        if (inRange == true)
        {
            //xu ly khi player trong tam
            if (playerTransform.position.x > transform.position.x && facingLeft == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (playerTransform.position.x < transform.position.x && facingLeft == false)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }

            if (Vector2.Distance(transform.position, playerTransform.position) > retrieveDistance)
            {
                animator.SetBool("Attack1", false);
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack1", true);
            }

        }
        else
        {
            //xu ly khi player ngoai tam

            transform.Translate(Vector2.left * Time.deltaTime * speed);
            RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, LayerMask);

            if (hit == false && facingLeft == true)
            {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else if (hit == false && facingLeft == false)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }
        }
    }

    private void Attack()
    {
        Collider2D colliInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, layerMask);

        if(colliInfo)
        {
            if (colliInfo.gameObject.GetComponent<Player_Scripts>() != null)
            {
                
                colliInfo.gameObject.GetComponent<Player_Scripts>().takeDamage(1);
                
            }
        }
        
    }
    private void Died()
    {
        Debug.Log(this.transform.name + " died");
        Destroy(this.gameObject);
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
    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null)
        {
            Debug.Log("checkPoint is null");
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (attackPoint == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

}
