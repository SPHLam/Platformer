using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator _animator;
    private bool _active;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_active)
            return;

        Player player = collision.GetComponent<Player>();
        if (player != null)
            ActivateCheckpoint();
    }
    private void ActivateCheckpoint()
    {
        _active = true;
        _animator.SetTrigger("Activated");
        GameManager.Instance.UpdateCheckpointPosition(transform);
    }
}
