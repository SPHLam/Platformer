using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : Enemy
{
	[Header("Trunk details")]
	[SerializeField] private Bullet _bulletPrefab;
	[SerializeField] private Transform _gunPoint;
	[SerializeField] private float _bulletSpeed = 7f;
	[SerializeField] private float _attackCooldown = 1.5f;
	private bool _canAttack = true;
	private float lastTimeAttacked;
	protected override void Update()
	{
		base.Update();

		if (isDead)
			return;

		_canAttack = Time.time > lastTimeAttacked + _attackCooldown;

		if (isPlayerDetected && _canAttack)
			Attack();

		HandleMovement();
		HandleCollision();

		if (isGrounded)
			HandleTurnAround();
	}

	private void HandleTurnAround()
	{
		if (!isGroundInfrontDetected || isTouchingWall)
		{
			Flip();
			idleTimer = idleDuration;
			rigidBody.velocity = Vector2.zero;
		}
	}

	private void HandleMovement()
	{
		if (idleTimer > 0)
			return;

		if (isGrounded)
			rigidBody.velocity = new Vector2(moveSpeed * facingDirection, rigidBody.velocity.y);
	}

	private void Attack()
	{
		idleTimer = idleDuration + _attackCooldown; // Stand still
		lastTimeAttacked = Time.time;
		animator.SetTrigger("Attack");
	}

	private void CreateBullet()
	{
		Bullet newBullet = Instantiate(_bulletPrefab, _gunPoint.position, Quaternion.identity);

		if (facingDirection == 1)
			newBullet.FlipSprite();

		Vector2 bulletVelocity = new Vector2(_bulletSpeed * facingDirection, 0);
		newBullet.SetVelocity(bulletVelocity);
		Destroy(newBullet, 10f);
	}
}
