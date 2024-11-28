using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
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
}
