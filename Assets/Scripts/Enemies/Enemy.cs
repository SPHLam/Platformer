using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rigidBody;
    protected Collider2D[] colliders;
    protected Transform player;
    protected SpriteRenderer spriteRenderer;

    [Header("General Info")]
    [SerializeField] protected float moveSpeed = 2f;
    protected bool canMove = true;
    [SerializeField] protected float idleDuration = 2f;
    protected float idleTimer = 2f;
    private bool _canFlip = true;

    [Header("Basic collision")]
    [SerializeField] protected float groundCheckDistance = 1.1f;
    [SerializeField] protected float wallCheckDistance = 0.7f;
    [SerializeField] protected float playerDetectionDistance = 15f;
    [SerializeField] protected LayerMask playerLayerMask;
    [SerializeField] protected LayerMask groundLayerMask;
    [SerializeField] protected Transform groundCheck;

    [Header("Death detail")]
    [SerializeField] protected float deathImpactSpeed = 5f;
    [SerializeField] protected float deathRotationSpeed = 150f;
    protected bool isDead;

    protected int facingDirection = -1;
    protected bool isFacingRight = false;
    protected bool isTouchingWall;
    protected bool isGrounded;
    protected bool isGroundInfrontDetected;
    protected bool isPlayerDetected;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        if (spriteRenderer.flipX == true && !isFacingRight)
        {
            spriteRenderer.flipX = false;
            Flip();
        }

        UpdatePlayerReference();
        GameManager.OnPlayerRespawn += UpdatePlayerReference;
    }

    private void UpdatePlayerReference()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        idleTimer -= Time.deltaTime;

        HandleCollision();
        HandleAnimation();

        if (isDead)
        {
            HandleDeathRotation();
        }
    }

    #region Die

    public virtual void Die()
    {
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, deathImpactSpeed);
        animator.SetTrigger("Hit");
        isDead = true;

        GameManager.OnPlayerRespawn -= UpdatePlayerReference;
        Destroy(gameObject, 10f);
    }

    private void HandleDeathRotation()
    {
        transform.Rotate(0, 0, deathRotationSpeed * Time.deltaTime);
    }

    #endregion

    #region Animation

    /// <summary>
    /// Handle enemy's animation
    /// </summary>
    protected virtual void HandleAnimation()
    {
        // Change the animation in blender trees
        animator.SetFloat("xVelocity", rigidBody.velocity.x);
    }
    #endregion

    #region Collision

    /// <summary>
    /// Handle collision (detect if Enemy is touching the ground & touching the wall)
    /// </summary>
    protected virtual void HandleCollision()
    {
        // Detect if the enemy is on the ground, if then can only JUMP 
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayerMask);

        // Detect if there is ground infront of the enemy
        isGroundInfrontDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayerMask);

        // Detect if the enemy is touching the wall, if then can SLIDE
        isTouchingWall = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, groundLayerMask);

        // Detect if this enemy can detect the Player
        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, playerDetectionDistance, playerLayerMask);
    }
    #endregion

    #region Flip
    protected virtual void HandleFlip(float playerXPosition)
    {
        if ((transform.position.x > playerXPosition && isFacingRight == true) || (transform.position.x < playerXPosition && isFacingRight == false))
        {
            if (_canFlip)
            {
                _canFlip = false;
                Invoke(nameof(Flip), 0.35f);
            }
        }
    }

    /// <summary>
    /// Handle flip the enemy direction
    /// </summary>
    protected virtual void Flip()
    {
        // Flip by rotating the y axis
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
        facingDirection = facingDirection * -1;
        _canFlip = true;
    }
    #endregion

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(groundCheck.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDirection), transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (playerDetectionDistance * facingDirection), transform.position.y));

    }
}
