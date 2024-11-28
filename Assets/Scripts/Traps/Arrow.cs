using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Trampoline
{
    [SerializeField] private float _rotationSpeed = 120f;
    [SerializeField] private float _coolDown;

    [Space]
    [SerializeField] private float _scaleUpSpeed = 10f;
    [SerializeField] private Vector3 _targetScale;

    private void Start()
    {
        transform.localScale = new Vector3(.3f, .3f, .3f);
    }

    // Update is called once per frame
    void Update()
    {
        HandleScaleUp();
        // Rotate
        HandleRotation();
    }

    private void HandleRotation()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }

    private void HandleScaleUp()
    {
        if (transform.localScale.x < _targetScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, _scaleUpSpeed * Time.deltaTime);
        }
    }

    private void DestroyMe()
    {
        GameObject newArrow = GameManager.Instance.arrowPrefab;
        Destroy(gameObject);
        GameManager.Instance.CreateObject(newArrow, transform, _coolDown);
    }

}
