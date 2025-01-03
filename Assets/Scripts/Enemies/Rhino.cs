using Cinemachine;
using System.Collections;
using UnityEngine;

public class Rhino : Enemy
{
    [Header("Rhino detail")]
    [SerializeField] private float _aggroDuration;
    [SerializeField] private Vector2 _impactPower; // Bounce back to wall
    [SerializeField] private float _maxSpeed;
    private float _defaultSpeed;
    [SerializeField] private float _speedUpRate = 0.6f;
    private float _aggroTimer;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _dustVFX;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private Vector2 _impulseDirection;

    protected override void Start()
    {
        base.Start();
        canMove = false;
        _defaultSpeed = moveSpeed;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    protected override void Update()
    {
        base.Update();

        _aggroTimer -= Time.deltaTime;

        if (isDead)
            return;

        if (isPlayerDetected)
        {
            canMove = true;
            _aggroTimer = _aggroDuration;
        }

        HandleCharge();
    }

    private void HandleCharge()
    {
        if (!canMove)
            return;

        HandleSpeedUp();

        rigidBody.velocity = new Vector2(moveSpeed * facingDirection, rigidBody.velocity.y);

        if (isTouchingWall == true)
            WallHit();

        HandleTurnAround();
    }

    private void HandleSpeedUp()
    {
        moveSpeed += (Time.deltaTime * _speedUpRate);

        if (moveSpeed >= _maxSpeed)
            _maxSpeed = moveSpeed;
    }

    private void WallHit()
    {
        animator.SetBool("hitWall", true);
        rigidBody.velocity = new Vector2(_impactPower.x * -facingDirection, _impactPower.y);
        SpeedReset();
        canMove = false;
        HitWallImpact();
    }

    private void SpeedReset()
    {
        moveSpeed = _defaultSpeed;
    }

    private void ChargeOver()
    {
        animator.SetBool("hitWall", false);
        StartCoroutine(DelayTurnAround());
    }

    private IEnumerator DelayTurnAround()
    {
        yield return new WaitForSeconds(0.5f);
        Flip();
    }

    private void HandleTurnAround()
    {
        if (isGroundInfrontDetected == false)
        {
            Flip();
            canMove = false;
            rigidBody.velocity = Vector2.zero;
            SpeedReset();
        }
    }

    private void HandleMovement()
    {
        if (!canMove)
            return;

        if (isGrounded)
            rigidBody.velocity = new Vector2(moveSpeed * facingDirection, rigidBody.velocity.y);
    }

    private void HitWallImpact()
    {
        _dustVFX.Play();
        _impulseSource.m_DefaultVelocity = new Vector2(_impulseDirection.x * facingDirection, _impulseDirection.y);
        _impulseSource.GenerateImpulse();
    }
}
