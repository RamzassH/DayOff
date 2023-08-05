using System;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private float _timer;
    
    private void Awake()
    {
        Instance = this;
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void DoShakeCamera(float shakeIntensity, float time)
    {
        CinemachineBasicMultiChannelPerlin _cbmcp =
            _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = shakeIntensity;
        _timer = time;
    }
    
    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;

            if (_timer < 0)
            {
                CinemachineBasicMultiChannelPerlin _cbmcp =
                    _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                _cbmcp.m_AmplitudeGain = 0f;
            }
        }
    }
}
