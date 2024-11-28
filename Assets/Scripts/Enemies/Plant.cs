using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{
	[Header("Plant details")]
	[SerializeField] private Bullet _bulletPrefab;
	[SerializeField] private Transform _gunPoint;
	[SerializeField] private float _bulletSpeed = 7f;
	[SerializeField] private float _attackCooldown = 1.5f;
	private bool _canAttack = true;
	private float lastTimeAttacked;
	protected override void Update()
	{
		base.Update();

		_canAttack = Time.time > lastTimeAttacked + _attackCooldown;

		if (isPlayerDetected && _canAttack)
			Attack();
	}

	private void Attack()
	{
		lastTimeAttacked = Time.time;
		animator.SetTrigger("Attack");
	}

	private void CreateBullet()
	{
		Bullet newBullet = Instantiate(_bulletPrefab, _gunPoint.position, Quaternion.identity);

		Vector2 bulletVelocity = new Vector2(_bulletSpeed * facingDirection, 0);
		newBullet.SetVelocity(bulletVelocity);
		Destroy(newBullet, 10f);
	}

	protected override void HandleAnimation()
	{
		// Keep empty
	}
}
