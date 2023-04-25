using System;
using System.Collections;
using UnityEngine;

public class TimedActionUpdater
{
    public float Duration { get; private set; }
    public float RemainingTime { get; private set; }
    public bool IsActive { get; private set; }
    public Action OnStart = delegate { };
    public Action OnStop = delegate { };
    public Action OnUpdate = delegate { };

    public void Start(float duration)
    {
        if (IsActive)
        {
            Stop();
        }

        IsActive = true;
        Duration = duration;
        RemainingTime = duration;
        _paused = false;

        OnStart();
    }

    public void Pause()
    {
        _paused = true;
    }

    public void Unpause()
    {
        _paused = false;
    }

    public void Kill()
    {
        IsActive = false;
    }

    public void Stop()
    {
        Kill();
        OnStop();
    }

    public void Restart(float duration)
    {
        Duration = duration;
        RemainingTime = duration;
    }

    public void Update(float deltaTime)
    {
        if (IsActive && !_paused)
        {
            RemainingTime -= deltaTime;
            OnUpdate();

            if (RemainingTime <= 0)
            {
                IsActive = false;
                OnStop();
            }
        }
    }

    private bool _paused;
}
