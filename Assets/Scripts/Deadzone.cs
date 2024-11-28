using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null )
        {
            player.Damage();
            player.Die();
            GameManager.Instance.RespawnPlayer();
        }

        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null ) 
            enemy.Die();
    }
}
