using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineImpulseSource _cinemachineImpulseSource;
    private float _intensity = 1.8f;

    private void Awake()
    {
        _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeCamera()
    {
        _cinemachineImpulseSource.GenerateImpulseWithForce(_intensity);
    }
}
