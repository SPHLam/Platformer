using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("Screen Shake")]
    [SerializeField] private Vector2 _shakeVelocity;

    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        Instance = this;

        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ScreenShake(float shakeDirection)
    {
        _impulseSource.m_DefaultVelocity = new Vector2(_shakeVelocity.x * shakeDirection, _shakeVelocity.y);
        _impulseSource.GenerateImpulse();
    }
}
