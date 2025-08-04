using DG.Tweening;
using System;

public class Timer
{
    public event Action<int> TickEvent;

    private TweenCallback<int> _tickCallback;
    private TweenCallback _completeCallback;
    private Tweener _tween;

    private int _from;
    private int _to;

    public Timer()
    {

    }

    public void Init(int from, int to, TweenCallback<int> onTick = null, TweenCallback onComplete = null) 
    {
        _tickCallback = onTick;
        _completeCallback = onComplete;
        _from = from;
        _to = to;
        OnTick(_from);
    }

    public void Start() 
    {
        

        _tween = DOVirtual.Int(_from, _to, _from, OnTick).OnComplete(OnComplete).SetEase(Ease.Linear);
    }

    public void Stop() 
    {
        _tween.Kill();
    }

    public void Pause()
    {
        _tween.Pause();
    }

    public void Resume()
    {
        bool active = _tween.IsActive();

        if (!active)
        {
            return;
        }

        _tween.Play();
    }

    private void OnTick(int value) 
    {
        _tickCallback?.Invoke(value);
        TickEvent?.Invoke(value);
    }

    private void OnComplete() 
    {
        _completeCallback?.Invoke();
    }
}

[Serializable]
public class TimeDuration 
{
    public uint Minutes;
    public uint Seconds;

    public TimeDuration(uint minutes, uint secundes) 
    {
        Minutes = minutes;
        Seconds = secundes;
    }

    public uint GetTotalSeconds() 
    {
        return (Minutes * 60) + Seconds;
    }
}
