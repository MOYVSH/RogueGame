using System;
using System.Collections;
using UnityEngine;

public class TimedAction
{
    public float Duration { get; private set; }
    public float RemainingTime { get; private set; }
    public bool IsActive { get; private set; }
    public Action OnStart;
    public Action OnStop;
    public Action<bool> OnUpdate;

    public TimedAction() { }

    public TimedAction(float duration)
    {
        Duration = duration;
    }

    public void Start(float duration, float updateCycleInSeconds)
    {
        if (IsActive)
        {
            Stop();
        }

        IsActive = true;
        Duration = duration;
        RemainingTime = duration;
        _paused = false;
        _updateCycleInSeconds = updateCycleInSeconds;
        if (Application.isPlaying)
        {
            _skillJob = Job.Make(StartSkillCoroutine());
        }
        else
        {
            Debug.Log("no run time no run job ");
        }

        OnStart?.Invoke();
    }

    public void Start()
    {
        Start(Duration);
    }

    public void Start(float duration)
    {
        Start(duration, 0.1f);
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
        if (_skillJob != null)
        {
            _skillJob.Kill();
        }

        IsActive = false;
    }

    public void Stop()
    {
        Kill();
        OnStop?.Invoke();
    }

    public void Restart()
    {
        RemainingTime = Duration;
    }

    public void Restart(float duration)
    {
        Duration = duration;
        RemainingTime = duration;
    }

    private IEnumerator StartSkillCoroutine()
    {
        _lastUpdateTime = Time.time;
        while (RemainingTime >= 0)
        {
            yield return Yielders.Get(RemainingTime > _updateCycleInSeconds ? 
                _updateCycleInSeconds : RemainingTime);

            if (!_paused)
            {
                RemainingTime -= (Time.time - _lastUpdateTime);
            }

            OnUpdate?.Invoke(IsActive);

            _lastUpdateTime = Time.time;
        }

        IsActive = false;
        OnStop?.Invoke();
    }

    private Job _skillJob;
    private bool _paused;
    private float _lastUpdateTime;
    private float _updateCycleInSeconds;
}