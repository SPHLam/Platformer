using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody2D;
	private void Awake()
	{
		_rigidBody2D = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetVelocity(Vector2 velocity)
	{
		_rigidBody2D.velocity = velocity;
	}

	public void FlipSprite()
	{
		_spriteRenderer.flipX = !_spriteRenderer.flipX;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			collision.GetComponent<Player>().Knockback(transform.position.x);
			Destroy(gameObject);
		}

		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			Destroy(gameObject, 0.05f);
		}
	}
}
