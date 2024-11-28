using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireButton : MonoBehaviour
{
    private Animator _animator;
    private Fire _trapFire;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _trapFire = GetComponentInParent<Fire>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            _animator.SetTrigger("Activate");
            _trapFire.SwitchOffFire();
        }
    }
}
