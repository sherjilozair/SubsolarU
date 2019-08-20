using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public static Clock Instance;

    public float TimeRate = 1f;

    public TMPro.TextMeshProUGUI FPS;

    public float DeltaTime
    {
        get
        {
            return Time.deltaTime * TimeRate;
        }
    }

    public float TimeMult
    {
        get
        {
            return Time.deltaTime * TimeRate * 60;
        }
    }

    void Awake()
    {
        Instance = this;
    }

    float StartRate;
    float TargetRate;
    float LerpTimer;
    float LerpTime;
    bool Lerping;
    Action Callback;
    public void LerpTo(float target, float time, Action callback = null)
    {
        StartRate = TimeRate;
        TargetRate = target;
        LerpTimer = time;
        LerpTime = time;
        Callback = callback;
        Lerping = true;
    }

    void Update()
    {
        if (Lerping)
        {
            if (LerpTimer > 0)
            {
                float ratio = 1 - LerpTimer / LerpTime;
                TimeRate = Mathf.Lerp(StartRate, TargetRate, ratio);
                LerpTimer = Mathf.Max(0, LerpTimer - Time.deltaTime);
            }
            else
            {
                TimeRate = TargetRate;
                Lerping = false;
                Callback?.Invoke();
            }
        }
    }
}
