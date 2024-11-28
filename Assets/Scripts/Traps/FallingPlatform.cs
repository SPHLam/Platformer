using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float _travelDistance;
    [SerializeField] private float _speed = 0.75f;
    private Vector3[] _wayPoints;
    private int _wayPointIndex;
    private bool _canMove = true;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D[] _colliders;

    [Header("Platform fall details")]
    [SerializeField] private float _impactSpeed = 3f;
    [SerializeField] private float _impactDuration = 0.1f;
    private float _impactTimer;
    private bool _impactHappened;
    [Space]
    [SerializeField] private float _fallDelay = 0.5f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _colliders = GetComponents<BoxCollider2D>();    
    }

    private IEnumerator Start()
    {
        SetupWaypoints();

        float randomDelay = Random.Range(0, 0.6f);
        yield return new WaitForSeconds(randomDelay);
        Invoke(nameof(ActivatePlatform), randomDelay);
    }

    private void Update()
    {
        HandleMovement();
        HandleImpact();
    }

    private void SetupWaypoints()
    {
        _wayPoints = new Vector3[2];

        float yOffset = _travelDistance / 2;

        _wayPoints[0] = transform.position + new Vector3(0, yOffset, 0);
        _wayPoints[1] = transform.position + new Vector3(0, -yOffset, 0);
    }

    private void ActivatePlatform()
    {
        _canMove = true;
    }

    private void HandleMovement()
    {
        if (_canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, _wayPoints[_wayPointIndex], _speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _wayPoints[_wayPointIndex]) < 0.1f)
            {
                _wayPointIndex++;

                if (_wayPointIndex >= _wayPoints.Length)
                {
                    _wayPointIndex = 0;
                }
            }
        }
    }

    private void HandleImpact()
    {
        if (_impactTimer < 0f)
            return;

        _impactTimer -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3.down * 10f), _impactSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_impactHappened)
        {
            return;
        }
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            Invoke(nameof(SwitchOffPlatform), _fallDelay);
            _impactTimer = _impactDuration;
            _impactHappened = true;
        }
    }

    private void SwitchOffPlatform()
    {
        _animator.SetTrigger("Deactivate");

        _canMove = false;
        _rigidbody.isKinematic = false;
        _rigidbody.gravityScale = 3.5f;
        _rigidbody.drag = 0.5f;

        foreach(BoxCollider2D collider in _colliders)
        {
            collider.enabled = false;
        }
    }
}
