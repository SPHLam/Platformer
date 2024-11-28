using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _spikeRigidBody;
    [SerializeField] private float _pushForce;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 pushVector = new Vector2(_pushForce, 0);
        _spikeRigidBody.AddForce(pushVector, ForceMode2D.Impulse);
    }
}
