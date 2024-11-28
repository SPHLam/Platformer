using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _delay = 2f;
    [SerializeField] private Transform[] _wayPoints;
    private Vector3[] _wayPointPosition;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool _canMove = true;
    private int _moveDirection = 1;

    public int wayPointIndex = 1;

    private void Awake()
    {
        _animator = GetComponent<Animator>();   
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        UpdateWaypointsInfo();
        transform.position = _wayPointPosition[0];
    }

    private void UpdateWaypointsInfo()
    {
        List<SawWaypoints> waypointList = new List<SawWaypoints>(GetComponentsInChildren<SawWaypoints>());

        // In case of forgetting to assign a way point
        if (waypointList.Count != _wayPoints.Length)
        {
            _wayPoints = new Transform[waypointList.Count];

            for (int i = 0; i < waypointList.Count; i++)
            {
                _wayPoints[i] = waypointList[i].transform;
            }
        }

        _wayPointPosition = new Vector3[_wayPoints.Length];

        for (int i = 0; i < _wayPoints.Length; i++)
        {
            _wayPointPosition[i] = _wayPoints[i].position;
        }
    }

    private IEnumerator Idle(float delay)
    {
        _canMove = false;
        yield return new WaitForSeconds(delay);
        _canMove = true;
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }

    private void Update()
    {
        _animator.SetBool("isActive", _canMove);
        if (_canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, _wayPointPosition[wayPointIndex], _moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _wayPointPosition[wayPointIndex]) < 0.1f)
            {
                if (wayPointIndex == _wayPointPosition.Length - 1 || wayPointIndex == 0)
                {
                    StartCoroutine(Idle(_delay));
                    _moveDirection *= -1;
                }
                wayPointIndex += _moveDirection;
            }
        }
    }
}
