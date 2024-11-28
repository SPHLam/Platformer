using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBackground : MonoBehaviour
{
    [SerializeField] private Vector2 _moveDirection;
    private MeshRenderer _mesh;

    private void Awake()
    {
        _mesh = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        _mesh.material.mainTextureOffset += _moveDirection * Time.deltaTime;
    }
}
