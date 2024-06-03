﻿using PlatformGame.Contents.Loader;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PlatformGame.Contents
{
    public class StageLoader : ILevelLoader
    {
        public WorkState State { get; private set; }
        int mStageLevel;
        readonly StageList Stages;
        readonly Slider mProgressBar;
        readonly TextMeshProUGUI mTitle;
        readonly LoadingWindow mLoadingWindow;
        readonly MonoBehaviour mCoroutineRunner;

        public StageLoader()
        {
            // TODO : 래핑
            Stages = Resources.Load<StageList>("StageLevels");
            Debug.Assert(Stages);
            // TODOEND

            Debug.Assert(Stages.Names.Count > 0);

            mLoadingWindow = UIWindowContainer.GetLoadingWindow();
            mCoroutineRunner = mLoadingWindow.CoroutineRunner;
            mTitle = mLoadingWindow.LoadSceneNameText;
            mProgressBar = mLoadingWindow.ProgressBar;
        }

        public void LoadNext()
        {
            var sceneName = Stages.Names[mStageLevel];
            mStageLevel = Mathf.Min(mStageLevel + 1, Stages.Names.Count - 1);
            mCoroutineRunner.StartCoroutine(LoadSceneProcess(sceneName));
        }

        IEnumerator LoadSceneProcess(string sceneName)
        {
            mTitle.text = sceneName;
            var timer = 0f;
            State = WorkState.Action;
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            mLoadingWindow.ShowWindow(true);
#if DEVELOPMENT
            while (true)
            {
                timer += Time.unscaledDeltaTime;
                if (timer > 5f)
                {
                    asyncOperation.allowSceneActivation = true;
                    break;
                }

                mProgressBar.normalizedValue = timer / 5f;

                yield return null;
            }
#else
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress < 0.9f)
                {
                    mProgressBar.normalizedValue = asyncOperation.progress;
                }
                else
                {
                    timer += Time.unscaledDeltaTime;
                    mProgressBar.normalizedValue = Mathf.Lerp(0.9f, 1f, timer);
                    if (mProgressBar.normalizedValue >= 1f)
                    {
                        asyncOperation.allowSceneActivation = true;
                        break;
                    }
                }
                yield return null;
            }
#endif
            mLoadingWindow.ShowWindow(false);
            State = WorkState.Ready;
        }
    }
}