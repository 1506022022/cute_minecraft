using System;
using UnityEngine;

namespace PlatformGame
{
    public class Timer
    {
        public event Action<Timer> OnStartEvent;
        public event Action<Timer> OnStopEvent;
        public event Action<Timer> OnPauseEvent;
        public event Action<Timer> OnResumeEvent;
        public event Action<Timer> OnTimeoutEvent;
        public event Action<Timer> OnTickEvent;

        bool mbPause = new();
        public bool IsPause
        {
            get => mbPause;
            private set => mbPause = value;
        }

        bool mbStart = new();
        public bool IsStart
        {
            get => mbStart;
            private set => mbStart = value;
        }

        float mTimeout = new();
        public float Timeout
        {
            get => mTimeout;
            private set => mTimeout = value;
        }

        float mElapsedTime = new();
        public float ElapsedTime
        {
            get => mElapsedTime;
            private set => mElapsedTime = value;
        }

        float mLastPauseTime = new();
        public float LastPauseTime
        {
            get => mLastPauseTime;
            private set => mLastPauseTime = value;
        }

        float mLastTickTime = new();
        float LastTickTime
        {
            get => mLastTickTime;
            set => mLastTickTime = value;
        }

        float ServerTime => Server.ServerTime.Time;

        public void Start()
        {
            if (IsStart)
            {
                return;
            }

            IsStart = true;
            IsPause = false;
            ElapsedTime = 0f;
            LastTickTime = ServerTime;
            OnStartEvent?.Invoke(this);
        }

        public void Stop()
        {
            if (IsStart == false)
            {
                return;
            }

            IsStart = false;
            OnStopEvent?.Invoke(this);
        }

        public void Pause()
        {
            if (IsPause)
            {
                return;
            }

            IsPause = true;
            LastPauseTime = ServerTime;
            OnPauseEvent?.Invoke(this);
        }

        public void Resume()
        {
            if (!IsPause)
            {
                return;
            }

            IsPause = false;
            OnResumeEvent?.Invoke(this);
        }

        public void SetTimeout(float timeout)
        {
            Debug.Assert(0 < timeout, $"The timeout({timeout}) must be greater than 0 seconds.");
            Timeout = timeout;
        }

        public void RemoveTimeout()
        {
            Timeout = 0f;
        }

        public void Tick()
        {
            if (!IsStart || IsPause)
            {
                return;
            }

            ElapsedTime += (ServerTime - LastTickTime) - Mathf.Max((LastPauseTime - LastTickTime), 0);
            LastTickTime = ServerTime;
            OnTickEvent?.Invoke(this);

            if (Timeout == 0)
            {
                return;
            }

            if (Timeout < ElapsedTime)
            {
                DoTimeout();
            }
        }

        void DoTimeout()
        {
            IsStart = false;
            OnTimeoutEvent?.Invoke(this);
        }

    }
}