﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpeedChanger : MonoBehaviour
{
    [SerializeField] float m_targetTimeSpeed = 1f;
    [SerializeField] float m_timeSpeedRateOfChange = 1f;

    private float lastTargetTimeSpeed = 1f;
    private float t = 0f;

    void Update()
    {
        t += Time.deltaTime * m_timeSpeedRateOfChange;
        Time.timeScale = Mathf.Lerp(lastTargetTimeSpeed, m_targetTimeSpeed, t);
    }

    public void SetTartTimeSpeed(int targetSpeed)
    {
        m_targetTimeSpeed = targetSpeed;
    }
}