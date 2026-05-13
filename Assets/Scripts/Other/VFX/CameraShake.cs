using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        Instance = this;

        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float intensity, float time)
    {
        StartCoroutine(ShakeRoutine(intensity, time));
    }

    private IEnumerator ShakeRoutine(float intensity, float time)
    {
        noise.m_AmplitudeGain = intensity;

        yield return new WaitForSeconds(time);

        noise.m_AmplitudeGain = 0f;
    }
}
