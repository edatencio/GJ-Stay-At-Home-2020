using UnityEngine;

public class Timer
{
    private float startTime;
    private bool ranOnce;

    public bool isRunning { get; private set; }

    public float ElapsedSeconds
    {
        get
        {
            if (ranOnce)
                return Time.time - startTime;
            else
                return 0f;
        }
    }

    public void Start()
    {
        ranOnce = true;
        isRunning = true;
        startTime = Time.time;
    }

    public void Stop()
    {
        isRunning = false;
    }
}
