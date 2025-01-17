﻿using UnityEngine;

namespace Flow
{
    public class LevelLoader : ILevelLoader
    {
        public WorkState State => mEndTime <= Time.time ? WorkState.Ready : WorkState.Action;

        float mEndTime;

        public void LoadNext()
        {
            mEndTime = Time.time + 2f;
        }
    }
}