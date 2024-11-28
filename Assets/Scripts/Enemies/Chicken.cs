using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Enemy
{
	[SerializeField] private float _aggroDuration;
	private float _aggroTimer;

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

		// Cannot detect player -> Can't move
		if (_aggroTimer < 0)
			canMove = false;

		if (isGrounded)
			HandleTurnAround();

		HandleMovement();
	}

	private void HandleTurnAround()
	{
		if (isGroundInfrontDetected == false || isTouchingWall == true)
		{
			Flip();
			canMove = false;
			rigidBody.velocity = Vector2.zero;
		}
	}

	private void HandleMovement()
	{
		if (!canMove)
			return;

		HandleFlip(player.position.x);

		if (isGrounded)
			rigidBody.velocity = new Vector2(moveSpeed * facingDirection, rigidBody.velocity.y);
	}
}
