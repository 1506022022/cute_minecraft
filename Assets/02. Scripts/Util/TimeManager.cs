using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float mTimeScale;

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = mTimeScale;
    }

    public void SetScale(float t)
    {
        mTimeScale = t;
    }
}