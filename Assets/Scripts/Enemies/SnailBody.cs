using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailBody : MonoBehaviour
{
	private SpriteRenderer _spriteRender;
    private Rigidbody2D _rigidBody2D;
    private float _zRotation;

	private void Awake()
	{
		_rigidBody2D = GetComponent<Rigidbody2D>();
		_spriteRender = GetComponent<SpriteRenderer>();
	}

	public void SetupBody(float yVelocity, float zRotation, int facingDirection)
    {
		if (facingDirection == 1)
			_spriteRender.flipX = true;

		_rigidBody2D.velocity = new Vector2 (_rigidBody2D.velocity.x, yVelocity);
		_zRotation = zRotation;
    }

	private void Update()
	{
		transform.Rotate(new Vector3(0, 0, _zRotation * Time.deltaTime));	
	}
}
