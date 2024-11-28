using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
	[Header("Snail Info")]
	[SerializeField] private SnailBody _snailBody;
	[SerializeField] private float _maxSpeed = 10f;
	private bool _hasBody = true;
	protected override void Update()
	{
		base.Update();

		if (isDead)
			return;

		HandleMovement();
		HandleCollision();

		if (isGrounded)
			HandleTurnAround();
	}

	private void HandleTurnAround()
	{
		bool canFlipFromLedge = !isGroundInfrontDetected && _hasBody;

		if (canFlipFromLedge || isTouchingWall)
		{
			Flip();
			idleTimer = idleDuration;
			rigidBody.velocity = Vector2.zero;
		}
	}

	public override void Die()
	{
		if (_hasBody)
		{
			canMove = false;
			_hasBody = false;
			idleDuration = 0;
			rigidBody.velocity = Vector2.zero;
			animator.SetTrigger("Hit");
		}
		else if (canMove == false && _hasBody == false)
		{
			animator.SetTrigger("Hit");
			canMove = true;
			moveSpeed = _maxSpeed;
		}
		else
		{
			base.Die();
		}
	}

	protected override void Flip()
	{
		base.Flip();

        if (!_hasBody)
        {
			animator.SetTrigger("WallHit");
        }
    }

	private void HandleMovement()
	{
		if (idleTimer > 0 || !canMove)
			return;

		if (isGrounded)
			rigidBody.velocity = new Vector2(moveSpeed * facingDirection, rigidBody.velocity.y);
	}

	private void CreateBody()
	{
		SnailBody newBody = Instantiate(_snailBody, transform.position, Quaternion.identity);
		
		newBody.SetupBody(deathImpactSpeed, facingDirection * deathRotationSpeed, facingDirection);

		Destroy(newBody, 10f);
	}
}
