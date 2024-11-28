using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private DifficultyType _gameDifficulty;
    private GameManager _gameManager;

    private CapsuleCollider2D _capsuleCollider2D;
    private bool _canBeControlled = false;

    [Header("Movement Info")]
    [SerializeField] private float _speed = 7.5f;
    [SerializeField] private float _jumpForce = 2.5f;
    [SerializeField] private float _doubleJumpForce;
    private bool _canDoubleJump;
    private float _defaultGravityScale;

    [Header("Collision Info")]
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private LayerMask _groundLayerMask;
    [Space]
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private Transform _enemyCheck;
    [SerializeField] private float _enemyCheckRadius;

    private bool _isTouchingWall;
    private bool _isGrounded;
    private bool _isFacingRight = true;
    private bool _isAirborne;
    private float _facingDirection = 1f;
    private float _xInput;
    private float _yInput;

    [Header("Wall Info")]
    [SerializeField] private float _wallJumpDuration = 0.6f;
    [SerializeField] private Vector2 _wallJumpForce;
    private bool _isWallJumping;

    [Header("Knockback")]
    [SerializeField] private float _knockBackDuration = 1f;
    [SerializeField] private Vector2 _knockBackPower;
    private bool _isKnocked;

    [Header("Buffer & Coyote Jump")]
    [SerializeField] private float _bufferJumpWindow = 0.25f;
    private float _bufferJumpActivated = -1f;
    [SerializeField] private float _coyoteJumpWindow = 0.5f;
    private float _coyoteJumpActivated = -1f;

    [Header("Player Visuals")]
    [SerializeField] private GameObject _deathVFX;
    [SerializeField] private AnimatorOverrideController[] _animators;
    [SerializeField] private int _skinId = 0;

    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _defaultGravityScale = _rigidBody.gravityScale;

        UpdateGameDifficulty();
        RespawnFinished(false);
        UpdateSkin();
    }


    // Update is called once per frame
    void Update()
    {
        if (!_canBeControlled)
        {
            HandleAnimation();
            HandleCollision();
            return;
        }
            

        if (_isKnocked)
            return;

        UpdateAirborneStatus();

        HandleEnemyDetection();
        HandleWallSlide();
        HandleMovement();
        HandleFlip();
        HandleCollision();
        HandleAnimation();

    }

    #region Skin

    public void UpdateSkin() 
    {
        SkinManager skinManager = SkinManager.Instance;

        if (skinManager == null)
            return;
        _animator.runtimeAnimatorController = _animators[skinManager.chosenSkinId];
    }

    #endregion

    #region Difficulty
    private void UpdateGameDifficulty()
    {
        if (DifficultyManager.Instance != null)
            _gameDifficulty = DifficultyManager.Instance.difficulty;
    }

    #endregion

    #region Enemy

    private void HandleEnemyDetection()
    {
        if (_rigidBody.velocity.y >= 0)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemyCheck.position, _enemyCheckRadius, _enemyLayerMask);

        foreach (Collider2D enemy in colliders)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();
            
            if (newEnemy != null)
            {
                newEnemy.Die();
                Jump();
            }

        }
    }

    #endregion

    #region Slide

    /// <summary>
    /// Handle wall sliding
    /// </summary>
    private void HandleWallSlide()
    {
        bool canWallSlide = _isTouchingWall && _rigidBody.velocity.y < 0;

        if (!canWallSlide)
            return;

        float yModifier = _yInput < 0 ? 1 : 0.05f;

        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _rigidBody.velocity.y * yModifier);
    }

    #endregion

    #region Airborne

    /// <summary>
    /// Updating Player double jump status (in air or not)
    /// </summary>
    private void UpdateAirborneStatus()
    {
        if (_isGrounded && _isAirborne)
            HandleLanding();

        if (!_isGrounded && !_isAirborne)
            BecomeAirborne();
    }

    /// <summary>
    /// Enable airborne; which allowed to double jump
    /// </summary>
    private void BecomeAirborne()
    {
        _isAirborne = true;

        // Leaving off the edge
        if (_rigidBody.velocity.x < 0)
            ActivateCoyoteJump();
    }

    #endregion

    #region Land

    /// <summary>
    /// Handle landing; which disable double jump by disable airborne
    /// </summary>
    private void HandleLanding()
    {
        _isAirborne = false;
        _canDoubleJump = true;

        AttemptBufferJump();
    }

    #endregion

    #region Move

    /// <summary>
    /// Handle movement of the player (horizontal & vertical)
    /// </summary>
    private void HandleMovement()
    {
        // Get the horizontal & vertical input
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");

        // Touching the wall / wall jumping -> No movement
        if (_isTouchingWall || _isWallJumping)
            return;

        // Apply it to the velocity -> Moving
        _rigidBody.velocity = new Vector2(_xInput * _speed, _rigidBody.velocity.y);
    }

    #endregion

    #region Animation

    /// <summary>
    /// Handle player's animation
    /// </summary>
    private void HandleAnimation()
    {
        // Change the animation in blender trees
        _animator.SetFloat("xVelocity", _rigidBody.velocity.x);
        _animator.SetFloat("yVelocity", _rigidBody.velocity.y);

        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetBool("isTouchingWall", _isTouchingWall);
    }
    #endregion

    #region Collision

    /// <summary>
    /// Handle collision (detect if Player is touching the ground & touching the wall)
    /// </summary>
    private void HandleCollision()
    {
        // Detect if the player is on the ground, if then can only JUMP 
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, _groundLayerMask);

        // Detect if the player is touching the wall, if then can SLIDE
        _isTouchingWall = Physics2D.Raycast(transform.position, Vector2.right * _facingDirection, _wallCheckDistance, _groundLayerMask);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJump();
            RequestBufferJump();
        }
    }
    #endregion

    #region Flip
    private void HandleFlip()
    {
        if ((_xInput < 0 && _isFacingRight) || (_xInput > 0 && !_isFacingRight))
            Flip();
    }

    /// <summary>
    /// Handle flip the player direction
    /// </summary>
    private void Flip()
    {
        // Flip by rotating the y axis
        transform.Rotate(0, 180, 0);
        _isFacingRight = !_isFacingRight;
        _facingDirection = _facingDirection * -1;
    }
    #endregion

    #region Jumps

    #region Handle Jumping functionality
    /// <summary>
    /// Handle different Jump 
    /// </summary>
    private void HandleJump()
    {
        bool coyoteJumpAvailable = Time.time < _coyoteJumpActivated + _coyoteJumpWindow;

        if (_isGrounded || coyoteJumpAvailable)
            Jump();
        else if (_isTouchingWall && !_isGrounded)
            WallJump();
        else if (_canDoubleJump)
            DoubleJump();

        CancelCoyoteJump();
    }
    #endregion

    #region Normal Jump
    /// <summary>
    /// Jump
    /// </summary>
    private void Jump() 
    {
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce); 
    }

    #endregion

    #region Double Jump
    /// <summary>
    /// Double Jump
    /// </summary>
    private void DoubleJump() 
    {
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _doubleJumpForce);
        _canDoubleJump = false;
        _isWallJumping = false;
    }

    #endregion

    #region Wall Jump

    private IEnumerator WallJumpRoutine()
    {
        _isWallJumping = true;
        yield return new WaitForSeconds(_wallJumpDuration);
        _isWallJumping = false;
    }

    /// <summary>
    /// Wall jump
    /// </summary>
    private void WallJump()
    {
        _canDoubleJump = true;
        _rigidBody.velocity = new Vector2(_wallJumpForce.x * -_facingDirection, _wallJumpForce.y);
        Flip();
        StopAllCoroutines();
        StartCoroutine(WallJumpRoutine());
    }

    #endregion

    #region Buffer Jump

    /// <summary>
    /// Buffer Jump, for smoother timing experience
    /// </summary>
    private void AttemptBufferJump()
    {
        if (Time.time < _bufferJumpActivated + _bufferJumpWindow)
        {
            _bufferJumpWindow = 0;
            Jump();
        }
    }

    /// <summary>
    /// Getting the time when the jump button is pressed before landing
    /// </summary>
    private void RequestBufferJump()
    {
        if (_isAirborne)
            _bufferJumpActivated = Time.time;
    }

    #endregion

    #region Coyote Jump

    /// <summary>
    /// Getting the time when the jump button is pressed before leaving the edge
    /// </summary>
    private void ActivateCoyoteJump()
    {
        _coyoteJumpActivated = Time.time;
    }
    /// <summary>
    /// Reset the coyote time 
    /// </summary>
    private void CancelCoyoteJump()
    {
        _coyoteJumpActivated = 0;
    }

    #endregion

    #endregion

    #region Knockback

    private IEnumerator KnockbackRoutine()
    {
        _isKnocked = true;
        _animator.SetBool("isKnocked", true);
        yield return new WaitForSeconds(_knockBackDuration);
        _isKnocked = false;
        _animator.SetBool("isKnocked", false);
    }

    public void Knockback(float sourceDamageDirection)
    {
        //_facingDirection = 1f;

        if (transform.position.x > sourceDamageDirection)
            _facingDirection = -1f;
        if (_isKnocked)
            return;

        StartCoroutine(KnockbackRoutine());
        _rigidBody.velocity = new Vector2(_knockBackPower.x * -_facingDirection, _knockBackPower.y);
    }

    #endregion

    #region Damage
    public void Damage()
    {
        if (_gameDifficulty == DifficultyType.Normal)
        {
            if (_gameManager.GetFruitsCollected() <= 0)
            {
                // Restart the level
                Die();
                _gameManager.RestartLevel();
            }
            else
                _gameManager.RemoveFruit();

            return;
        }
        else if (_gameDifficulty == DifficultyType.Hard)
        {
            // Restart the level
            Die();
            _gameManager.RestartLevel();
        }
    }
    #endregion

    #region Die

    public void Die()
    {
        GameObject newDeathVFX = Instantiate(_deathVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    #endregion

    #region Respawn

    public void RespawnFinished(bool finished)
    {
        if (finished)
        {
            _rigidBody.gravityScale = _defaultGravityScale;
            _canBeControlled = true;
            _capsuleCollider2D.enabled = true;
        }
        else
        {
            _rigidBody.gravityScale = 0;
            _canBeControlled = false;
            _capsuleCollider2D.enabled = false;
        }
    }

    #endregion

    #region Trampoline

    private IEnumerator PushCoroutine(Vector2 direction, float duration)
    {
        _canBeControlled = false;
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.AddForce(direction, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);
        _canBeControlled = true;
    }

    public void Push(Vector2 direction, float duration)
    {
        StartCoroutine(PushCoroutine(direction, duration));
    }

    #endregion

    /// <summary>
    /// Draw gizmos
    /// </summary>
    private void OnDrawGizmos()
    { 
        Gizmos.DrawWireSphere(_enemyCheck.position, _enemyCheckRadius);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - _groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (_wallCheckDistance * _facingDirection), transform.position.y));
    }
}
