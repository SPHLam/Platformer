using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float _pushPower;
    [SerializeField] private float _duration = 0.5f;

    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null )
        {
            _animator.SetTrigger("Activate");
            player.Push(transform.up * _pushPower, _duration);
        }
    }
}
