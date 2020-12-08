using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class TimeSpeedChanger : MonoBehaviour
{
    [Range(0.0f, 5.0f)]
    [SerializeField] float m_targetTimeSpeed = 1f;
    [Range(0.0f, 1f)]
    [SerializeField] float m_timeSpeedRateOfChange = 1f;

    [SerializeField] AudioMixer m_slowmoMixer = default;
    [SerializeField] AudioSource m_slowmoSFX = default;

    private float timeVelocity = 0f;
    private bool stop = false;

    void Update()
    {
        if (stop)
            return;

        float newTime = Mathf.SmoothDamp(Time.timeScale, m_targetTimeSpeed, ref timeVelocity, m_timeSpeedRateOfChange);
        m_slowmoMixer.SetFloat("Pitch", newTime);

        Time.timeScale = newTime;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (Time.timeScale >= 0.9f)
        {
            m_slowmoSFX.Stop();
        }
        else
        {
            if (!m_slowmoSFX.isPlaying)
                m_slowmoSFX.Play();
        }
    }

    public void SetTargetTimeSpeed(float targetSpeed)
    {
        m_targetTimeSpeed = targetSpeed;
    }

    public void Resume()
    {
        stop = false;
    }

    public void Stop()
    {
        m_targetTimeSpeed = 0;
        stop = true;
        m_slowmoSFX.Stop();
        m_slowmoMixer.SetFloat("Pitch", 1);
    }
}
