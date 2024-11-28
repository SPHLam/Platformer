using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private float _offDuration;
    [SerializeField] private FireButton _fireButton;

    private Animator _animator;
    private CapsuleCollider2D _capsuleCollider;
    private bool _isActive;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        if (_fireButton == null)
            Debug.LogWarning("No fire button on " + gameObject.name);

        SetFire(true);
    }

    private IEnumerator FireCoroutine()
    {
        SetFire(false);
        yield return new WaitForSeconds(_offDuration);
        SetFire(true);
    }

    private void SetFire(bool active)
    {
        _animator.SetBool("isActive", active);
        _capsuleCollider.enabled = active;
        _isActive = active;
    }

    public void SwitchOffFire()
    {
        if (!_isActive)
            return;
            
        StartCoroutine(FireCoroutine());
    }
}
